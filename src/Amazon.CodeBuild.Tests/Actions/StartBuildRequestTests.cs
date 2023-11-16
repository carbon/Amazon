namespace Amazon.CodeBuild.Tests;

public class StartBuildRequestTests
{
    [Fact]
    public async Task CanConstruct()
    {
        var request = new StartBuildRequest("carbon");

        var result = CodeBuildClient.GetRequestMessage("https://test/", request);

        Assert.NotNull(result.Content);
        Assert.Equal("""{"projectName":"carbon"}""", await result.Content.ReadAsStringAsync());
    }
}