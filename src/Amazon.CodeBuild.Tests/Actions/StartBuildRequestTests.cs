using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild.Tests;

public class StartBuildRequestTests
{
    [Fact]
    public async Task CanConstruct()
    {
        var request = new StartBuildRequest("carbon");

        var result = CodeBuildClient.GetRequestMessage("https://test/", request, CodeBuildSerializerContext.Default.StartBuildRequest);

        Assert.NotNull(result.Content);
        Assert.Equal("""{"projectName":"carbon"}"""u8.ToArray(), await result.Content.ReadAsByteArrayAsync());
    }
}