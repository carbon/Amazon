using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Amazon.Rekognition;

public sealed class RekognitionClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Rekognition, region, credential)
{
    // public async Task CompareFacesAsync()

    // public async Task CopyProjectVersionAsync()

    // public async Task CreateCollectionAsync()

    // public async Task CreateDatasetAsync()

    // public async Task CreateProjectAsync()

    // public async Task CreateProjectVersionAsync()

    // public async Task CreateStreamProcessorAsync()

    // public async Task DeleteCollectionAsync()

    // public async Task DeleteDatasetAsync()

    // public async Task DeleteFacesAsync()

    // public async Task DeleteProjectAsync()

    // public async Task DeleteProjectPolicyAsync()

    // public async Task DeleteProjectVersionAsync()

    // public async Task DeleteStreamProcessorAsync()

    // public async Task DescribeCollectionAsync()

    // public async Task DescribeDatasetAsync()

    // public async Task DescribeProjectsAsync()

    // public async Task DescribeProjectVersionsAsync()

    // public async Task DescribeStreamProcessorAsync()

    // public async Task DetectCustomLabelsAsync()

    // public async Task DetectFacesAsync()

    public Task<DetectLabelsResult> DetectLabelsAsync(DetectLabelsRequest request)
    {
        return SendAsync<DetectLabelsRequest, DetectLabelsResult>("DetectLabels", request);
    }

    // public async Task DetectModerationLabelsAsync()

    // public async Task DetectProtectiveEquipmentAsync()

    // public async Task DetectTextAsync()

    // public async Task DistributeDatasetEntriesAsync()

    // public async Task GetCelebrityInfoAsync()

    // public async Task GetCelebrityRecognitionAsync()

    // public async Task GetContentModerationAsync()

    // public async Task GetFaceDetectionAsync()

    // public async Task GetFaceSearchAsync()

    // public async Task GetLabelDetectionAsync()

    // public async Task GetPersonTrackingAsync()

    // public async Task GetSegmentDetectionAsync()

    // public async Task GetTextDetectionAsync()

    // public async Task IndexFacesAsync()

    // public async Task ListCollectionsAsync()

    // public async Task ListDatasetEntriesAsync()

    // public async Task ListDatasetLabelsAsync()

    // public async Task ListFacesAsync()

    // public async Task ListProjectPoliciesAsync()

    // public async Task ListStreamProcessorsAsync()

    // public async Task ListTagsForResourceAsync()

    // public async Task PutProjectPolicyAsync()

    // public async Task RecognizeCelebritiesAsync()

    // public async Task SearchFacesAsync()

    // public async Task SearchFacesByImageAsync()

    // public async Task StartCelebrityRecognitionAsync()

    // public async Task StartContentModerationAsync()

    // public async Task StartFaceDetectionAsync()

    // public async Task StartFaceSearchAsync()

    // public async Task StartLabelDetectionAsync()

    // public async Task StartPersonTrackingAsync()

    // public async Task StartProjectVersionAsync()

    // public async Task StartSegmentDetectionAsync()

    // public async Task StartStreamProcessorAsync()

    // public async Task StartTextDetectionAsync()

    // public async Task StopProjectVersionAsync()

    // public async Task StopStreamProcessorAsync()

    // public async Task TagResourceAsync()

    // public async Task UntagResourceAsync()

    // public async Task UpdateDatasetEntriesAsync()

    // public async Task UpdateStreamProcessorAsync()

    #region Helpers

    private async Task<TResult> SendAsync<TRequest, TResult>(
        string action,
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest: IRekognitionRequest
        where TResult: notnull
    {
        var message = GetRequestMessage(action, request);

        await SignAsync(message).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<TResult>(JsonSerializerOptions.Default, cancellationToken).ConfigureAwait(false);

        return result!;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        throw new Exception(text);
    }

    private static readonly MediaTypeHeaderValue s_mediaType_xAmzJson1_1 = new("application/x-amz-json-1.1");

    private HttpRequestMessage GetRequestMessage<T>(string action, T request)
        where T : IRekognitionRequest
    {
        return new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = JsonContent.Create(request, s_mediaType_xAmzJson1_1, JsonSerializerOptions.Default),
            Headers = {
                { "x-amz-target", $"RekognitionService.{action}" }
            }
        };
    }

    #endregion
}