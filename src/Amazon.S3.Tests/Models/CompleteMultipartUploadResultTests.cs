namespace Amazon.S3.Models.Tests;

public class CompleteMultipartUploadResultTests
{
    [Fact]
    public void CompleteXmlTests()
    {
        var result = CompleteMultipartUploadResult.Deserialize(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <CompleteMultipartUploadResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
              <Location>http://Example-Bucket.s3.amazonaws.com/Example-Object</Location>
              <Bucket>Example-Bucket</Bucket>
              <Key>Example-Object</Key>
              <ETag>"3858f62230ac3c915f300c664312c11f-9"</ETag>
            </CompleteMultipartUploadResult>
            """);

        Assert.Equal("http://Example-Bucket.s3.amazonaws.com/Example-Object", result.Location);
        Assert.Equal("Example-Bucket", result.Bucket);
        Assert.Equal("Example-Object", result.Key);
        Assert.Equal("\"3858f62230ac3c915f300c664312c11f-9\"", result.ETag);
    }
}