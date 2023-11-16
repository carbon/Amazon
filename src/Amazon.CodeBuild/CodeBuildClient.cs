using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Exceptions;

namespace Amazon.CodeBuild;

public sealed class CodeBuildClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.CodeBuild, region, credential)
{
    public const string Version = "2016-10-06";

    #region Builds

    public Task<BatchGetBuildsResponse> BatchGetBuildsAsync(BatchGetBuildsRequest request)
    {
        return SendAsync<BatchGetBuildsResponse>(request);
    }

    public Task<ListBuildsResponse> ListBuildsAsync(ListBuildsRequest request)
    {
        return SendAsync<ListBuildsResponse>(request);
    }

    public Task<ListBuildsForProjectResponse> ListBuildsForProjectAsync(ListBuildsForProjectRequest request)
    {
        return SendAsync<ListBuildsForProjectResponse>(request);
    }

    public Task<StartBuildResponse> StartBuildAsync(StartBuildRequest request)
    {
        return SendAsync<StartBuildResponse>(request);
    }

    public Task<StopBuildResponse> StopBuildAsync(StopBuildRequest request)
    {
        return SendAsync<StopBuildResponse>(request);
    }

    #endregion

    #region Environments

    public Task<ListCuratedEnvironmentImagesResponse> ListCuratedEnvironmentImagesAsync(ListCuratedEnvironmentImagesRequest request)
    {
        return SendAsync<ListCuratedEnvironmentImagesResponse>(request);
    }

    #endregion

    #region Projects

    public Task<BatchGetProjectsResponse> BatchGetProjectsAsync(BatchGetProjectsRequest request)
    {
        return SendAsync<BatchGetProjectsResponse>(request);
    }

    public Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request)
    {
        return SendAsync<DeleteProjectResponse>(request);
    }

    public Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        return SendAsync<CreateProjectResponse>(request);
    }

    public Task<ListProjectsResponse> ListProjectsAsync(ListProjectsRequest request)
    {
        return SendAsync<ListProjectsResponse>(request);
    }

    public Task<UpdateProjectResponse> UpdateProjectAsync(UpdateProjectRequest request)
    {
        return SendAsync<UpdateProjectResponse>(request);
    }

    #endregion

    #region Helpers

    private static readonly JsonSerializerOptions s_jso = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private async Task<T> SendAsync<T>(ICodeBuildRequest request)
        where T : notnull, new()
    {
        var httpRequest = GetRequestMessage(Endpoint, request);

        await SignAsync(httpRequest).ConfigureAwait(false);

        using var response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            throw new AwsException($"{response.StatusCode} -> {responseText}", response.StatusCode);         
        }

        if (response.StatusCode is HttpStatusCode.NoContent || response.Content.Headers.ContentLength is 0)
        {
            return new T();
        }


        var result = await response.Content.ReadFromJsonAsync<T>(s_jso).ConfigureAwait(false);

        return result!;
    }

    public static HttpRequestMessage GetRequestMessage(string endpoint, object request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var actionName = request.GetType().Name.Replace("Request", string.Empty);

        byte[] json = JsonSerializer.SerializeToUtf8Bytes(request, s_jso);

        // 2016-10-06
        // X-Amz-Target: CodeBuild_20161006.StopBuild

        return new HttpRequestMessage(HttpMethod.Post, endpoint) {
            Headers = {
                { "x-amz-target", $"CodeBuild_20161006.{actionName}" },
            },
            Content = new ByteArrayContent(json) {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.1; charset=utf-8" }
                }
            }
        };
    }

    #endregion
}

// ref: http://docs.aws.amazon.com/codebuild/latest/APIReference/Welcome.html