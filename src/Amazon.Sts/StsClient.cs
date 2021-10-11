using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Sts.Exceptions;

namespace Amazon.Sts;

public sealed class StsClient : AwsClient
{
    public const string Version = "2011-06-15";
    public const string Namespace = "https://sts.amazonaws.com/doc/2011-06-15/";

    public StsClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Sts, region, credential)
    {
    }

    public Task<AssumeRoleResponse> AssumeRoleAsync(AssumeRoleResponseRequest request)
    {
        return SendAsync<AssumeRoleResponse>(request);
    }

    public Task<AssumeRoleWithSAMLResponse> AssumeRoleWithSAMLAsync(AssumeRoleWithSAMLRequest request)
    {
        return SendAsync<AssumeRoleWithSAMLResponse>(request);
    }

    public Task<AssumeRoleWithWebIdentityResponse> AssumeRoleWithWebIdentityAsync(AssumeRoleWithWebIdentityRequest request)
    {
        return SendAsync<AssumeRoleWithWebIdentityResponse>(request);
    }

    public Task<DecodeAuthorizationMessageResponse> DecodeAuthorizationMessageAsync(DecodeAuthorizationMessageRequest request)
    {
        return SendAsync<DecodeAuthorizationMessageResponse>(request);
    }

    public Task<GetCallerIdentityResponse> GetCallerIdentityAsync()
    {
        return SendAsync<GetCallerIdentityResponse>(GetCallerIdentityRequest.Default);
    }

    public async ValueTask<CallerIdentityVerificationParameters> GetCallerIdentityVerificationParametersAsync()
    {
        const string body = "Action=GetCallerIdentity&Version=" + Version;

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = new ByteArrayContent(Encoding.ASCII.GetBytes(body))
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        var headers = new Dictionary<string, string>(4);

        foreach (var header in httpRequest.Headers)
        {
            if (header.Key.StartsWith("x-amz-", StringComparison.Ordinal)
             || header.Key.Equals("Authorization", StringComparison.Ordinal))
            {
                headers.Add(header.Key, string.Join(";", header.Value));
            }
        }

        return new CallerIdentityVerificationParameters(
            url     : httpRequest.RequestUri!.ToString(),
            headers : headers,
            body    : body
        );
    }

    public Task<GetFederationTokenResponse> GetFederationTokenAsync(GetFederationTokenRequest request)
    {
        return SendAsync<GetFederationTokenResponse>(request);
    }

    public Task<GetSessionTokenResponse> GetSessionTokenAsync(GetSessionTokenRequest request)
    {
        return SendAsync<GetSessionTokenResponse>(request);
    }

    #region API Helpers

    private async Task<TResponse> SendAsync<TResponse>(IStsRequest request)
      where TResponse : IStsResponse
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(StsRequestHelper.ToParams(request))
        };

        string responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return StsSerializer<TResponse>.ParseXml(responseText);
    }

    private static FormUrlEncodedContent GetPostContent(Dictionary<string, string> parameters)
    {
        parameters.Add("Version", Version);

        return new FormUrlEncodedContent(parameters!);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        throw new StsException(response.StatusCode, responseText);
    }

    #endregion
}
