using System.Text.Json;
using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild.Results.Tests;

public class StartBuildResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        StartBuildResult? result = JsonSerializer.Deserialize(
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
                  "computeType": "BUILD_GENERAL1_LARGE",
                  "environmentVariables": [],
                  "image": "mcr.microsoft.com/dotnet/sdk:8.0",
                  "type":" LINUX_CONTAINER"
                },
                "id": "id",
                "initiator": "initiator",
                "projectName": "projectName",
                "source":{
                  "auth": {
                    "type":"OAUTH"
                  },
                  "location": "location",
                  "type": "GITHUB"
                },
                "startTime": 1.494895355089E9,
                "timeoutInMinutes":60
              }
            }
            """u8, CodeBuildSerializerContext.Default.StartBuildResult);

        Assert.NotNull(result);
        var build = result.Build;

        Assert.Equal("arn", build.Arn);
        Assert.Equal("id", build.Id);
        Assert.Equal("initiator", build.Initiator);
        Assert.Equal(1494895355089, ((DateTimeOffset)build.StartTime).ToUnixTimeMilliseconds());
        Assert.Equal(new DateTime(2017, 05, 16, 00, 42, 35, 89, DateTimeKind.Utc), build.StartTime);

        // Artifacts
        Assert.Equal("omitted", build.Artifacts.Location);

        Assert.False(build.BuildComplete);
        Assert.Equal("IN_PROGRESS", build.BuildStatus);
        Assert.Equal("SUBMITTED", build.CurrentPhase);

        Assert.Null(build.EndTime);

        // Environment
        Assert.Equal("BUILD_GENERAL1_LARGE", build.Environment.ComputeType);
        Assert.Equal(ProjectEnvironmentType.LINUX_CONTAINER, build.Environment.Type);
        Assert.Equal("mcr.microsoft.com/dotnet/sdk:8.0", build.Environment.Image);
        Assert.NotNull(build.Environment.EnvironmentVariables);
        Assert.Empty(build.Environment.EnvironmentVariables);

        Assert.Equal(60, build.TimeoutInMinutes);
    }
}