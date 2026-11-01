using Carbon.Storage;

namespace Amazon.S3.Models.Tests;

public class S3ObjectInfoTests
{
    [Fact]
    public void RequiresKey()
    {
        Assert.Throws<ArgumentNullException>(() => new S3ObjectInfo(null, 1));
        Assert.Throws<ArgumentNullException>(() => new S3ObjectInfo(null, 1, DateTime.UtcNow));
    }

    [Fact]
    public void CanConstructWithoutResponse()
    {
        var modified = new DateTime(2020, 01, 01, 01, 01, 01, DateTimeKind.Utc);

        using IBlob blob = new S3ObjectInfo("key", 5, modified);

        var info = (S3ObjectInfo)blob;

        Assert.Equal("key", blob.Key);
        Assert.Equal(5, blob.Size);
        Assert.Equal(modified, blob.Modified);
        Assert.Null(info.ContentType);
        Assert.Null(info.VersionId);
        Assert.Null(info.StorageClass);
    }

    [Fact]
    public async Task CannotBeOpened()
    {
        using IBlob info = new S3ObjectInfo("key", 5);

        await Assert.ThrowsAsync<NotImplementedException>(async () => await info.OpenAsync());
    }
}
