using System.Net;
using System.Security.Cryptography;

using Carbon.Storage;

namespace Amazon.S3.Tests;

public class S3BucketTests
{
    private const string Ns = "http://s3.amazonaws.com/doc/2006-03-01/";

    private const int MaxAttempts = 4;

    private static readonly byte[] s_key = Enumerable.Range(1, 32).Select(static i => (byte)i).ToArray();

    private static string ListResult(string? nextToken, params string[] keys)
    {
        var contents = string.Concat(keys.Select(static key => $"<Contents><Key>{key}</Key></Contents>"));
        var token = nextToken is null ? "" : $"<NextContinuationToken>{nextToken}</NextContinuationToken>";

        return $"""<ListBucketResult xmlns="{Ns}"><Name>bucket</Name>{token}{contents}</ListBucketResult>""";
    }

    private static async Task<List<string>> ScanKeysAsync(S3Bucket bucket, string? prefix, int take = 1_000_000_000)
    {
        var keys = new List<string>();

        await foreach (var blob in bucket.ScanAsync(prefix, take))
        {
            keys.Add(blob.Key);
        }

        return keys;
    }

    [Fact]
    public void ValidatesArguments()
    {
        var client = FakeS3.CreateClient(FakeS3Handler.Sequence(FakeS3.Unavailable));

        Assert.Throws<ArgumentNullException>(() => new S3Bucket(null, client));
        Assert.Throws<ArgumentException>(() => new S3Bucket("", client));
        Assert.Throws<ArgumentNullException>(() => new S3Bucket("bucket", null));
    }

    [Fact]
    public void WithTimeout_ReturnsBucket()
    {
        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        Assert.Equal("bucket", bucket.Name);
        Assert.Same(bucket, bucket.WithTimeout(TimeSpan.FromSeconds(1)));
    }

    [Fact]
    public void GetPresignedUrl()
    {
        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        var url = bucket.GetPresignedUrl("key", TimeSpan.FromMinutes(5));

        Assert.StartsWith("https://s3.test/bucket/key?X-Amz-Algorithm=AWS4-HMAC-SHA256", url);
        Assert.Contains("X-Amz-Expires=300", url);
        Assert.Contains("X-Amz-Signature=", url);
    }

    [Fact]
    public async Task ScanAsync_FollowsContinuationTokens()
    {
        var handler = FakeS3Handler.Sequence(
            () => FakeS3.Xml(ListResult("next", "a", "b")),
            () => FakeS3.Xml(ListResult(null, "c")));

        var keys = await ScanKeysAsync(FakeS3.CreateBucket(handler), "prefix");

        Assert.Equal(["a", "b", "c"], keys);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Contains("prefix=prefix", handler.Requests[0].Uri);
        Assert.DoesNotContain("continuation-token", handler.Requests[0].Uri);
        Assert.Contains("continuation-token=next", handler.Requests[1].Uri);
    }

    [Fact]
    public async Task ScanAsync_StopsAtTake()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(ListResult("next", "a", "b", "c")));

        var keys = await ScanKeysAsync(FakeS3.CreateBucket(handler), null, take: 2);

        Assert.Equal(["a", "b"], keys);
        Assert.Single(handler.Requests);
        Assert.Contains("max-keys=2", handler.Requests[0].Uri);
    }

    [Fact]
    public async Task ScanAsync_RequestsOnlyWhatIsLeft()
    {
        var handler = FakeS3Handler.Sequence(
            () => FakeS3.Xml(ListResult("next", "a", "b", "c")),
            () => FakeS3.Xml(ListResult(null, "d", "e")));

        var keys = await ScanKeysAsync(FakeS3.CreateBucket(handler), null, take: 5);

        Assert.Equal(["a", "b", "c", "d", "e"], keys);
        Assert.Contains("max-keys=5", handler.Requests[0].Uri);
        Assert.Contains("max-keys=2", handler.Requests[1].Uri);
    }

    [Fact]
    public async Task PutAsync_CanceledBeforeHashing()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"a\""));

        using var blob = new Blob("key", new MemoryStream([1]));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => FakeS3.CreateBucket(handler).PutAsync(blob, new CancellationToken(canceled: true)));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Upload_RetriesWhenResponseBodyFails()
    {
        var upload = new InitiateMultipartUploadResult { Bucket = "bucket", Key = "key", UploadId = "u1" };

        var handler = FakeS3Handler.Sequence(
            () => new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StreamContent(new FakeBodyStream(static _ => throw new IOException("The response ended prematurely")))
            },
            () => FakeS3.Xml(CompleteResult()));

        await FakeS3.CreateBucket(handler).FinalizeUploadAsync(upload, []);

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task ScanAsync_StopsWhenCanceledBetweenPages()
    {
        var handler = new FakeS3Handler((_, cancellationToken) => {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(FakeS3.Xml(ListResult("next", "a")));
        });

        using var cts = new CancellationTokenSource();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => {
            await foreach (var blob in FakeS3.CreateBucket(handler).ScanAsync(null).WithCancellation(cts.Token))
            {
                cts.Cancel();
            }
        });

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task ScanAsync_LimitsPageSize()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(ListResult(null, "a")));

        await ScanKeysAsync(FakeS3.CreateBucket(handler), null, take: 5_000);

        Assert.Contains("max-keys=1000", handler.Requests[0].Uri);
    }

    [Fact]
    public async Task ScanAsync_EmptyResultReturnsNothing()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(ListResult(null)));

        Assert.Empty(await ScanKeysAsync(FakeS3.CreateBucket(handler), "missing"));
    }

    [Fact]
    public async Task ListAsync_EmptyResultReturnsEmptyList()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(ListResult(null)));

        Assert.Empty(await FakeS3.CreateBucket(handler).ListAsync("missing"));
    }

    [Fact]
    public async Task ScanAsync_RejectsInvalidTake()
    {
        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ScanKeysAsync(bucket, null, take: 0));
    }

    [Fact]
    public async Task ListAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(ListResult("next", "a", "b")));

        var bucket = FakeS3.CreateBucket(handler);

        var result = await bucket.ListAsync("prefix");

        Assert.Equal(["a", "b"], result.Select(static blob => blob.Key));
        Assert.Contains("max-keys=1000", handler.Requests[0].Uri);

        await bucket.ListAsync(null, "token", take: 5);

        Assert.Contains("continuation-token=token", handler.Requests[1].Uri);
        Assert.Contains("max-keys=5", handler.Requests[1].Uri);
    }

    [Fact]
    public async Task GetAsync_RetriesTransientFailures()
    {
        var handler = FakeS3Handler.Sequence(
            FakeS3.Unavailable,
            FakeS3.BrokenPipe,
            () => FakeS3.Blob([1, 2, 3]));

        using var blob = await FakeS3.CreateBucket(handler).GetAsync("key");

        Assert.Equal("key", blob.Key);
        Assert.Equal(3, blob.Size);
        Assert.Equal(3, handler.Requests.Count);
    }

    [Fact]
    public async Task GetAsync_DoesNotRetryPermanentFailures()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Error("AccessDenied", HttpStatusCode.Forbidden));

        await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateBucket(handler).GetAsync("key"));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task GetAsync_ThrowsLastErrorWhenRetriesAreExhausted()
    {
        var handler = FakeS3Handler.Sequence(FakeS3.Unavailable);

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateBucket(handler).GetAsync("key"));

        Assert.Equal("SlowDown", ex.Error!.Code);
        Assert.Equal(MaxAttempts, handler.Requests.Count);
    }

    [Fact]
    public async Task GetAsync_Options_SetsHeaders()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Blob([1, 2, 3]));

        using var result = await FakeS3.CreateBucket(handler).GetAsync("key", new GetBlobOptions {
            IfModifiedSince = new DateTimeOffset(2020, 01, 01, 01, 01, 01, TimeSpan.Zero),
            IfNoneMatch     = "\"abc\"",
            Range           = new ByteRange(1, 2),
            EncryptionKey   = s_key,
            BufferResponse  = true
        });

        var headers = handler.Requests[0].Headers;

        Assert.Equal("Wed, 01 Jan 2020 01:01:01 GMT", headers["If-Modified-Since"]);
        Assert.Equal("\"abc\"", headers["If-None-Match"]);
        Assert.Equal("bytes=1-2", headers["Range"]);
        AssertEncryptionHeaders(headers);
    }

    [Fact]
    public async Task GetAsync_Options_ReturnsNotModified()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.NotModified));

        using var result = await FakeS3.CreateBucket(handler).GetAsync("key", new GetBlobOptions { IfNoneMatch = "\"abc\"" });

        Assert.Equal(HttpStatusCode.NotModified, ((IBlobResult)result).StatusCode);
    }

    [Fact]
    public async Task GetAsync_Options_RetriesTransientFailures()
    {
        var handler = FakeS3Handler.Sequence(
            FakeS3.Unavailable,
            FakeS3.BrokenPipe,
            () => FakeS3.Blob([1]));

        using var result = await FakeS3.CreateBucket(handler).GetAsync("key", new GetBlobOptions());

        Assert.Equal(3, handler.Requests.Count);
    }

    [Fact]
    public async Task GetAsync_Options_ThrowsWhenExhausted()
    {
        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        await Assert.ThrowsAsync<ArgumentNullException>(() => bucket.GetAsync("key", (GetBlobOptions)null));
        await Assert.ThrowsAsync<S3Exception>(() => bucket.GetAsync("key", new GetBlobOptions()));
    }

    [Fact]
    public async Task GetPropertiesAsync_RetriesTransientFailures()
    {
        var handler = FakeS3Handler.Sequence(
            FakeS3.Unavailable,
            () => FakeS3.Head(42));

        var properties = await FakeS3.CreateBucket(handler).GetPropertiesAsync("key");

        Assert.Equal("42", properties["Content-Length"]);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Same(HttpMethod.Head, handler.Requests[0].Method);
    }

    [Fact]
    public async Task GetPropertiesAsync_ThrowsWhenExhausted()
    {
        var handler = FakeS3Handler.Sequence(FakeS3.Unavailable);

        await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateBucket(handler).GetPropertiesAsync("key"));

        Assert.Equal(MaxAttempts, handler.Requests.Count);
    }

    [Fact]
    public async Task InitiateRestoreAsync_SendsRequestedDays()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.Accepted));

        await FakeS3.CreateBucket(handler).InitiateRestoreAsync("key", 30);

        Assert.Contains("<Days>30</Days>", handler.Requests[0].BodyText);
    }

    [Fact]
    public async Task InitiateRestoreAsync_ReturnsStatus()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.Accepted));

        var result = await FakeS3.CreateBucket(handler).InitiateRestoreAsync("key", 30);

        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
    }

    private static string CopyResult() => $"""<CopyObjectResult xmlns="{Ns}"><ETag>"abc"</ETag></CopyObjectResult>""";

    [Fact]
    public async Task PutAsync_Copy_KeepsMetadataByDefault()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(CopyResult()));

        var result = await FakeS3.CreateBucket(handler).PutAsync("target", new S3ObjectLocation("source", "a"));

        var request = handler.Requests[0];

        Assert.Equal("\"abc\"", result.ETag);
        Assert.Equal("https://s3.test/bucket/target", request.Uri);
        Assert.Equal("/source/a", request.Headers["x-amz-copy-source"]);
        Assert.False(request.Headers.ContainsKey("x-amz-metadata-directive"));
    }

    [Fact]
    public async Task PutAsync_Copy_ReplacesMetadata()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(CopyResult()));

        await FakeS3.CreateBucket(handler).PutAsync("target", new S3ObjectLocation("source", "a"), new Dictionary<string, string> {
            { "x-amz-meta-color", "red" }
        });

        var headers = handler.Requests[0].Headers;

        Assert.Equal("REPLACE", headers["x-amz-metadata-directive"]);
        Assert.Equal("red", headers["x-amz-meta-color"]);
    }

    [Fact]
    public async Task PutAsync_Copy_RetriesTransientFailures()
    {
        var handler = FakeS3Handler.Sequence(
            FakeS3.Unavailable,
            () => FakeS3.Xml(CopyResult()));

        await FakeS3.CreateBucket(handler).PutAsync("target", new S3ObjectLocation("source", "a"));

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task PutAsync_Copy_WrapsLastErrorWhenExhausted()
    {
        var handler = FakeS3Handler.Sequence(FakeS3.Unavailable);

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateBucket(handler).PutAsync("target", new S3ObjectLocation("source", "a")));

        Assert.Contains("'target'", ex.Message);
        Assert.Equal("SlowDown", ex.Error!.Code);
        Assert.IsType<S3Exception>(ex.InnerException);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.HttpStatusCode);
    }

    [Fact]
    public async Task PutAsync_Blob_SendsContentAndHash()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"abc\""));

        byte[] data = [1, 2, 3];

        using var blob = new Blob("key", new MemoryStream(data), new Dictionary<string, string> {
            { "Content-Type", "image/png" },
            { "Content-Encoding", "br" },
            { "Content-Length", "999" },
            { "ETag", "skipped" },
            { "x-amz-meta-color", "red" }
        });

        await FakeS3.CreateBucket(handler).PutAsync(blob);

        var request = handler.Requests[0];

        Assert.Same(HttpMethod.Put, request.Method);
        Assert.Equal("https://s3.test/bucket/key", request.Uri);
        Assert.Equal(data, request.Body);
        Assert.Equal(Convert.ToHexStringLower(SHA256.HashData(data)), request.Headers["x-amz-content-sha256"]);
        Assert.Equal("image/png", request.Headers["Content-Type"]);
        Assert.Equal("br", request.Headers["Content-Encoding"]);
        Assert.Equal("3", request.Headers["Content-Length"]);
        Assert.Equal("red", request.Headers["x-amz-meta-color"]);
        Assert.False(request.Headers.ContainsKey("ETag"));
    }

    [Fact]
    public async Task PutAsync_Blob_SetsEncryptionHeaders()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"abc\""));

        using var blob = new Blob("key", new MemoryStream([1]));

        await FakeS3.CreateBucket(handler).PutAsync(blob, new PutBlobOptions(s_key));

        AssertEncryptionHeaders(handler.Requests[0].Headers);
    }

    [Fact]
    public async Task PutAsync_Blob_RejectsInvalidEncryptionKey()
    {
        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(() => FakeS3.WithETag("\"abc\"")));

        using var blob = new Blob("key", new MemoryStream([1]));

        await Assert.ThrowsAsync<ArgumentException>(() => bucket.PutAsync(blob, new PutBlobOptions(new byte[16])));
    }

    [Fact]
    public async Task PutAsync_Blob_EncodesTags()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"abc\""));

        using var blob = new Blob("key", new MemoryStream([1]));

        await FakeS3.CreateBucket(handler).PutAsync(blob, new PutBlobOptions(new KeyValuePair<string, string>[] {
            new("a b", "c&d"),
            new("e", "f")
        }));

        Assert.Equal("a%20b=c%26d&e=f", handler.Requests[0].Headers["x-amz-tagging"]);
    }

    [Fact]
    public async Task PutAsync_Blob_SupportsLongTagSets()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"abc\""));

        var tags = Enumerable.Range(0, 10)
            .Select(static i => new KeyValuePair<string, string>($"key{i}", new string('v', 256)))
            .ToArray();

        using var blob = new Blob("key", new MemoryStream([1]));

        await FakeS3.CreateBucket(handler).PutAsync(blob, new PutBlobOptions(tags));

        Assert.StartsWith($"key0={new string('v', 256)}&key1=", handler.Requests[0].Headers["x-amz-tagging"]);
    }

    public static TheoryData<KeyValuePair<string, string>[]> InvalidTags => new() {
        Enumerable.Range(0, 11).Select(static i => new KeyValuePair<string, string>($"key{i}", "value")).ToArray(),
        new KeyValuePair<string, string>[] { new(new string('k', 129), "value") },
        new KeyValuePair<string, string>[] { new("key", new string('v', 257)) }
    };

    [Theory]
    [MemberData(nameof(InvalidTags))]
    public async Task PutAsync_Blob_RejectsInvalidTags(KeyValuePair<string, string>[] tags)
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"abc\""));

        using var blob = new Blob("key", new MemoryStream([1]));

        await Assert.ThrowsAsync<ArgumentException>(() => FakeS3.CreateBucket(handler).PutAsync(blob, new PutBlobOptions(tags)));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task PutAsync_Blob_ValidatesArguments()
    {
        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        await Assert.ThrowsAsync<ArgumentNullException>(() => bucket.PutAsync(null));

        await Assert.ThrowsAsync<ArgumentException>(() => bucket.PutAsync(new StubBlob(null, [1])));
        await Assert.ThrowsAsync<ArgumentException>(() => bucket.PutAsync(new StubBlob("key", [])));

        using var moved = new Blob("key", new MemoryStream([1, 2]) { Position = 1 });

        await Assert.ThrowsAsync<ArgumentException>(() => bucket.PutAsync(moved));
    }

    [Fact]
    public async Task PutAsync_Blob_ResendsWholeStreamOnRetry()
    {
        var handler = FakeS3Handler.Sequence(
            FakeS3.Unavailable,
            FakeS3.BrokenPipe,
            () => FakeS3.WithETag("\"abc\""));

        using var blob = new Blob("key", new MemoryStream([1, 2, 3]));

        await FakeS3.CreateBucket(handler).PutAsync(blob);

        Assert.Equal(3, handler.Requests.Count);
        Assert.All(handler.Requests, static request => Assert.Equal([1, 2, 3], request.Body));
    }

    [Fact]
    public async Task PutAsync_Blob_ThrowsWhenExhausted()
    {
        var handler = FakeS3Handler.Sequence(FakeS3.Unavailable);

        using var blob = new Blob("key", new MemoryStream([1]));

        await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateBucket(handler).PutAsync(blob));

        Assert.Equal(MaxAttempts, handler.Requests.Count);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.NoContent));

        var bucket = FakeS3.CreateBucket(handler);

        await bucket.DeleteAsync("key");
        await bucket.DeleteAsync("key", "v1");

        Assert.Equal("https://s3.test/bucket/key", handler.Requests[0].Uri);
        Assert.Equal("https://s3.test/bucket/key?versionId=v1", handler.Requests[1].Uri);
    }

    [Fact]
    public async Task DeleteAsync_Batch()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml($"""<DeleteResult xmlns="{Ns}"></DeleteResult>"""));

        var result = await FakeS3.CreateBucket(handler).DeleteAsync(["a", "b"]);

        Assert.False(result.HasErrors);

        var request = handler.Requests[0];

        Assert.Equal("https://s3.test/bucket?delete", request.Uri);
        Assert.Contains("<Quiet>true</Quiet>", request.BodyText);
        Assert.Contains("<Key>a</Key>", request.BodyText);
        Assert.Contains("<Key>b</Key>", request.BodyText);
    }

    [Fact]
    public async Task DeleteAsync_Batch_ThrowsOnPartialFailure()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(
            $"""<DeleteResult xmlns="{Ns}"><Error><Key>a</Key><Code>AccessDenied</Code><Message>Access Denied</Message></Error></DeleteResult>"""));

        var ex = await Assert.ThrowsAsync<Exception>(() => FakeS3.CreateBucket(handler).DeleteAsync(["a"]));

        Assert.Equal("Access Denied", ex.Message);
    }

    private static string InitiateResult() =>
        $"""<InitiateMultipartUploadResult xmlns="{Ns}"><Bucket>bucket</Bucket><Key>key</Key><UploadId>u1</UploadId></InitiateMultipartUploadResult>""";

    private static string CompleteResult() =>
        $"""<CompleteMultipartUploadResult xmlns="{Ns}"><Bucket>bucket</Bucket><Key>key</Key><ETag>"final"</ETag></CompleteMultipartUploadResult>""";

    [Fact]
    public async Task Upload_EndToEnd()
    {
        var handler = FakeS3Handler.Sequence(
            () => FakeS3.Xml(InitiateResult()),
            () => FakeS3.WithETag("\"part1\""),
            () => FakeS3.Xml(CompleteResult()));

        var bucket = FakeS3.CreateBucket(handler);

        var upload = await bucket.InitiateUploadAsync("key", new Dictionary<string, string> {
            { "Content-Type", "video/mp4" }
        });

        Assert.Equal("u1", upload.UploadId);
        Assert.Equal("bucket", upload.BucketName);
        Assert.Equal("key", upload.ObjectName);

        var block = await bucket.UploadPartAsync(upload, 1, new MemoryStream([1, 2, 3]));

        Assert.Equal(1, block.Number);
        Assert.Equal("\"part1\"", block.BlockId);
        Assert.Equal("u1", block.UploadId);
        Assert.Null(block.Range);

        await bucket.FinalizeUploadAsync(upload, [block]);

        Assert.Equal([1, 2, 3], handler.Requests[1].Body);
        Assert.Contains("<PartNumber>1</PartNumber>", handler.Requests[2].BodyText);
    }

    [Fact]
    public async Task Upload_RetriesTransientFailures()
    {
        var handler = FakeS3Handler.Sequence(
            FakeS3.Unavailable,
            FakeS3.BrokenPipe,
            () => FakeS3.Xml(InitiateResult()),
            FakeS3.Unavailable,
            FakeS3.BrokenPipe,
            () => FakeS3.WithETag("\"part1\""),
            FakeS3.Unavailable,
            FakeS3.BrokenPipe,
            () => FakeS3.Xml(CompleteResult()));

        var bucket = FakeS3.CreateBucket(handler);

        var upload = await bucket.InitiateUploadAsync("key", new Dictionary<string, string>());
        var block  = await bucket.UploadPartAsync(upload, 1, new MemoryStream([1, 2, 3]));

        await bucket.FinalizeUploadAsync(upload, [block]);

        Assert.Equal(9, handler.Requests.Count);
        Assert.All(handler.Requests.Skip(3).Take(3), static request => Assert.Equal([1, 2, 3], request.Body));
    }

    [Fact]
    public async Task Upload_ThrowsWhenExhausted()
    {
        var upload = new InitiateMultipartUploadResult { Bucket = "bucket", Key = "key", UploadId = "u1" };

        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        await Assert.ThrowsAsync<S3Exception>(() => bucket.InitiateUploadAsync("key", new Dictionary<string, string>()));
        await Assert.ThrowsAsync<S3Exception>(() => bucket.UploadPartAsync(upload, 1, new MemoryStream([1])));
        await Assert.ThrowsAsync<S3Exception>(() => bucket.FinalizeUploadAsync(upload, []));
    }

    [Fact]
    public async Task Upload_ValidatesArguments()
    {
        var upload = new InitiateMultipartUploadResult { Bucket = "bucket", Key = "key", UploadId = "u1" };

        var bucket = FakeS3.CreateBucket(FakeS3Handler.Sequence(FakeS3.Unavailable));

        await Assert.ThrowsAsync<ArgumentNullException>(() => bucket.UploadPartAsync(null, 1, new MemoryStream([1])));
        await Assert.ThrowsAsync<ArgumentNullException>(() => bucket.UploadPartAsync(upload, 1, null));
        await Assert.ThrowsAsync<ArgumentNullException>(() => bucket.CancelUploadAsync(null));
    }

    [Fact]
    public async Task CancelUploadAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.NoContent));

        var upload = new InitiateMultipartUploadResult { Bucket = "bucket", Key = "key", UploadId = "u1" };

        await FakeS3.CreateBucket(handler).CancelUploadAsync(upload);

        Assert.Same(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.Equal("https://s3.test/bucket/key?uploadId=u1", handler.Requests[0].Uri);
    }

    private sealed class StubBlob(string key, byte[] data) : IBlob
    {
        public string Key => key;

        public long Size => data.Length;

        public DateTime Modified => default;

        public IReadOnlyDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public ValueTask<Stream> OpenAsync() => new(new MemoryStream(data));

        public void Dispose() { }
    }

    private static void AssertEncryptionHeaders(Dictionary<string, string> headers)
    {
        Assert.Equal("AES256", headers["x-amz-server-side-encryption-customer-algorithm"]);
        Assert.Equal(Convert.ToBase64String(s_key), headers["x-amz-server-side-encryption-customer-key"]);
        Assert.Equal(Convert.ToBase64String(MD5.HashData(s_key)), headers["x-amz-server-side-encryption-customer-key-MD5"]);
    }
}
