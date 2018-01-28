using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Kms
{
    public sealed class KmsClient : AwsClient
    {
        public const string Version = "2014-11-01";

        public KmsClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.Kms, region, credential)
        { }

        #region Aliases

        public Task<CreateAliasResponse> CreateAliasAsync(CreateAliasRequest request)
        {
            return SendAsync<CreateAliasResponse>("CreateAlias", request);
        }

        #endregion

        #region Grants

        public Task<CreateGrantResponse> CreateGrantAsync(CreateGrantRequest request)
        {
            return SendAsync<CreateGrantResponse>("CreateGrant", request);
        }

        public Task<RetireGrantResponse> RetireGrantAsync(RetireGrantRequest request)
        {
            return SendAsync<RetireGrantResponse>("RetireGrant", request);
        }

        public Task<ListGrantsResponse> ListGrantsAsync(ListGrantsRequest request)
        {
            return SendAsync<ListGrantsResponse>("ListGrants", request);
        }

        #endregion

        public Task<EncryptResponse> EncryptAsync(EncryptRequest request)
        {
            return SendAsync<EncryptResponse>("Encrypt", request);
        }

        public Task<DecryptResponse> DecryptAsync(DecryptRequest request)
        {
            return SendAsync<DecryptResponse>("Decrypt", request);
        }

        #region Data Keys

        public Task<GenerateDataKeyResponse> GenerateDataKeyAsync(GenerateDataKeyRequest request)
        {
            return SendAsync<GenerateDataKeyResponse>("GenerateDataKey", request);
        }

        public Task<GenerateDataKeyResponse> GenerateDataKeyWithoutPlaintextAsync(GenerateDataKeyRequest request)
        {
            return SendAsync<GenerateDataKeyResponse>("GenerateDataKeyWithoutPlaintext", request);
        }

        #endregion

        #region Helpers

        private async Task<T> SendAsync<T>(string action, KmsRequest request)
            where T : KmsResponse, new()
        {
            var jsonText = JsonObject.FromObject(request).ToString(pretty: false);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Headers = {
                    { "x-amz-target", "TrentService." + action }
                },

                Content = new StringContent(jsonText, Encoding.UTF8, "application/x-amz-json-1.1")
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            if (responseText == "") return null;

            return JsonObject.Parse(responseText).As<T>();
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseText.StartsWith("{"))
            {
                var error = JsonObject.Parse(responseText).As<KmsError>();

                if (error.Type == "AccessDeniedException")
                {
                    return new AccessDeniedException(error.Message);
                }
                else
                {
                    return new KmsException(error);
                }
            }
            else
            {
                throw new Exception(responseText);
            }
        }

        #endregion
    }
}

// http://docs.aws.amazon.com/kms/latest/APIReference/Welcome.html