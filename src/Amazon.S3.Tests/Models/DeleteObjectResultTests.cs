namespace Amazon.S3.Models.Tests;

public class DeleteObjectResultTests
{
    [Fact]
    public void CanConstruct()
    {
        var result = new DeleteObjectResult(
            "true",
            "false",
            "11"
        );

        Assert.True(result.IsDeleteMarker);
        Assert.Equal("11", result.VersionId);
    }
}