using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.CodeBuild
{
    public sealed class CodeBuildClient : AwsClient
    {
        public const string Version = "2016-10-06";

        public CodeBuildClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.CodeBuild, region, credential) { }

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

        private async Task<T> SendAsync<T>(ICodeBuildRequest request)
              where T : new()
        {
            var responseText = await SendAsync(GetRequestMessage(Endpoint, request)).ConfigureAwait(false);

            if (responseText.Length == 0) return new T();

            // TEMP try / catch ... remove once all the JSON deserialization methods are verified

            try
            {
                return JsonSerializer.Deserialize<T>(responseText, jso);
            }
            catch(Exception ex)
            {
                throw new Exception("Dserialize error for " + responseText, ex);
            }
        }

        private static readonly JsonSerializerOptions jso = new JsonSerializerOptions {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static HttpRequestMessage GetRequestMessage(string endpoint, object request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var actionName = request.GetType().Name.Replace("Request", string.Empty);

            byte[] json = JsonSerializer.SerializeToUtf8Bytes(request, jso);

            // 2016-10-06
            // X-Amz-Target: CodeBuild_20161006.StopBuild

            return new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Headers = {
                    { "x-amz-target", "CodeBuild_20161006." + actionName },
                },
                Content = new ByteArrayContent(json) {
                    Headers = {
                        { "Content-Type", "application/x-amz-json-1.1; charset=utf-8" }
                    }
                }
            };
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            string responseText = await response.Content.ReadAsStringAsync();

            throw new Exception(response.StatusCode + "/" + responseText);
        }

        #endregion
    }
}

// ref: http://docs.aws.amazon.com/codebuild/latest/APIReference/Welcome.html