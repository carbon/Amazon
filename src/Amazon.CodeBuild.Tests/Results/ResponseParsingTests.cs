using System.Text.Json;

using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild.Tests;

public class ResponseParsingTests
{
    [Fact]
    public void Build_ParsesLogsCacheArtifactsAndPhaseContexts()
    {
        var build = JsonSerializer.Deserialize(
            """
            {
              "id": "p:1",
              "buildComplete": true,
              "buildStatus": "FAILED",
              "startTime": 1.5E9,
              "endTime": 1.500000060E9,
              "sourceVersion": "main",
              "artifacts": { "artifactIdentifier": "main", "location": "arn:s3", "md5sum": "md5", "sha256sum": "sha" },
              "cache": { "type": "S3", "location": "bucket/prefix", "modes": [ "LOCAL_SOURCE_CACHE" ] },
              "logs": { "deepLink": "https://logs", "groupName": "group", "streamName": "stream", "cloudWatchLogs": { "status": "ENABLED" } },
              "phases": [
                {
                  "phaseType": "BUILD",
                  "phaseStatus": "FAILED",
                  "durationInSeconds": 60,
                  "startTime": 1.5E9,
                  "endTime": 1.500000060E9,
                  "contexts": [ { "statusCode": "COMMAND_EXECUTION_ERROR", "message": "exit status 1" } ]
                },
                { "phaseType": "COMPLETED", "startTime": 1.500000060E9 }
              ]
            }
            """u8, CodeBuildSerializerContext.Default.Build)!;

        Assert.True(build.BuildComplete);
        Assert.Equal("FAILED", build.BuildStatus);
        Assert.Equal("main", build.SourceVersion);
        Assert.Equal(new DateTime(2017, 07, 14, 02, 41, 00, DateTimeKind.Utc), build.EndTime!.Value);

        Assert.Equal("main", build.Artifacts.ArtifactIdentifier);
        Assert.Equal("md5", build.Artifacts.Md5Sum);
        Assert.Equal("sha", build.Artifacts.Sha256Sum);

        Assert.Equal(ProjectCacheType.S3, build.Cache.Type);
        Assert.Equal("bucket/prefix", build.Cache.Location);
        Assert.Equal(["LOCAL_SOURCE_CACHE"], build.Cache.Modes!);

        Assert.Equal("https://logs", build.Logs.DeepLink);
        Assert.Equal("group", build.Logs.GroupName);
        Assert.Equal("stream", build.Logs.StreamName);

        var failed = build.Phases[0];

        Assert.Equal(PhaseType.BUILD, failed.PhaseType);
        Assert.Equal(PhaseStatus.FAILED, failed.PhaseStatus);
        Assert.Equal(60, failed.DurationInSeconds);

        var context = Assert.Single(failed.Contexts!);

        Assert.Equal("COMMAND_EXECUTION_ERROR", context.StatusCode);
        Assert.Equal("exit status 1", context.Message);

        // The final phase has no status, end time or contexts
        var completed = build.Phases[1];

        Assert.Equal(PhaseType.COMPLETED, completed.PhaseType);
        Assert.Equal(default, completed.PhaseStatus);
        Assert.Null(completed.EndTime);
        Assert.Null(completed.Contexts);
    }

    [Theory]
    [InlineData("NO_CACHE", ProjectCacheType.NO_CACHE)]
    [InlineData("LOCAL", ProjectCacheType.LOCAL)]
    public void ProjectCache_ParsesType(string json, ProjectCacheType expected)
    {
        var cache = JsonSerializer.Deserialize($$"""{"type":"{{json}}"}""", CodeBuildSerializerContext.Default.ProjectCache)!;

        Assert.Equal(expected, cache.Type);
        Assert.Null(cache.Location);
        Assert.Null(cache.Modes);
    }

    [Fact]
    public void Project_ParsesTheModelledMembers()
    {
        var result = JsonSerializer.Deserialize(
            """
            {
              "projects": [
                {
                  "arn": "arn",
                  "name": "p",
                  "description": "description",
                  "encryptionKey": "alias/key",
                  "serviceRole": "role",
                  "timeoutInMinutes": 2160,
                  "created": 1.5E9,
                  "lastModified": 1.5000000015E9,
                  "artifacts": { "type": "S3", "location": "bucket", "namespaceType": "BUILD_ID", "packaging": "ZIP", "encryptionDisabled": true },
                  "environment": {
                    "computeType": "BUILD_GENERAL1_SMALL",
                    "image": "image",
                    "type": "WINDOWS_SERVER_2019_CONTAINER",
                    "environmentVariables": [ { "name": "A", "value": "1", "type": "SECRETS_MANAGER" } ]
                  },
                  "tags": [ { "key": "k", "value": "v" } ],
                  "webhook": { "url": "https://hook", "payloadUrl": "https://payload", "branchFilter": "main", "buildType": "BUILD", "lastModifiedSecret": 1.5E9 }
                }
              ],
              "projectsNotFound": [ "missing" ]
            }
            """u8, CodeBuildSerializerContext.Default.BatchGetProjectsResult)!;

        var project = Assert.Single(result.Projects);

        Assert.Equal("description", project.Description);
        Assert.Equal("alias/key", project.EncryptionKey);
        Assert.Equal("role", project.ServiceRole);
        Assert.Equal(2160, project.TimeoutInMinutes);
        Assert.Equal(new DateTime(2017, 07, 14, 02, 40, 00, DateTimeKind.Utc), project.Created);
        Assert.Equal(new DateTime(2017, 07, 14, 02, 40, 01, 500, DateTimeKind.Utc), project.LastModified);

        Assert.Equal(ProjectArtifactsType.S3, project.Artifacts.Type);
        Assert.Equal(ProjectArtifactsNamespaceType.BUILD_ID, project.Artifacts.NamespaceType);
        Assert.Equal(ProjectArtifactsPackagingType.ZIP, project.Artifacts.Packaging);
        Assert.True(project.Artifacts.EncryptionDisabled);

        Assert.Equal(ProjectEnvironmentType.WINDOWS_SERVER_2019_CONTAINER, project.Environment!.Type);

        var variable = Assert.Single(project.Environment.EnvironmentVariables!);

        Assert.Equal("A", variable.Name);
        Assert.Equal("1", variable.Value);
        Assert.Equal("SECRETS_MANAGER", variable.Type);

        var tag = Assert.Single(project.Tags!);

        Assert.Equal("k", tag.Key);
        Assert.Equal("v", tag.Value);

        Assert.Equal("https://hook", project.Webhook!.Url);
        Assert.Equal("https://payload", project.Webhook.PayloadUrl);
        Assert.Equal("main", project.Webhook.BranchFilter);
        Assert.Equal("BUILD", project.Webhook.BuildType);
        Assert.Equal(1.5E9, project.Webhook.LastModifiedSecret.Value);
    }

    // https://docs.aws.amazon.com/codebuild/latest/APIReference/API_ProjectEnvironment.html
    [Theory]
    [InlineData("WINDOWS_SERVER_2022_CONTAINER")]
    [InlineData("LINUX_LAMBDA_CONTAINER")]
    [InlineData("ARM_LAMBDA_CONTAINER")]
    [InlineData("LINUX_EC2")]
    [InlineData("ARM_EC2")]
    [InlineData("WINDOWS_EC2")]
    [InlineData("MAC_ARM")]
    public void ProjectEnvironment_ParsesEveryDocumentedType(string type)
    {
        var environment = JsonSerializer.Deserialize(
            $$"""{"computeType":"BUILD_LAMBDA_1GB","image":"image","type":"{{type}}"}""",
            CodeBuildSerializerContext.Default.ProjectEnvironment)!;

        Assert.Equal(type, environment.Type.ToString());
    }

    [Fact]
    public void EnvironmentPlatform_ParsesLanguagesAndImages()
    {
        var platform = JsonSerializer.Deserialize(
            """
            {
              "platform": "UBUNTU",
              "languages": [
                { "language": "DOTNET", "images": [ { "name": "aws/codebuild/dot-net:core-2", "description": "Core 2", "versions": [ "x" ] } ] }
              ]
            }
            """u8, CodeBuildSerializerContext.Default.EnvironmentPlatform)!;

        Assert.Equal("UBUNTU", platform.Platform);

        var language = Assert.Single(platform.Languages!);

        Assert.Equal(Language.DOTNET, language.Language);

        var image = Assert.Single(language.Images!);

        Assert.Equal("aws/codebuild/dot-net:core-2", image.Name);
        Assert.Equal("Core 2", image.Description);
    }

    [Fact]
    public void Timestamp_ConvertsFractionalSecondsToUtc()
    {
        var timestamp = new Timestamp(1.5);

        Assert.Equal(new DateTime(1970, 01, 01, 00, 00, 01, 500, DateTimeKind.Utc), timestamp);
        Assert.Equal(new DateTimeOffset(1970, 01, 01, 00, 00, 01, 500, TimeSpan.Zero), timestamp);
    }

    [Fact]
    public void Timestamp_RoundTripsThroughJson()
    {
        string json = JsonSerializer.Serialize(new Timestamp(1.5), CodeBuildSerializerContext.Default.Timestamp);

        Assert.Equal("1.5", json);
        Assert.Equal(1.5, JsonSerializer.Deserialize(json, CodeBuildSerializerContext.Default.Timestamp).Value);
    }

    [Fact]
    public void NullEndTime_IsParsedAsNull()
    {
        var phase = JsonSerializer.Deserialize("""{"startTime":1,"endTime":null}""", CodeBuildSerializerContext.Default.BuildPhase)!;

        Assert.Null(phase.EndTime);
    }
}
