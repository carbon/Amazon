﻿using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Amazon.Lambda;

public sealed class LambdaClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Lambda, region, credential)
{
    public const string Version = "2015-03-31";

    // lambda:InvokeFunction

    public Task<InvokeResult> InvokeAsync<T>(string functionName, T param)
    {
        return InvokeFunctionAsync(new InvokeRequest(functionName, JsonSerializer.Serialize(param)));
    }

    public async Task<InvokeResult> InvokeFunctionAsync(InvokeRequest message)
    {
        // /2015-03-31/functions/FunctionName/invocations

        var url = $"{Endpoint}{Version}/functions/{message.FunctionName}/invocations";

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url) {
            Content = new StringContent(message.Payload!, Encoding.UTF8)
        };

        if (message.InvocationType != null)
        {
            httpRequest.Headers.Add("X-Amz-Invocation-Type", message.InvocationType.Value.ToString());
        }

        if (message.LogType is LogType logType)
        {
            httpRequest.Headers.Add("X-Amz-Log-Type", logType.ToString());
        }

        var responseBytes = await SendAsync(httpRequest).ConfigureAwait(false);

        return new InvokeResult(responseBytes);
    }
}

// http://docs.aws.amazon.com/lambda/latest/dg/API_Reference.html