using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Amazon.Bedrock.Actions;
using Amazon.Bedrock.Exceptions;

namespace Amazon.Bedrock;

public class BedrockClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(new AwsService("bedrock"), region, credential)
{
    public const string Version = "2022-01-01";

    public async Task<ListFoundationalModelsResult> ListFoundationalModelsAsync()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://bedrock.{Region.Name}.amazonaws.com/foundation-models") {
            Headers = {
                { "Accept", MediaTypeNames.Application.Json }
            }
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<ListFoundationalModelsResult>();

        return result!;
    }

    // runtime.

    public async Task<TResult> InvokeModelAsync<TRequest, TResult>(string modelId, TRequest request)
    {
        var content = JsonContent.Create(request);

        content.Headers.ContentType!.CharSet = null; // otherwise, throws

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-runtime.{Region.Name}.amazonaws.com/model/{modelId}/invoke") {
            Headers = {
                { "Accept", MediaTypeNames.Application.Json }
            },
            Content = content
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var text = await response.Content.ReadAsStringAsync();

        return (await response.Content.ReadFromJsonAsync<TResult>())!;
    }

    public async Task<ConverseResult> ConverseAsync(string modelId, ConverseRequest request)
    {
        var content = JsonContent.Create(request);

        content.Headers.ContentType!.CharSet = null; // otherwise, throws

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-runtime.{Region.Name}.amazonaws.com/model/{modelId}/converse") {
            Content = content
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<ConverseResult>().ConfigureAwait(false);

        return result!;
    }

    public async Task<RerankResult> RerankAsync(RerankRequest request)
    {
        var content = JsonContent.Create(request);

        content.Headers.ContentType!.CharSet = null; // otherwise, throws

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-agent-runtime.{Region.Name}.amazonaws.com/rerank") {
            Content = content
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<RerankResult>().ConfigureAwait(false);

        return result!;
    }

    public async Task<string> InvokeModelAsync(string modelId, HttpContent content, string accept = "application/json")
    {
        content.Headers.ContentType!.CharSet = null; // otherwise, throws

        var request = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-runtime.{Region.Name}.amazonaws.com/model/{modelId}/invoke") {
            Headers = {
                { "Accept", accept }
            },
            Content = content
        };

        // https://bedrock-runtime.us-east-1.amazonaws.com/model/amazon.titan-text-express-v1/invoke


        await SignAsync(request).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        byte[] responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        if (response.Content.Headers.ContentType?.MediaType is MediaTypeNames.Application.Json)
        {
            // {"message":"The system encountered an unexpected error during processing. Try your request again."}
            // {"message":"You are not authorized to invoke the Rerank operation."}

            if (JsonSerializer.Deserialize<Error>(responseBytes) is Error { Message: string errorMessage })
            {
                return new BedrockException(errorMessage, response.StatusCode);
            }
        }
        

        return new BedrockException(Encoding.UTF8.GetString(responseBytes), response.StatusCode);
    }
}