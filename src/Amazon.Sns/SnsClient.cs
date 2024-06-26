﻿using System.Net.Http;

namespace Amazon.Sns;

public sealed class SnsClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Sns, region, credential)
{
    public const string Version = "2010-03-31";

    public async Task<byte[]> PublishAsync(PublishRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetFormContent(request.ToParams())
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    #region Helpers

    private static FormUrlEncodedContent GetFormContent(List<KeyValuePair<string, string>> parameters)
    {
        parameters.Add(new("Version", Version));

        return new FormUrlEncodedContent(parameters);
    }

    #endregion
}

// http://docs.aws.amazon.com/sns/latest/api/Welcome.html
// sns:Publish	This grants permission to publish to a topic.