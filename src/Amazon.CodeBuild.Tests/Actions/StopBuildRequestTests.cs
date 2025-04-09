using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild.Tests;

public class StopBuildRequestTests
{
    [Fact]
    public async Task CanConstruct()
    {
        var request = new StopBuildRequest("build-id");

        var result = CodeBuildClient.GetRequestMessage("https://test/", request, CodeBuildSerializerContext.Default.StopBuildRequest);

        Assert.NotNull(result.Content);
        Assert.Equal("""{"id":"build-id"}"""u8.ToArray(), await result.Content.ReadAsByteArrayAsync());

        Assert.Equal("application/x-amz-json-1.1; charset=utf-8", result.Content.Headers.NonValidated["Content-Type"].ToString());
    }
}