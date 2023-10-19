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

        request.SetStream(ms, "image/jpeg");

        var sha256 = request.Headers.GetValues("x-amz-content-sha256").First();

        Assert.Equal("image/jpeg", string.Join(';', request.Content.Headers.GetValues("Content-Type")));
        Assert.Equal(new Uri("https://s3.amazon.com/bucket/key"), request.RequestUri);
        Assert.Equal("REDUCED_REDUNDANCY", request.Headers.GetValues("x-amz-storage-class").First());
        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", sha256);
    }

    [Fact]
    public void CanConstruct_2()
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
}