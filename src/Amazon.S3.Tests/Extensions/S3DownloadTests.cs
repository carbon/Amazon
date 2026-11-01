using System.Net;

using Amazon.S3.Extensions;

namespace Amazon.S3.Tests;

public sealed class S3DownloadTests : IDisposable
{
    private static readonly TimeSpan s_hangTimeout = TimeSpan.FromSeconds(5);

    private readonly string _directory = Directory.CreateTempSubdirectory("s3-download-").FullName;

    private string Destination => Path.Combine(_directory, "file.bin");

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
    }

    private static byte[] CreateData(int length)
    {
        return Enumerable.Range(0, length).Select(static i => (byte)i).ToArray();
    }

    // Serves HEAD and ranged GET requests for an object
    private static FakeS3Handler ObjectServer(byte[] data)
    {
        return new FakeS3Handler((request, _) => {
            if (request.Method == HttpMethod.Head)
            {
                return Task.FromResult(FakeS3.Head(data.Length));
            }

            if (request.Headers.Range is { } range)
            {
                var item = range.Ranges.First();

                return Task.FromResult(FakeS3.Blob(data[(int)item.From!.Value..((int)item.To!.Value + 1)], HttpStatusCode.PartialContent));
            }

            return Task.FromResult(FakeS3.Blob(data));
        });
    }

    [Fact]
    public async Task Client_DownloadsAllChunks()
    {
        var data = CreateData(10);

        var handler = ObjectServer(data);

        var info = await FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination, new S3DownloadOptions {
            ChunkSize      = 4,
            MaxThreadCount = 2
        });

        Assert.Equal(10, info.ContentLength);
        Assert.Equal(data, await File.ReadAllBytesAsync(Destination));

        var ranges = handler.Requests.Where(static request => request.Method == HttpMethod.Get).Select(static request => request.Headers["Range"]).Order();

        Assert.Equal(["bytes=0-3", "bytes=4-7", "bytes=8-9"], ranges);
    }

    [Fact]
    public async Task Client_DefaultOptionsDownloadSequentially()
    {
        var data = CreateData(10);

        await FakeS3.CreateClient(ObjectServer(data)).DownloadAsync("bucket", "key", Destination);

        Assert.Equal(data, await File.ReadAllBytesAsync(Destination));
    }

    [Fact]
    public async Task Client_DoesNotOverwriteExistingFile()
    {
        await File.WriteAllBytesAsync(Destination, [9]);

        await Assert.ThrowsAsync<IOException>(() => FakeS3.CreateClient(ObjectServer(CreateData(10))).DownloadAsync("bucket", "key", Destination));

        Assert.Equal([9], await File.ReadAllBytesAsync(Destination));
    }

    [Fact]
    public async Task Client_ThrowsWhenChunkFails()
    {
        var handler = new FakeS3Handler((request, _) => Task.FromResult(request.Method == HttpMethod.Head
            ? FakeS3.Head(10)
            : FakeS3.Error("AccessDenied", HttpStatusCode.Forbidden)));

        var ex = await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination).WaitAsync(s_hangTimeout));

        Assert.Equal("AccessDenied", ex.Error!.Code);
    }

    [Fact]
    public async Task Client_ThrowsWhenChunkLengthIsWrong()
    {
        var handler = new FakeS3Handler((request, _) => Task.FromResult(request.Method == HttpMethod.Head
            ? FakeS3.Head(10)
            : FakeS3.Blob([1, 2, 3], HttpStatusCode.PartialContent)));

        var ex = await Assert.ThrowsAsync<Exception>(() => FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination).WaitAsync(s_hangTimeout));

        Assert.Contains("Expected 10 bytes", ex.Message);
    }

    [Fact]
    public async Task Client_RejectsTooManyChunks()
    {
        var handler = new FakeS3Handler((_, _) => Task.FromResult(FakeS3.Head(10_001)));

        var ex = await Assert.ThrowsAsync<Exception>(() => FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination, new S3DownloadOptions {
            ChunkSize = 1
        }));

        Assert.Equal("Must be less than 10,000", ex.Message);
    }

    [Fact]
    public async Task Client_ThrowsWhenEarlierChunkFailsAndLaterChunkSucceeds()
    {
        var secondStarted = new TaskCompletionSource();
        var firstFailed   = new TaskCompletionSource();

        var handler = new FakeS3Handler(async (request, _) => {
            if (request.Method == HttpMethod.Head)
            {
                return FakeS3.Head(8);
            }

            if (request.Headers.Range!.Ranges.First().From is 0)
            {
                await secondStarted.Task;

                firstFailed.SetResult();

                return FakeS3.Error("AccessDenied", HttpStatusCode.Forbidden);
            }

            secondStarted.SetResult();

            await firstFailed.Task;

            // Let the first chunk's continuation record its failure
            await Task.Delay(200);

            return FakeS3.Blob(CreateData(4), HttpStatusCode.PartialContent);
        });

        await Assert.ThrowsAsync<S3Exception>(() => FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination, new S3DownloadOptions {
            ChunkSize      = 4,
            MaxThreadCount = 2
        }).WaitAsync(s_hangTimeout));
    }

    [Fact]
    public async Task Client_DownloadsEmptyObject()
    {
        var info = await FakeS3.CreateClient(ObjectServer([])).DownloadAsync("bucket", "key", Destination).WaitAsync(s_hangTimeout);

        Assert.Equal(0, info.ContentLength);
        Assert.Empty(await File.ReadAllBytesAsync(Destination));
    }

    [Fact]
    public async Task Client_StopsWhenCancelled()
    {
        using var cts = new CancellationTokenSource();

        var handler = new FakeS3Handler(async (request, cancellationToken) => {
            if (request.Method == HttpMethod.Head)
            {
                return FakeS3.Head(4);
            }

            cts.CancelAfter(50);

            await Task.Delay(Timeout.Infinite, cancellationToken);

            return FakeS3.Blob([]);
        });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => FakeS3.CreateClient(handler)
            .DownloadAsync("bucket", "key", Destination, cancellationToken: cts.Token)
            .WaitAsync(s_hangTimeout));
    }

    [Fact]
    public async Task Bucket_DownloadsWithSingleRequestByDefault()
    {
        var data = CreateData(10);

        var handler = ObjectServer(data);

        await FakeS3.CreateBucket(handler).DownloadAsync("key", Destination);

        Assert.Equal(data, await File.ReadAllBytesAsync(Destination));

        var request = Assert.Single(handler.Requests);

        Assert.Same(HttpMethod.Get, request.Method);
        Assert.False(request.Headers.ContainsKey("Range"));
    }

    [Fact]
    public async Task Bucket_DownloadsChunksWhenParallel()
    {
        var data = CreateData(10);

        await FakeS3.CreateBucket(ObjectServer(data)).DownloadAsync("key", Destination, new S3DownloadOptions {
            ChunkSize      = 4,
            MaxThreadCount = 2
        });

        Assert.Equal(data, await File.ReadAllBytesAsync(Destination));
    }

    [Fact]
    public async Task Bucket_ValidatesArguments()
    {
        var bucket = FakeS3.CreateBucket(ObjectServer([]));

        await Assert.ThrowsAsync<ArgumentNullException>(() => bucket.DownloadAsync(null, Destination));
        await Assert.ThrowsAsync<ArgumentException>(() => bucket.DownloadAsync("key", ""));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bucket.DownloadAsync("key", Destination, new S3DownloadOptions { MaxThreadCount = 0 }));
    }

    [Fact]
    public async Task Client_RejectsInvalidThreadCount()
    {
        var handler = ObjectServer(CreateData(10));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination, new S3DownloadOptions {
            MaxThreadCount = 0
        }));

        Assert.Empty(handler.Requests);
        Assert.False(File.Exists(Destination));
    }

    [Fact]
    public async Task Client_LimitsConcurrentChunks()
    {
        var data = CreateData(40);

        int current = 0;
        int peak    = 0;

        var handler = new FakeS3Handler(async (request, _) => {
            if (request.Method == HttpMethod.Head)
            {
                return FakeS3.Head(data.Length);
            }

            int active = Interlocked.Increment(ref current);

            InterlockedMax(ref peak, active);

            await Task.Delay(10);

            Interlocked.Decrement(ref current);

            var item = request.Headers.Range!.Ranges.First();

            return FakeS3.Blob(data[(int)item.From!.Value..((int)item.To!.Value + 1)], HttpStatusCode.PartialContent);
        });

        await FakeS3.CreateClient(handler).DownloadAsync("bucket", "key", Destination, new S3DownloadOptions {
            ChunkSize      = 4,
            MaxThreadCount = 3
        });

        Assert.Equal(data, await File.ReadAllBytesAsync(Destination));
        Assert.InRange(peak, 1, 3);
    }

    private static void InterlockedMax(ref int target, int value)
    {
        int seen;

        while (value > (seen = Volatile.Read(ref target)) && Interlocked.CompareExchange(ref target, value, seen) != seen)
        {
        }
    }
}
