using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using Amazon.CodeBuild.Serialization;
using Amazon.Exceptions;

namespace Amazon.CodeBuild;

public sealed class CodeBuildClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.CodeBuild, region, credential)
{
    public const string Version = "2016-10-06";

    #region Builds

    public Task<BatchGetBuildsResult> BatchGetBuildsAsync(BatchGetBuildsRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.BatchGetBuildsRequest, CodeBuildSerializerContext.Default.BatchGetBuildsResult);
    }

    public Task<ListBuildsResult> ListBuildsAsync(ListBuildsRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.ListBuildsRequest, CodeBuildSerializerContext.Default.ListBuildsResult);
    }

    public Task<ListBuildsForProjectResult> ListBuildsForProjectAsync(ListBuildsForProjectRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.ListBuildsForProjectRequest, CodeBuildSerializerContext.Default.ListBuildsForProjectResult);
    }

    public Task<StartBuildResult> StartBuildAsync(StartBuildRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.StartBuildRequest, CodeBuildSerializerContext.Default.StartBuildResult);
    }

    public Task<StopBuildResult> StopBuildAsync(StopBuildRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.StopBuildRequest, CodeBuildSerializerContext.Default.StopBuildResult);
    }

    #endregion

    #region Environments

    public Task<ListCuratedEnvironmentImagesResult> ListCuratedEnvironmentImagesAsync(ListCuratedEnvironmentImagesRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.ListCuratedEnvironmentImagesRequest, CodeBuildSerializerContext.Default.ListCuratedEnvironmentImagesResult);
    }

    #endregion

    #region Projects

    public Task<BatchGetProjectsResult> BatchGetProjectsAsync(BatchGetProjectsRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.BatchGetProjectsRequest, CodeBuildSerializerContext.Default.BatchGetProjectsResult);
    }

    public Task DeleteProjectAsync(DeleteProjectRequest request)
    {
        // If the action is successful, the service sends back an HTTP 200 response with an empty HTTP body.

        return SendAsync(request, CodeBuildSerializerContext.Default.DeleteProjectRequest);
    }

    public Task<CreateProjectResult> CreateProjectAsync(CreateProjectRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.CreateProjectRequest, CodeBuildSerializerContext.Default.CreateProjectResult);
    }

    public Task<ListProjectsResult> ListProjectsAsync(ListProjectsRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.ListProjectsRequest, CodeBuildSerializerContext.Default.ListProjectsResult);
    }

    public Task<UpdateProjectResult> UpdateProjectAsync(UpdateProjectRequest request)
    {
        return SendAsync(request, CodeBuildSerializerContext.Default.UpdateProjectRequest, CodeBuildSerializerContext.Default.UpdateProjectResult);
    }

    #endregion

    #region Helpers
    
    private async Task SendAsync<T>(T request, JsonTypeInfo<T> requestType)
    {
        var httpRequest = GetRequestMessage<T>(Endpoint, request, requestType);

        await SignAsync(httpRequest).ConfigureAwait(false);

        using var response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            throw new AwsException($"{response.StatusCode} -> {responseText}", response.StatusCode);
        }
    }

    private async Task<TResult> SendAsync<TRequest, TResult>(TRequest request, JsonTypeInfo<TRequest> requestType, JsonTypeInfo<TResult> resultType)
    {
        var httpRequest = GetRequestMessage(Endpoint, request, requestType);

        await SignAsync(httpRequest).ConfigureAwait(false);

        using var response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            throw new AwsException($"{response.StatusCode} -> {responseText}", response.StatusCode);         
        }

        var result = await response.Content.ReadFromJsonAsync(resultType).ConfigureAwait(false);

        return result!;
    }

    internal static HttpRequestMessage GetRequestMessage<T>(string endpoint, T request, JsonTypeInfo<T> requestType)
    {
        ArgumentNullException.ThrowIfNull(request);

        var actionName = typeof(T).Name.Replace("Request", string.Empty);

        byte[] json = JsonSerializer.SerializeToUtf8Bytes(request, requestType);

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