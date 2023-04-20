﻿using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class RekognitionClient : AwsClient
{
    public const string Version = "2016-06-27";

    public RekognitionClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Rekognition, region, credential)
    {
    }

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

    private async Task<TResult> SendAsync<TRequest, TResult>(string action, TRequest request, CancellationToken cancellationToken = default)
        where TRequest: notnull
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
        string xmlText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        throw new Exception(xmlText);
    }

    private static readonly JsonSerializerOptions jso = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private HttpRequestMessage GetRequestMessage<T>(string action, T request)
        where T : notnull
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, jso);

        return new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Headers = {
                { "x-amz-target", $"RekognitionService.{action}" }
            },
            Content = new ByteArrayContent(jsonBytes)
            {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.1" }
                }
            }
        };
    }

    #endregion
}