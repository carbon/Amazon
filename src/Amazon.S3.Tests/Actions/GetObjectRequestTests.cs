namespace Amazon.S3.Actions.Tests;

public class GetObjectRequestTests
{
    [Fact]
    public void SetRange()
    {
        var request = new GetObjectRequest("s3.amazon.com", "bucket", "objectKey");

        request.SetRange(int.MaxValue, 9147484647);
        
        Assert.Equal("bytes=2147483647-9147484647", request.Headers.NonValidated["Range"].ToString());
    }

    [Fact]
    public void SetRange2()
    {
        var request = new GetObjectRequest("s3.amazon.com", "bucket", "objectKey");

        request.SetRange(1325400064, 1342177279);
        Assert.Equal("bytes=1325400064-1342177279", request.Headers.NonValidated["Range"].ToString());
    }
}
