using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

using Amazon.Exceptions;
using Amazon.Transcribe.Serialization;

namespace Amazon.Transcribe;

public sealed class TranscribeClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(new AwsService("transcribe"), region, credential)
{
    public Task<StartTranscriptionJobResult> StartTranscriptionJobAsync(StartTranscriptionJobRequest request)
    {
        var requestMessage = ConstructPostRequest("StartTranscriptionJob", request, TranscribeSerializerContext.Default.StartTranscriptionJobRequest);

        return SendAsync(requestMessage, TranscribeSerializerContext.Default.StartTranscriptionJobResult);
    }

    public Task<GetTranscriptionJobResult> GetTranscriptionJobAsync(GetTranscriptionJobRequest request)
    {
        var requestMessage = ConstructPostRequest("GetTranscriptionJob", request, TranscribeSerializerContext.Default.GetTranscriptionJobRequest);

        return SendAsync(requestMessage, TranscribeSerializerContext.Default.GetTranscriptionJobResult);
    }

    private static readonly MediaTypeHeaderValue s_mediaType_xAmzJson1_0 = new("application/x-amz-json-1.1");

    internal HttpRequestMessage ConstructPostRequest<TRequest>(
        [ConstantExpected] string action,
        TRequest request,
        JsonTypeInfo<TRequest> jsonTypeInfo)
    {
        return new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Headers = {
                { "x-amz-target", $"com.amazonaws.transcribe.Transcribe.{action}" },
            },
            Content = JsonContent.Create(request, jsonTypeInfo, mediaType: s_mediaType_xAmzJson1_0)
        };
    }

    private async Task<TResult> SendAsync<TResult>(
        HttpRequestMessage request,
        JsonTypeInfo<TResult> jsonTypeInfo,
        CancellationToken cancellationToken = default)
    {
        await SignAsync(request).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }
              
        var result = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken).ConfigureAwait(false);

        return result!;
    }

    #region Helpers

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        if (response.StatusCode is HttpStatusCode.ServiceUnavailable) // 503
        {
            return new ServiceUnavailableException();
        }
       
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new AwsException($"status = {response.StatusCode}. response = {responseText}", response.StatusCode);
        
    }

    #endregion
}