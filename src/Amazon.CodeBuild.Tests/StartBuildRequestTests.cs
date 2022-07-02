namespace Amazon.CodeBuild.Tests;

public class StartBuildRequestTests
{
    [Fact]
    public async Task Construct()
    {
        var request = new StartBuildRequest("carbon");

        var result = CodeBuildClient.GetRequestMessage("https://google.com/", request);

        Assert.Equal("""{"projectName":"carbon"}""", await result.Content.ReadAsStringAsync());
    }
}