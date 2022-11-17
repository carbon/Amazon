using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Sts.Exceptions;
using Amazon.Sts.Serialization;

namespace Amazon.Sts;

public sealed class StsClient : AwsClient
{
    public const string Version = "2011-06-15";
    public const string Namespace = "https://sts.amazonaws.com/doc/2011-06-15/";

    public StsClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Sts, region, credential)
    {
    }

    public Task<AssumeRoleResponse> AssumeRoleAsync(AssumeRoleRequest request)
    {
        return SendAsync<AssumeRoleRequest, AssumeRoleResponse>(request);
    }

    public Task<AssumeRoleWithSamlResponse> AssumeRoleWithSAMLAsync(AssumeRoleWithSamlRequest request)
    {
        return SendAsync<AssumeRoleWithSamlRequest, AssumeRoleWithSamlResponse>(request);
    }

    public Task<AssumeRoleWithWebIdentityResponse> AssumeRoleWithWebIdentityAsync(AssumeRoleWithWebIdentityRequest request)
    {
        return SendAsync<AssumeRoleWithWebIdentityRequest, AssumeRoleWithWebIdentityResponse>(request);
    }

    public Task<DecodeAuthorizationMessageResponse> DecodeAuthorizationMessageAsync(DecodeAuthorizationMessageRequest request)
    {
        return SendAsync<DecodeAuthorizationMessageRequest, DecodeAuthorizationMessageResponse>(request);
    }

    public Task<GetCallerIdentityResponse> GetCallerIdentityAsync()
    {
        return SendAsync<GetCallerIdentityRequest, GetCallerIdentityResponse>(GetCallerIdentityRequest.Default);
    }

    public async ValueTask<CallerIdentityVerificationParameters> GetCallerIdentityVerificationParametersAsync()
    {
        const string body = $"Action=GetCallerIdentity&Version={Version}";

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = new ByteArrayContent(Encoding.ASCII.GetBytes(body))
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        var headers = new Dictionary<string, string>(4);

        foreach (var header in httpRequest.Headers.NonValidated)
        {
            if (header.Key.StartsWith("x-amz-", StringComparison.Ordinal) || header.Key is "Authorization")
            {
                headers.Add(header.Key, header.Value.ToString());
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
        return SendAsync<GetFederationTokenRequest, GetFederationTokenResponse>(request);
    }

    public Task<GetSessionTokenResponse> GetSessionTokenAsync(GetSessionTokenRequest request)
    {
        return SendAsync<GetSessionTokenRequest, GetSessionTokenResponse>(request);
    }

    #region API Helpers

    private async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IStsRequest
        where TResponse : IStsResponse
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(StsRequestHelper.ToParams(request))
        };

        string responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return StsXmlSerializer<TResponse>.Deserialize(responseText);
    }

    private static FormUrlEncodedContent GetPostContent(List<KeyValuePair<string, string>> parameters)
    {
        parameters.Add(new ("Version", Version));

        return new FormUrlEncodedContent(parameters);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        throw new StsException(responseText, response.StatusCode);
    }

    #endregion
}