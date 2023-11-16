using System.Text.Json;

namespace Amazon.CodeBuild.Tests;

public class StartBuildResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = JsonSerializer.Deserialize<StartBuildResponse>(
            """
            {
              "build": {
                "arn": "arn",
                "artifacts": {
                  "location": "omitted"
                },
                "buildComplete" :false,
                "buildStatus" :"IN_PROGRESS",
                "currentPhase" :"SUBMITTED",
                "environment": {
                  "computeType":"BUILD_GENERAL1_LARGE",
                  "environmentVariables":[],
                  "image":"microsoft/dotnet:1.1.1-sdk",
                  "type":"LINUX_CONTAINER"
                },
                "id":"id",
                "initiator":"initiator",
                "projectName":"projectName",
                "source":{
                  "auth":{"type":"OAUTH"},
                  "location":"location",
                  "type":"GITHUB"
                },
                "startTime": 1.494895355089E9,
                "timeoutInMinutes":60
              }
            }
            """u8);

        Assert.NotNull(response);
        var build = response.Build;

        Assert.Equal("arn", build.Arn);
        Assert.Equal(1494895355088, ((DateTimeOffset)build.StartTime).ToUnixTimeMilliseconds());
        Assert.Equal("omitted", build.Artifacts.Location);

        Assert.False(build.BuildComplete);
        Assert.Equal("IN_PROGRESS", build.BuildStatus);
        Assert.Equal("SUBMITTED", build.CurrentPhase);

        // Assert.Equal(new DateTime(2017, 05, 16, 00, 42, 35, 89, DateTimeKind.Utc), build.StartTime);
        Assert.Null(build.EndTime);

        Assert.Equal("BUILD_GENERAL1_LARGE", build.Environment.ComputeType);

        Assert.Equal(60, build.TimeoutInMinutes);
    }
}