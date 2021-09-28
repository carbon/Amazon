namespace Amazon.S3.Actions.Tests;

public class RestoreObjectRequestTests
{
    [Fact]
    public void Construct()
    {
        var request = new RestoreObjectRequest("s3.amazon.com", "bucket-name", "key", "version");

        Assert.Equal("/bucket-name/key?restore&versionId=version", request.RequestUri.PathAndQuery);
    }

    [Fact]
    public void Serialize()
    {
        var request = new RestoreObjectRequest("s3.amazonaws.com", "a", "b") { Days = 30 };

        Assert.Equal("/a/b?restore", request.RequestUri.PathAndQuery);

        Assert.Equal(@"<RestoreRequest>
  <Days>30</Days>
  <GlacierJobParameters>
    <Tier>Standard</Tier>
  </GlacierJobParameters>
</RestoreRequest>", request.GetXmlString());
    }
}