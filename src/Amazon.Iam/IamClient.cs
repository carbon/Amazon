using System.Collections.Generic;
using System.Net.Http;

namespace Amazon.Iam;

public sealed class IamClient(AwsRegion region, AwsCredential credential) 
    : AwsClient(AwsService.Iam, region, credential)
{
    public const string Version = "2010-05-08";

    public async Task CreateAccessKeyAsync(string userName)
    {
        List<KeyValuePair<string, string>> parameters = [
            new("Action", "CreateAccessKey"),
            new("UserName", userName)
        ];

        await SendAsync(parameters).ConfigureAwait(false);
    }

    public async Task CreateUserAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        List<KeyValuePair<string, string>> parameters = [
            new("Action", "CreateUser"),
            new("UserName", userName)
        ];

        await SendAsync(parameters).ConfigureAwait(false);
    }

    public async Task PutUserPolicyAsync(string userName, string policyName, string policyDocument)
    {
        List<KeyValuePair<string, string>> parameters = [
            new("Action", "PutUserPolicy"),
            new("UserName", userName),
            new("PolicyName", policyName),
            new("PolicyDocument", policyDocument)
        ];

        await SendAsync(parameters).ConfigureAwait(false);
    }

    #region Helpers

    private Task<byte[]> SendAsync(List<KeyValuePair<string, string>> parameters)
    {
        parameters.Add(new("Version", Version));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = new FormUrlEncodedContent(parameters)
        };

        return SendAsync(httpRequest);
    }

    #endregion
}

// http://docs.aws.amazon.com/IAM/latest/APIReference/Welcome.html