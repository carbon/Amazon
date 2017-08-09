using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Carbon.Json;

namespace Amazon.Sts
{
    public class StsClient : AwsClient
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
        
        public async Task<CallerIdentityVerificationParameters> GetCallerIdentityVerificationParametersAsync()
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(StsRequestHelper.ToParams(GetCallerIdentityRequest.Default))
            };

            await SignAsync(httpRequest);

            var headers = new JsonObject();

            foreach (var header in httpRequest.Headers)
            {
                if (header.Key.StartsWith("x-amz-") || header.Key == "Authorization")
                {
                    headers.Add(header.Key, string.Join(";", header.Value));
                }
            }

            return new CallerIdentityVerificationParameters(
                url     : httpRequest.RequestUri.ToString(), 
                headers : headers, 
                body    : await httpRequest.Content.ReadAsStringAsync().ConfigureAwait(false)
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
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = GetPostContent(StsRequestHelper.ToParams(request))
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return StsResponseHelper<TResponse>.ParseXml(responseText);
        }

        private FormUrlEncodedContent GetPostContent(Dictionary<string, string> parameters)
        {
            parameters.Add("Version", Version);

            return new FormUrlEncodedContent(parameters);
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            throw new Exception(response.StatusCode + "/" + responseText);
        }

        #endregion
    }
}
