namespace Amazon.S3.Actions.Tests;

public class RestoreObjectRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new RestoreObjectRequest("s3.amazon.com", "bucket-name", "key", "version");

        Assert.Equal("/bucket-name/key?restore&versionId=version", request.RequestUri.PathAndQuery);
    }

    [Fact]
    public void CanSerialize()
    {
        var request = new RestoreObjectRequest("s3.amazonaws.com", "a", "b") { Days = 30 };

        Assert.Equal("/a/b?restore", request.RequestUri.PathAndQuery);

        Assert.Equal(
            """
            <RestoreRequest>
              <Days>30</Days>
              <GlacierJobParameters>
                <Tier>Standard</Tier>
              </GlacierJobParameters>
            </RestoreRequest>
            """, request.GetXmlString());
    }

    [Fact]
    public async Task ContentUsesInitializedValues()
    {
        var request = new RestoreObjectRequest("s3.amazonaws.com", "a", "b") { Days = 30, Tier = GlacierJobTier.Expedited };

        string content = await request.Content.ReadAsStringAsync();

        Assert.Equal(request.GetXmlString(), content);
        Assert.Contains("<Days>30</Days>", content);
        Assert.Contains("<Tier>Expedited</Tier>", content);
        Assert.Equal(System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(content)), request.Content.Headers.ContentMD5);
    }

    [Fact]
    public async Task ContentDefaultsToSevenDays()
    {
        var request = new RestoreObjectRequest("s3.amazonaws.com", "a", "b");

        Assert.Contains("<Days>7</Days>", await request.Content.ReadAsStringAsync());
    }
}