using System.Net;
using System.Text;

namespace Amazon.S3.Tests;

public class S3ObjectTests
{
    [Fact]
    public void CanConstructFromHttpResponse()
    {
        var response = GetMockResponse();

        using var blob = new S3Object("key", response);

        Assert.Equal(5, blob.Properties.Count);

        Assert.Equal(7, blob.ContentLength);
        Assert.Null(blob.ContentRange);

        Assert.Equal("Wed, 01 Jan 2020 01:01:01 GMT", blob.Properties["Last-Modified"]);

        Assert.Equal(new DateTimeOffset(2020, 01, 01, 01, 01, 01, TimeSpan.Zero), blob.LastModified);
        Assert.Equal("application/json; charset=utf-8", blob.ContentType);

        Assert.Same(blob.ContentType, blob.Properties["Content-Type"]);
    }

    [Fact]
    public async Task CanDispose()
    {
        var response = GetMockResponse();

        var blob = new S3Object("key", response);

        blob.Dispose();

        await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await blob.OpenAsync(); });
    }

    private static HttpResponseMessage GetMockResponse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        var lastModified = new DateTimeOffset(2020, 01, 01, 01, 01, 01, TimeSpan.Zero);

        response.Headers.TryAddWithoutValidation("x-amz-meta-format", "json");

        response.Content = new StringContent("[1,2,3]", Encoding.UTF8, "application/json") {
            Headers = {
                { "Content-Length", "7" },
                { "Last-Modified", lastModified.ToString("R") },
                { "x-test", "1" }
            }
        };

        return response;
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new S3Object(null, GetMockResponse()));
        Assert.Throws<ArgumentNullException>(() => new S3Object("key", null));
    }

    [Fact]
    public async Task CanReadContent()
    {
        using var blob = new S3Object("key", GetMockResponse());

        Assert.Equal("[1,2,3]", Encoding.UTF8.GetString(await blob.ReadAsByteArrayAsync()));
    }

    [Fact]
    public async Task OpenAsync_ReturnsSameStream()
    {
        using var blob = new S3Object("key", GetMockResponse());

        var stream = await blob.OpenAsync();

        Assert.Same(stream, await blob.OpenAsync());
        Assert.Equal("[1,2,3]", await new StreamReader(stream).ReadToEndAsync());
    }

    [Fact]
    public async Task CanCopyTo()
    {
        using var blob = new S3Object("key", GetMockResponse());

        using var a = new MemoryStream();
        using var b = new MemoryStream();

        await blob.CopyToAsync(a);
        await blob.CopyToAsync(b, CancellationToken.None);

        Assert.Equal("[1,2,3]", Encoding.UTF8.GetString(a.ToArray()));
        Assert.Equal("[1,2,3]", Encoding.UTF8.GetString(b.ToArray()));
    }

    [Fact]
    public async Task ThrowsAfterDispose()
    {
        var blob = new S3Object("key", GetMockResponse());

        blob.Dispose();

        await Assert.ThrowsAsync<ObjectDisposedException>(() => blob.ReadAsByteArrayAsync());
        await Assert.ThrowsAsync<ObjectDisposedException>(() => blob.CopyToAsync(new MemoryStream()));
        await Assert.ThrowsAsync<ObjectDisposedException>(() => blob.CopyToAsync(new MemoryStream(), CancellationToken.None));
    }

    [Fact]
    public void ImplementsBlob()
    {
        using var blob = new S3Object("key", GetMockResponse());

        Carbon.Storage.IBlob item = blob;

        Assert.Equal(7, item.Size);
        Assert.Equal(new DateTime(2020, 01, 01, 01, 01, 01, DateTimeKind.Utc), item.Modified);
    }

    [Fact]
    public void ParsesCacheControlAndContentRange()
    {
        var response = GetMockResponse();

        response.Headers.TryAddWithoutValidation("Cache-Control", "max-age=60");
        response.Content.Headers.TryAddWithoutValidation("Content-Range", "bytes 0-6/7");

        using var blob = new S3Object("key", response);

        Assert.Equal(TimeSpan.FromSeconds(60), blob.CacheControl.MaxAge);
        Assert.Equal(7, blob.ContentRange.Length);
        Assert.Null(new S3Object("key", GetMockResponse()).CacheControl);
    }

    [Fact]
    public async Task NotModifiedHasNoContent()
    {
        using var blob = new S3Object("key", new HttpResponseMessage(HttpStatusCode.NotModified));

        Assert.Equal(HttpStatusCode.NotModified, blob.StatusCode);

        await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await blob.OpenAsync(); });
    }
}