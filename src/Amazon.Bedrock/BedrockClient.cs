using System.Net.Http.Json;

namespace Amazon.Bedrock;

public class BedrockClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Bedrock, region, credential)
{
    public const string Version = "2022-01-01";

    // runtime.

    public async Task<TResult> InvokeModelAsync<TRequest, TResult>(string modelId, TRequest request)
    {
        var content = JsonContent.Create(request);

        content.Headers.ContentType!.CharSet = null; // otherwise, throws

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-runtime.{Region.Name}.amazonaws.com/model/{modelId}/invoke") {
            Headers = {
                { "Accept", "application/json" }
            },
            Content = content
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        return (await response.Content.ReadFromJsonAsync<TResult>())!;
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
}