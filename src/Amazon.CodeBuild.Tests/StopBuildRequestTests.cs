namespace Amazon.CodeBuild.Tests
{
    public class StopBuildRequestTests
    {
        [Fact]
        public async Task Construct()
        {
            var request = new StopBuildRequest("buildid");

            var result = CodeBuildClient.GetRequestMessage("https://google.com/", request);

            Assert.Equal(@"{""id"":""buildid""}", await result.Content.ReadAsStringAsync());

            Assert.Equal("application/x-amz-json-1.1; charset=utf-8", result.Content.Headers.NonValidated["Content-Type"].ToString());
        }
    }
}