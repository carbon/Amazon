using System.Net;
using System.Net.Http;
using System.Text.Json;

using Amazon.Exceptions;

namespace Amazon.CodeBuild.Tests;

public class CodeBuildClientTests
{
    private const string BuildJson = """{"id":"p:1","projectName":"p","startTime":1.5E9}""";
    private const string ProjectJson = """{"name":"p","artifacts":{"type":"NO_ARTIFACTS"}}""";

    public static TheoryData<string, Func<CodeBuildClient, Task>, string, string> Operations => new()
    {
        { "BatchGetBuilds", static c => c.BatchGetBuildsAsync(new BatchGetBuildsRequest("a", "b")), """{"ids":["a","b"]}""", $$"""{"builds":[{{BuildJson}}]}""" },
        { "ListBuilds", static c => c.ListBuildsAsync(new ListBuildsRequest()), "{}", """{"ids":["a"]}""" },
        { "ListBuildsForProject", static c => c.ListBuildsForProjectAsync(new ListBuildsForProjectRequest("p")), """{"projectName":"p"}""", """{"ids":["a"]}""" },
        { "StartBuild", static c => c.StartBuildAsync(new StartBuildRequest("p")), """{"projectName":"p"}""", $$"""{"build":{{BuildJson}}}""" },
        { "StopBuild", static c => c.StopBuildAsync(new StopBuildRequest("p:1")), """{"id":"p:1"}""", $$"""{"build":{{BuildJson}}}""" },
        { "BatchGetProjects", static c => c.BatchGetProjectsAsync(new BatchGetProjectsRequest("p")), """{"names":["p"]}""", $$"""{"projects":[{{ProjectJson}}]}""" },
        { "ListProjects", static c => c.ListProjectsAsync(new ListProjectsRequest()), "{}", """{"projects":["p"]}""" },
        { "DeleteProject", static c => c.DeleteProjectAsync(new DeleteProjectRequest("p")), """{"name":"p"}""", "" },
        { "UpdateProject", static c => c.UpdateProjectAsync(new UpdateProjectRequest { Name = "p" }), """{"name":"p"}""", $$"""{"project":{{ProjectJson}}}""" }
    };

    [Theory]
    [MemberData(nameof(Operations))]
    public async Task SendsTheActionToTheServiceEndpoint(string action, Func<CodeBuildClient, Task> call, string expectedBody, string response)
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Json(response));

        await call(FakeCodeBuild.CreateClient(handler));

        var request = Assert.Single(handler.Requests);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://codebuild.us-east-1.amazonaws.com/", request.Uri.ToString());
        Assert.Equal($"CodeBuild_20161006.{action}", request.Target);
        Assert.Equal("application/x-amz-json-1.1; charset=utf-8", request.ContentType);
        Assert.Equal(expectedBody, request.Body);
    }

    [Fact]
    public async Task SignsRequestsForTheCodeBuildService()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Json("""{"ids":[]}"""));

        await FakeCodeBuild.CreateClient(handler).ListBuildsAsync(new ListBuildsRequest());

        string authorization = Assert.Single(handler.Requests).Authorization;

        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", authorization);
        Assert.Contains("/us-east-1/codebuild/aws4_request", authorization);
    }

    [Fact]
    public async Task CreateProject_SendsBodyAndParsesProject()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Json($$"""{"project":{{ProjectJson}}}"""));

        var result = await FakeCodeBuild.CreateClient(handler).CreateProjectAsync(new CreateProjectRequest {
            Name = "p",
            ServiceRole = "role",
            Artifacts = new ProjectArtifacts { Type = ProjectArtifactsType.NO_ARTIFACTS },
            Environment = new ProjectEnvironment { ComputeType = "BUILD_GENERAL1_SMALL", Image = "image", Type = ProjectEnvironmentType.LINUX_CONTAINER },
            Source = new ProjectSource { Type = ProjectSourceType.NO_SOURCE }
        });

        var request = Assert.Single(handler.Requests);

        Assert.Equal("CodeBuild_20161006.CreateProject", request.Target);

        using var body = JsonDocument.Parse(request.Body);

        Assert.Equal("p", body.RootElement.GetProperty("name").GetString());
        Assert.Equal("p", result.Project.Name);
        Assert.Equal(ProjectArtifactsType.NO_ARTIFACTS, result.Project.Artifacts.Type);
    }

    [Fact]
    public async Task ResultOperation_ThrowsAwsExceptionWithStatusAndServiceMessage()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Error("ResourceNotFoundException", "Project not found"));

        var ex = await Assert.ThrowsAsync<AwsException>(() => FakeCodeBuild.CreateClient(handler).StartBuildAsync(new StartBuildRequest("p")));

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.StartsWith("BadRequest -> ", ex.Message);
        Assert.Contains("ResourceNotFoundException", ex.Message);
        Assert.Contains("Project not found", ex.Message);
    }

    [Fact]
    public async Task DeleteProject_ThrowsAwsExceptionWithStatusAndServiceMessage()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Error("InvalidInputException", "Bad name"));

        var ex = await Assert.ThrowsAsync<AwsException>(() => FakeCodeBuild.CreateClient(handler).DeleteProjectAsync(new DeleteProjectRequest("p")));

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.Contains("InvalidInputException", ex.Message);
        Assert.Contains("Bad name", ex.Message);
    }

    [Fact]
    public async Task ServerError_WithNonJsonBody_StillSurfacesTheBody()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Json("<html>unavailable</html>", HttpStatusCode.ServiceUnavailable));

        var ex = await Assert.ThrowsAsync<AwsException>(() => FakeCodeBuild.CreateClient(handler).ListProjectsAsync(new ListProjectsRequest()));

        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.HttpStatusCode);
        Assert.Contains("<html>unavailable</html>", ex.Message);
    }

    [Fact]
    public async Task NullRequest_ThrowsArgumentNullException()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Json("{}"));
        var client = FakeCodeBuild.CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.StartBuildAsync(null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.DeleteProjectAsync(null!));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task ListProjects_FollowsNextTokenUntilExhausted()
    {
        var handler = new FakeCodeBuildHandler(
            () => FakeCodeBuild.Json("""{"projects":["a","b"],"nextToken":"page2"}"""),
            () => FakeCodeBuild.Json("""{"projects":["c"]}"""));

        var client = FakeCodeBuild.CreateClient(handler);
        var names = new List<string>();
        string? token = null;

        do
        {
            var page = await client.ListProjectsAsync(new ListProjectsRequest { SortBy = "NAME", SortOrder = SortOrder.ASCENDING, NextToken = token });

            names.AddRange(page.Projects);
            token = page.NextToken;
        }
        while (token is not null);

        Assert.Equal(["a", "b", "c"], names);
        Assert.Equal("""{"sortBy":"NAME","sortOrder":"ASCENDING"}""", handler.Requests[0].Body);
        Assert.Equal("""{"sortBy":"NAME","sortOrder":"ASCENDING","nextToken":"page2"}""", handler.Requests[1].Body);
    }

    [Fact]
    public async Task ListBuilds_FollowsNextTokenUntilExhausted()
    {
        var handler = new FakeCodeBuildHandler(
            () => FakeCodeBuild.Json("""{"ids":["a"],"nextToken":"page2"}"""),
            () => FakeCodeBuild.Json("""{"ids":["b"]}"""));

        var client = FakeCodeBuild.CreateClient(handler);

        var first = await client.ListBuildsAsync(new ListBuildsRequest { SortOrder = SortOrder.DESCENDING });
        var second = await client.ListBuildsAsync(new ListBuildsRequest { SortOrder = SortOrder.DESCENDING, NextToken = first.NextToken });

        Assert.Equal(["a"], first.Ids);
        Assert.Equal(["b"], second.Ids);
        Assert.Null(second.NextToken);
        Assert.Equal("""{"nextToken":"page2","sortOrder":"DESCENDING"}""", handler.Requests[1].Body);
    }

    [Fact]
    public async Task ListBuildsForProject_FollowsNextTokenUntilExhausted()
    {
        var handler = new FakeCodeBuildHandler(
            () => FakeCodeBuild.Json("""{"ids":["a"],"nextToken":"page2"}"""),
            () => FakeCodeBuild.Json("""{"ids":["b"]}"""));

        var client = FakeCodeBuild.CreateClient(handler);

        var first = await client.ListBuildsForProjectAsync(new ListBuildsForProjectRequest("p", SortOrder.ASCENDING));
        var second = await client.ListBuildsForProjectAsync(new ListBuildsForProjectRequest("p", SortOrder.ASCENDING, first.NextToken));

        Assert.Equal(["b"], second.Ids);
        Assert.Null(second.NextToken);
        Assert.Equal("""{"projectName":"p","sortOrder":"ASCENDING"}""", handler.Requests[0].Body);
        Assert.Equal("""{"projectName":"p","sortOrder":"ASCENDING","nextToken":"page2"}""", handler.Requests[1].Body);
    }

    [Fact]
    public async Task ListCuratedEnvironmentImages_SendsAnEmptyObject()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Error("InvalidInputException", "No"));

        await Assert.ThrowsAsync<AwsException>(() => FakeCodeBuild.CreateClient(handler).ListCuratedEnvironmentImagesAsync(new ListCuratedEnvironmentImagesRequest()));

        var request = Assert.Single(handler.Requests);

        Assert.Equal("CodeBuild_20161006.ListCuratedEnvironmentImages", request.Target);
        Assert.Equal("{}", request.Body);
    }

    // https://docs.aws.amazon.com/codebuild/latest/APIReference/API_ListCuratedEnvironmentImages.html
    [Fact]
    public async Task ListCuratedEnvironmentImages_ParsesThePlatformsArray()
    {
        var handler = new FakeCodeBuildHandler(() => FakeCodeBuild.Json(
            """{"platforms":[{"platform":"UBUNTU","languages":[{"language":"DOTNET","images":[{"name":"aws/codebuild/standard:7.0","description":"Standard"}]}]}]}"""));

        var result = await FakeCodeBuild.CreateClient(handler).ListCuratedEnvironmentImagesAsync(new ListCuratedEnvironmentImagesRequest());

        var platform = Assert.Single(result.Platforms);

        Assert.Equal("UBUNTU", platform.Platform);

        var language = Assert.Single(platform.Languages!);

        Assert.Equal(Language.DOTNET, language.Language);
        Assert.Equal("aws/codebuild/standard:7.0", Assert.Single(language.Images!).Name);
    }
}
