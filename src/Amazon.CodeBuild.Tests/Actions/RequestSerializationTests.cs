using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild.Tests;

public class RequestSerializationTests
{
    private static ProjectArtifacts S3Artifacts => new() {
        Type = ProjectArtifactsType.S3,
        Location = "bucket",
        Name = "output",
        NamespaceType = ProjectArtifactsNamespaceType.BUILD_ID,
        Packaging = ProjectArtifactsPackagingType.ZIP,
        Path = "path",
        ArtifactIdentifier = "main",
        EncryptionDisabled = true
    };

    private static ProjectEnvironment LinuxEnvironment => new() {
        ComputeType = "BUILD_GENERAL1_SMALL",
        Image = "aws/codebuild/standard:7.0",
        Type = ProjectEnvironmentType.LINUX_CONTAINER,
        Certificate = "cert",
        EnvironmentVariables = [new EnvironmentVariable("A", "1") { Type = "PARAMETER_STORE" }]
    };

    private static ProjectSource GitHubSource => new() {
        Type = ProjectSourceType.GITHUB,
        Location = "https://github.com/carbon/Amazon",
        Buildspec = "buildspec.yml",
        GitCloneDepth = 1,
        Auth = new SourceAuth { Type = "OAUTH", Resource = "resource" }
    };

    [Fact]
    public void CreateProject_WritesTheServiceShape()
    {
        var request = new CreateProjectRequest {
            Name = "p",
            ServiceRole = "role",
            Description = "description",
            EncryptionKey = "alias/key",
            BadgeEnabled = true,
            TimeoutInMinutes = 60,
            Tags = [new Tag("k", "v")],
            Artifacts = S3Artifacts,
            Environment = LinuxEnvironment,
            Source = GitHubSource
        };

        using var doc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(request, CodeBuildSerializerContext.Default.CreateProjectRequest));

        var root = doc.RootElement;

        Assert.Equal("p", root.GetProperty("name").GetString());
        Assert.Equal("role", root.GetProperty("serviceRole").GetString());
        Assert.Equal("description", root.GetProperty("description").GetString());
        Assert.Equal("alias/key", root.GetProperty("encryptionKey").GetString());
        Assert.True(root.GetProperty("badgeEnabled").GetBoolean());
        Assert.Equal(60, root.GetProperty("timeoutInMinutes").GetInt32());
        Assert.Equal("""{"key":"k","value":"v"}""", root.GetProperty("tags")[0].GetRawText());

        AssertArtifacts(root.GetProperty("artifacts"));
        AssertEnvironment(root.GetProperty("environment"));
        AssertSource(root.GetProperty("source"));
    }

    [Fact]
    public void CreateProject_OmitsUnsetOptionalMembers()
    {
        var request = new CreateProjectRequest {
            Name = "p",
            ServiceRole = "role",
            Artifacts = new ProjectArtifacts { Type = ProjectArtifactsType.NO_ARTIFACTS },
            Environment = LinuxEnvironment,
            Source = new ProjectSource { Type = ProjectSourceType.NO_SOURCE }
        };

        using var doc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(request, CodeBuildSerializerContext.Default.CreateProjectRequest));

        var root = doc.RootElement;

        Assert.False(root.TryGetProperty("description", out _));
        Assert.False(root.TryGetProperty("tags", out _));
        Assert.False(root.TryGetProperty("badgeEnabled", out _));
        Assert.False(root.TryGetProperty("timeoutInMinutes", out _));
    }

    [Fact]
    public void UpdateProject_WritesOnlyTheNameWhenNothingElseIsSet()
    {
        var request = new UpdateProjectRequest { Name = "p" };

        Assert.Equal("""{"name":"p"}""", JsonSerializer.Serialize(request, CodeBuildSerializerContext.Default.UpdateProjectRequest));
    }

    [Fact]
    public void UpdateProject_WritesTheServiceShape()
    {
        var request = new UpdateProjectRequest {
            Name = "p",
            ServiceRole = "role",
            Description = "description",
            EncryptionKey = "alias/key",
            TimeoutInMinutes = 30,
            Tags = [new Tag { Key = "k", Value = "v" }],
            Artifacts = S3Artifacts,
            Environment = LinuxEnvironment,
            Source = GitHubSource
        };

        using var doc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(request, CodeBuildSerializerContext.Default.UpdateProjectRequest));

        var root = doc.RootElement;

        Assert.Equal("role", root.GetProperty("serviceRole").GetString());
        Assert.Equal("description", root.GetProperty("description").GetString());
        Assert.Equal("alias/key", root.GetProperty("encryptionKey").GetString());
        Assert.Equal(30, root.GetProperty("timeoutInMinutes").GetInt32());
        Assert.Equal("k", root.GetProperty("tags")[0].GetProperty("key").GetString());

        AssertArtifacts(root.GetProperty("artifacts"));
        AssertEnvironment(root.GetProperty("environment"));
        AssertSource(root.GetProperty("source"));
    }

    [Fact]
    public void StartBuild_WritesOverrides()
    {
        var request = new StartBuildRequest {
            ProjectName = "p",
            ArtifactsOverride = S3Artifacts,
            BuildspecOverride = "buildspec.yml",
            EnvironmentVariablesOverride = [new EnvironmentVariable("A", "1")],
            SourceVersion = "main",
            TimeoutInMinutesOverride = 15,
            IdempotencyToken = "token"
        };

        using var doc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(request, CodeBuildSerializerContext.Default.StartBuildRequest));

        var root = doc.RootElement;

        Assert.Equal("p", root.GetProperty("projectName").GetString());
        Assert.Equal("buildspec.yml", root.GetProperty("buildspecOverride").GetString());
        Assert.Equal("main", root.GetProperty("sourceVersion").GetString());
        Assert.Equal(15, root.GetProperty("timeoutInMinutesOverride").GetInt32());
        Assert.Equal("token", root.GetProperty("idempotencyToken").GetString());

        // type is optional and defaults to PLAINTEXT on the service
        Assert.Equal("""{"name":"A","value":"1"}""", root.GetProperty("environmentVariablesOverride")[0].GetRawText());

        AssertArtifacts(root.GetProperty("artifactsOverride"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void BuildRequests_RejectMissingIdentifiers(string? value)
    {
        Assert.ThrowsAny<ArgumentException>(() => new StartBuildRequest(value!));
        Assert.ThrowsAny<ArgumentException>(() => new StopBuildRequest(value!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ProjectRequests_RejectMissingNames(string? value)
    {
        Assert.ThrowsAny<ArgumentException>(() => new DeleteProjectRequest(value!));
        Assert.ThrowsAny<ArgumentException>(() => new ListBuildsForProjectRequest(value!));
        Assert.ThrowsAny<ArgumentException>(() => new EnvironmentVariable(value!, "value"));
    }

    [Fact]
    public void BatchRequests_RejectNull()
    {
        Assert.Throws<ArgumentNullException>(() => new BatchGetBuildsRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new BatchGetProjectsRequest(null!));
    }

    [Fact]
    public void EnvironmentVariable_RejectsNullValue()
    {
        Assert.Throws<ArgumentNullException>(() => new EnvironmentVariable("A", null!));
    }

    // https://docs.aws.amazon.com/codebuild/latest/APIReference/API_EnvironmentVariable.html
    [Fact]
    public void EnvironmentVariable_AllowsEmptyValue()
    {
        Assert.Equal("", new EnvironmentVariable("A", "").Value);
    }

    // https://docs.aws.amazon.com/codebuild/latest/APIReference/API_CreateProject.html
    [Fact]
    public void CreateProject_TimeoutRangeMatchesTheService()
    {
        var request = new CreateProjectRequest {
            Name = "p",
            ServiceRole = "role",
            TimeoutInMinutes = 2160,
            Artifacts = new ProjectArtifacts { Type = ProjectArtifactsType.NO_ARTIFACTS },
            Environment = LinuxEnvironment,
            Source = new ProjectSource { Type = ProjectSourceType.NO_SOURCE }
        };

        Assert.True(Validator.TryValidateObject(request, new ValidationContext(request), null, validateAllProperties: true));
    }

    private static void AssertArtifacts(JsonElement artifacts)
    {
        Assert.Equal("S3", artifacts.GetProperty("type").GetString());
        Assert.Equal("bucket", artifacts.GetProperty("location").GetString());
        Assert.Equal("output", artifacts.GetProperty("name").GetString());
        Assert.Equal("BUILD_ID", artifacts.GetProperty("namespaceType").GetString());
        Assert.Equal("ZIP", artifacts.GetProperty("packaging").GetString());
        Assert.Equal("path", artifacts.GetProperty("path").GetString());
        Assert.Equal("main", artifacts.GetProperty("artifactIdentifier").GetString());
        Assert.True(artifacts.GetProperty("encryptionDisabled").GetBoolean());
    }

    private static void AssertEnvironment(JsonElement environment)
    {
        Assert.Equal("BUILD_GENERAL1_SMALL", environment.GetProperty("computeType").GetString());
        Assert.Equal("aws/codebuild/standard:7.0", environment.GetProperty("image").GetString());
        Assert.Equal("LINUX_CONTAINER", environment.GetProperty("type").GetString());
        Assert.Equal("cert", environment.GetProperty("certificate").GetString());
        Assert.Equal("""{"name":"A","value":"1","type":"PARAMETER_STORE"}""", environment.GetProperty("environmentVariables")[0].GetRawText());
    }

    private static void AssertSource(JsonElement source)
    {
        Assert.Equal("GITHUB", source.GetProperty("type").GetString());
        Assert.Equal("https://github.com/carbon/Amazon", source.GetProperty("location").GetString());
        Assert.Equal("buildspec.yml", source.GetProperty("buildspec").GetString());
        Assert.Equal(1, source.GetProperty("gitCloneDepth").GetInt32());
        Assert.Equal("""{"type":"OAUTH","resource":"resource"}""", source.GetProperty("auth").GetRawText());
    }
}
