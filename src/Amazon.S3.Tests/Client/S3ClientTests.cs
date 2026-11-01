using System.Net;

using Carbon.Storage;

namespace Amazon.S3.Tests;

public class S3ClientTests
{
    private const string Ns = "http://s3.amazonaws.com/doc/2006-03-01/";

    [Fact]
    public void DefaultsToDualStackHost()
    {
        var client = new S3Client(AwsRegion.USEast1, new AwsCredential("public", "private"));

        Assert.Equal("s3.dualstack.us-east-1.amazonaws.com", client.Host);
    }

    [Fact]
    public void RequiresHost()
    {
        Assert.Throws<ArgumentNullException>(() => new S3Client(AwsRegion.USEast1, null, new AwsCredential("public", "private"), new HttpClient()));
    }

    [Fact]
    public void WithTimeout_SetsHttpClientTimeout()
    {
        var httpClient = new HttpClient();

        var client = new S3Client(AwsRegion.USEast1, "s3.test", new AwsCredential("public", "private"), httpClient);

        Assert.Same(client, client.WithTimeout(TimeSpan.FromSeconds(7)));
        Assert.Equal(TimeSpan.FromSeconds(7), httpClient.Timeout);
    }

    [Fact]
    public async Task SignsRequests()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.WithETag("\"a\""));

        var request = new PutObjectRequest("s3.test", "bucket", "key");

        request.SetStream(new MemoryStream([1]));

        await FakeS3.CreateClient(handler).PutObjectAsync(request);

        var headers = handler.Requests[0].Headers;

        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", headers["Authorization"]);
        Assert.Contains("/us-east-1/s3/aws4_request", headers["Authorization"]);
        Assert.True(headers.ContainsKey("x-amz-date"));
    }

    [Fact]
    public async Task ListBucketAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(
            $"""
            <ListBucketResult xmlns="{Ns}">
              <Name>bucket</Name>
              <Contents><Key>a.txt</Key><Size>5</Size></Contents>
            </ListBucketResult>
            """));

        var result = await FakeS3.CreateClient(handler).ListBucketAsync("bucket", new ListBucketOptions { Prefix = "a", MaxKeys = 2 });

        Assert.Equal("a.txt", Assert.Single(result.Items).Key);

        Assert.StartsWith("https://s3.test/bucket?", handler.Requests[0].Uri);
        Assert.Contains("prefix=a", handler.Requests[0].Uri);
        Assert.Contains("max-keys=2", handler.Requests[0].Uri);
    }

    [Fact]
    public async Task ListObjectVersionsAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml($"""<ListVersionsResult xmlns="{Ns}"></ListVersionsResult>"""));

        var result = await FakeS3.CreateClient(handler).ListObjectVersionsAsync("bucket", new ListVersionsOptions { Prefix = "a" });

        Assert.NotNull(result);
        Assert.Equal("https://s3.test/bucket?versions&prefix=a", handler.Requests[0].Uri);
    }

    [Fact]
    public async Task MultipartUpload()
    {
        var handler = FakeS3Handler.Sequence(
            () => FakeS3.Xml($"""<InitiateMultipartUploadResult xmlns="{Ns}"><Bucket>bucket</Bucket><Key>key</Key><UploadId>u1</UploadId></InitiateMultipartUploadResult>"""),
            () => FakeS3.WithETag("\"part1\""),
            () => FakeS3.Xml($"""<CompleteMultipartUploadResult xmlns="{Ns}"><Bucket>bucket</Bucket><Key>key</Key><ETag>"final"</ETag></CompleteMultipartUploadResult>"""));

        var client = FakeS3.CreateClient(handler);

        var upload = await client.InitiateMultipartUploadAsync(new InitiateMultipartUploadRequest("s3.test", "bucket", "key"));

        Assert.Equal("u1", upload.UploadId);

        var partRequest = new UploadPartRequest("s3.test", "bucket", "key", upload.UploadId, 1);

        partRequest.SetStream(new MemoryStream([1, 2, 3]));

        var part = await client.UploadPartAsync(partRequest);

        Assert.Equal("u1", part.UploadId);
        Assert.Equal(1, part.PartNumber);
        Assert.Equal("\"part1\"", part.ETag);

        var result = await client.CompleteMultipartUploadAsync(new CompleteMultipartUploadRequest("s3.test", upload, [part]));

        Assert.Equal("\"final\"", result.ETag);

        Assert.Equal("https://s3.test/bucket/key?uploads", handler.Requests[0].Uri);
        Assert.Equal("https://s3.test/bucket/key?partNumber=1&uploadId=u1", handler.Requests[1].Uri);
        Assert.Equal("https://s3.test/bucket/key?uploadId=u1", handler.Requests[2].Uri);
        Assert.Contains("<ETag>\"part1\"</ETag>", System.Net.WebUtility.HtmlDecode(handler.Requests[2].BodyText));
    }

    [Fact]
    public async Task AbortMultipartUploadAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.NoContent));

        await FakeS3.CreateClient(handler).AbortMultipartUploadAsync(new AbortMultipartUploadRequest("s3.test", "bucket", "key", "u1"));

        Assert.Same(HttpMethod.Delete, handler.Requests[0].Method);
        Assert.Equal("https://s3.test/bucket/key?uploadId=u1", handler.Requests[0].Uri);
    }

    [Fact]
    public async Task PutObjectAsync_ReturnsETagAndVersion()
    {
        var handler = FakeS3Handler.Sequence(() => {
            var response = FakeS3.WithETag("\"abc\"");

            response.Headers.TryAddWithoutValidation("x-amz-version-id", "v1");

            return response;
        });

        var request = new PutObjectRequest("s3.test", "bucket", "key");

        request.SetStream(new MemoryStream([1, 2, 3]));

        var result = await FakeS3.CreateClient(handler).PutObjectAsync(request);

        Assert.Equal("\"abc\"", result.ETag);
        Assert.Equal("v1", result.VersionId);
        Assert.Equal([1, 2, 3], handler.Requests[0].Body);
    }

    [Fact]
    public async Task CopyObjectAsync()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(
            $"""<CopyObjectResult xmlns="{Ns}"><LastModified>2008-02-20T22:13:01</LastModified><ETag>"abc"</ETag></CopyObjectResult>"""));

        var result = await FakeS3.CreateClient(handler).CopyObjectAsync(new CopyObjectRequest(
            host        : "s3.test",
            source      : new S3ObjectLocation("source", "a"),
            destination : new S3ObjectLocation("target", "b")
        ));

        Assert.Equal("\"abc\"", result.ETag);
        Assert.Equal("/source/a", handler.Requests[0].Headers["x-amz-copy-source"]);
    }

    [Fact]
    public async Task DeleteObjectAsync_ReadsHeaders()
    {
        var handler = FakeS3Handler.Sequence(() => {
            var response = FakeS3.Status(HttpStatusCode.NoContent);

            response.Headers.TryAddWithoutValidation("x-amz-delete-marker", "true");
            response.Headers.TryAddWithoutValidation("x-amz-version-id", "v1");
            response.Headers.TryAddWithoutValidation("x-amz-request-charged", "requester");

            return response;
        });

        var result = await FakeS3.CreateClient(handler).DeleteObjectAsync(new DeleteObjectRequest("s3.test", "bucket", "key"));

        Assert.True(result.IsDeleteMarker);
        Assert.Equal("v1", result.VersionId);
        Assert.Equal("requester", result.RequestCharged);
    }

    [Fact]
    public async Task DeleteObjectAsync_ThrowsWhenNot204()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.OK));

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateClient(handler).DeleteObjectAsync(new DeleteObjectRequest("s3.test", "bucket", "key")));

        Assert.Equal("Expected 204", ex.Message);
        Assert.Equal(HttpStatusCode.OK, ex.HttpStatusCode);
    }

    [Fact]
    public async Task DeleteObjectsAsync_ReportsErrors()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(
            $"""
            <DeleteResult xmlns="{Ns}">
              <Deleted><Key>a</Key></Deleted>
              <Error><Key>b</Key><Code>AccessDenied</Code><Message>Access Denied</Message></Error>
            </DeleteResult>
            """));

        var result = await FakeS3.CreateClient(handler).DeleteObjectsAsync(new DeleteObjectsRequest("s3.test", "bucket", new DeleteBatch(["a", "b"])));

        Assert.True(result.HasErrors);
        Assert.Equal("a", Assert.Single(result.Deleted).Key);
        Assert.Equal("AccessDenied", Assert.Single(result.Errors).Code);
    }

    [Fact]
    public async Task RestoreObjectAsync_ReturnsStatus()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.Accepted));

        var result = await FakeS3.CreateClient(handler).RestoreObjectAsync(new RestoreObjectRequest("s3.test", "bucket", "key"));

        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        Assert.Equal("https://s3.test/bucket/key?restore", handler.Requests[0].Uri);
    }

    [Fact]
    public async Task GetObjectAsync_ReturnsContent()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Blob([1, 2, 3]));

        using var blob = await FakeS3.CreateClient(handler).GetObjectAsync(new GetObjectRequest("s3.test", "bucket", "key"));

        Assert.Equal("key", blob.Key);
        Assert.Equal(HttpStatusCode.OK, blob.StatusCode);
        Assert.Equal([1, 2, 3], await blob.ReadAsByteArrayAsync());
    }

    [Fact]
    public async Task GetObjectAsync_DoesNotThrowWhenNotModified()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.NotModified));

        using var blob = await FakeS3.CreateClient(handler).GetObjectAsync(new GetObjectRequest("s3.test", "bucket", "key"));

        Assert.Equal(HttpStatusCode.NotModified, blob.StatusCode);
    }

    [Fact]
    public async Task GetObjectHeadAsync_ReadsMetadata()
    {
        var handler = FakeS3Handler.Sequence(() => {
            var response = FakeS3.Head(42);

            response.Headers.TryAddWithoutValidation("ETag", "\"5d41402abc4b2a76b9719d911017c592\"");
            response.Headers.TryAddWithoutValidation("x-amz-version-id", "v1");
            response.Headers.TryAddWithoutValidation("x-amz-storage-class", "GLACIER");

            return response;
        });

        var info = await FakeS3.CreateClient(handler).GetObjectHeadAsync(new ObjectHeadRequest("s3.test", "bucket", "key"));

        Assert.Same(HttpMethod.Head, handler.Requests[0].Method);

        Assert.Equal("bucket", info.BucketName);
        Assert.Equal("key", info.Key);
        Assert.Equal(42, info.ContentLength);
        Assert.Equal(new DateTime(2020, 01, 01, 01, 01, 01, DateTimeKind.Utc), info.Modified);
        Assert.Equal("application/octet-stream", info.ContentType);
        Assert.Equal("\"5d41402abc4b2a76b9719d911017c592\"", (string)info.ETag);
        Assert.Equal("v1", info.VersionId);
        Assert.Equal("GLACIER", info.StorageClass);
    }

    [Fact]
    public async Task GetObjectHeadAsync_VersionIdIsOptional()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Head(42));

        var info = await FakeS3.CreateClient(handler).GetObjectHeadAsync(new ObjectHeadRequest("s3.test", "bucket", "key"));

        Assert.Null(info.VersionId);
    }

    [Fact]
    public async Task GetObjectHeadAsync_StorageClassIsOptional()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Head(42));

        var info = await FakeS3.CreateClient(handler).GetObjectHeadAsync(new ObjectHeadRequest("s3.test", "bucket", "key"));

        Assert.Null(info.StorageClass);
    }

    [Fact]
    public async Task NotFound_ThrowsStorageException()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Status(HttpStatusCode.NotFound));

        var ex = await Assert.ThrowsAsync<StorageException>(() => FakeS3.CreateClient(handler).GetObjectAsync(new GetObjectRequest("s3.test", "bucket", "a/b.txt")));

        Assert.Equal(StorageErrorCode.NotFound, ex.ErrorCode);
        Assert.Contains("bucket/a/b.txt", ex.Message);
    }

    [Fact]
    public async Task XmlError_ThrowsS3ExceptionWithDetails()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Error("AccessDenied", HttpStatusCode.Forbidden));

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateClient(handler).GetObjectAsync(new GetObjectRequest("s3.test", "bucket", "key")));

        Assert.Equal("AccessDenied", ex.Error!.Code);
        Assert.Equal("AccessDenied message", ex.Message);
        Assert.Equal(HttpStatusCode.Forbidden, ex.HttpStatusCode);
        Assert.False(ex.IsTransient);
    }

    [Fact]
    public async Task WasabiError_ThrowsS3ExceptionWithDetails()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(
            """
            <ErrorResponse xmlns="https://iam.amazonaws.com/doc/2010-05-08/">
              <Error><Type>Sender</Type><Code>TemporarilyUnavailable</Code><Message>Busy</Message></Error>
            </ErrorResponse>
            """, HttpStatusCode.ServiceUnavailable));

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateClient(handler).GetObjectAsync(new GetObjectRequest("s3.test", "bucket", "key")));

        Assert.Equal("TemporarilyUnavailable", ex.Error!.Code);
        Assert.True(ex.IsTransient);
    }

    [Theory]
    [InlineData("Bad Gateway")]
    [InlineData("<Error>not closed")]
    [InlineData("<ErrorResponse>not closed")]
    public async Task UnreadableError_IncludesStatusAndBody(string body)
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml(body, HttpStatusCode.BadGateway));

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateClient(handler).GetObjectAsync(new GetObjectRequest("s3.test", "bucket", "key")));

        Assert.Equal($"Unexpected S3 error. status = BadGateway | {body}", ex.Message);
        Assert.Null(ex.Error);
    }

    [Fact]
    public async Task GetObjectHeadAsync_ContentTypeIsOptional()
    {
        var handler = FakeS3Handler.Sequence(() => {
            var response = FakeS3.Head(42);

            response.Content.Headers.ContentType = null;

            return response;
        });

        var info = await FakeS3.CreateClient(handler).GetObjectHeadAsync(new ObjectHeadRequest("s3.test", "bucket", "key"));

        Assert.Null(info.ContentType);
    }

    [Fact]
    public async Task ListObjectVersionsAsync_EmptyResultHasNoVersions()
    {
        var handler = FakeS3Handler.Sequence(() => FakeS3.Xml($"""<ListVersionsResult xmlns="{Ns}"></ListVersionsResult>"""));

        var result = await FakeS3.CreateClient(handler).ListObjectVersionsAsync("bucket", new ListVersionsOptions());

        Assert.Empty(result.Versions);
    }
}
