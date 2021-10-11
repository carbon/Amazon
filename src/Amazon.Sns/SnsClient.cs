using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amazon.Sns;

public sealed class SnsClient : AwsClient
{
    public const string Version = "2010-03-31";

    public SnsClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Sns, region, credential)
    { }

    public async Task<string> PublishAsync(PublishRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetFormContent(request.ToParams())
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    #region Helpers

    private static FormUrlEncodedContent GetFormContent(Dictionary<string, string> parameters)
    {
        parameters.Add("Version", Version);

        return new FormUrlEncodedContent(parameters!);
    }

    #endregion
}

// http://docs.aws.amazon.com/sns/latest/api/Welcome.html
// sns:Publish	This grants permission to publish to a topic.