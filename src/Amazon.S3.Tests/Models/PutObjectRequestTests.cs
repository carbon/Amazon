namespace Amazon.S3.Tests;

public class PutObjectRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        request.SetStorageClass(StorageClass.ReducedRedundancy);

        var ms = new MemoryStream(1);

        ms.WriteByte(1);

        ms.Position = 0;

        request.SetStream(ms, "image/jpeg");

        var sha256 = request.Headers.GetValues("x-amz-content-sha256").First();

        Assert.Equal("image/jpeg", string.Join(';', request.Content.Headers.GetValues("Content-Type")));
        Assert.Equal(new Uri("https://s3.amazon.com/bucket/key"), request.RequestUri);
        Assert.Equal("REDUCED_REDUNDANCY", request.Headers.GetValues("x-amz-storage-class").First());
        Assert.Equal("4bf5122f344554c53bde2ebb8cd2b7e3d1600ad631c385a5d7cce23c7785459a", sha256);
    }

    [Fact]
    public void CanConstruct_ThenChangeContentEncoding()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        request.SetStorageClass(StorageClass.Standard);

        var ms = new MemoryStream([ 1, 234, 41 ]);

        request.SetStream(ms, "application/blob");

        request.Content.Headers.TryAddWithoutValidation("Content-Encoding", "br");

        string contentType     = request.Content.Headers.NonValidated["Content-Type"].ToString();
        string contentEncoding = request.Content.Headers.NonValidated["Content-Encoding"].ToString();
        string contentLength   = request.Content.Headers.NonValidated["Content-Length"].ToString();

        string storageClass = request.Headers.NonValidated["x-amz-storage-class"].ToString();
        string sha256       = request.Headers.NonValidated["x-amz-content-sha256"].ToString();

        Assert.Equal(new Uri("https://s3.amazon.com/bucket/key"),                        request.RequestUri);
        Assert.Equal("application/blob",                                                 contentType);
        Assert.Equal("STANDARD",                                                         storageClass);
        Assert.Equal("br",                                                               contentEncoding);
        Assert.Equal("3",                                                                contentLength);
        Assert.Equal("444af740f82d68c28b9827b3d68cbab07d8cb958052dbc346536f4021b2fd312", sha256);

        request.Content.Headers.ContentEncoding.Clear();

        request.Content.Headers.ContentEncoding.Add("gzip");

        contentEncoding = request.Content.Headers.NonValidated["Content-Encoding"].ToString();

        Assert.Equal("gzip", contentEncoding);
    }

    [Fact]
    public void SetStream_ValidatesArguments()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        Assert.Throws<ArgumentNullException>(() => request.SetStream(null, sha256Hash: null));
        Assert.Throws<ArgumentNullException>(() => request.SetStream(new MemoryStream([1]), sha256Hash: null, contentType: null));
        Assert.Throws<ArgumentException>(() => request.SetStream(new MemoryStream([1]), sha256Hash: null, contentType: ""));
        Assert.Throws<ArgumentException>(() => request.SetStream(new MemoryStream(), sha256Hash: null));
        Assert.Throws<ArgumentNullException>(() => request.SetStream(null, length: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => request.SetStream(new MemoryStream([1]), length: 0));
    }

    [Fact]
    public void SetStream_WithoutHashIsUnsigned()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        request.SetStream(new MemoryStream([1]), sha256Hash: null);

        Assert.Equal("UNSIGNED-PAYLOAD", request.Headers.NonValidated["x-amz-content-sha256"].ToString());
        Assert.Equal("application/octet-stream", request.Content.Headers.NonValidated["Content-Type"].ToString());
    }

    [Fact]
    public void SetStream_WithLength_HashesSeekableStream()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        var stream = new MemoryStream([1]);

        request.SetStream(stream, length: 1, contentType: "image/png");

        Assert.Equal("4bf5122f344554c53bde2ebb8cd2b7e3d1600ad631c385a5d7cce23c7785459a", request.Headers.NonValidated["x-amz-content-sha256"].ToString());
        Assert.Equal("image/png", request.Content.Headers.NonValidated["Content-Type"].ToString());
        Assert.Equal(1, request.Content.Headers.ContentLength);
        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public void SetStream_WithLength_DoesNotHashForwardOnlyStream()
    {
        var request = new PutObjectRequest("s3.amazon.com", "bucket", "key");

        request.SetStream(new ForwardOnlyStream(), length: 5);

        Assert.Equal("UNSIGNED-PAYLOAD", request.Headers.NonValidated["x-amz-content-sha256"].ToString());
        Assert.Equal(5, request.Content.Headers.ContentLength);
    }

    private sealed class ForwardOnlyStream : MemoryStream
    {
        public override bool CanSeek => false;
    }
}