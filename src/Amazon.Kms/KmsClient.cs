using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Kms
{
    public class KmsClient : AwsClient
    {
        public const string Version = "2014-11-01";

        public KmsClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.Kms, region, credential)
        { }

        public Task<CreateGrantResponse> CreateGrantAsync(CreateGrantRequest request) => 
            SendAsync<CreateGrantResponse>("CreateGrant", request);

        public Task<RetireGrantResponse> RetireGrantAsync(RetireGrantRequest request) => 
            SendAsync<RetireGrantResponse>("RetireGrant", request);

        public Task<ListGrantsResponse> ListGrantsAsync(ListGrantsRequest request) => 
            SendAsync<ListGrantsResponse>("ListGrants", request);

        public Task<EncryptResponse> EncryptAsync(EncryptRequest request) => 
            SendAsync<EncryptResponse>("Encrypt", request);

        public Task<DecryptResponse> DecryptAsync(DecryptRequest request) =>
            SendAsync<DecryptResponse>("Decrypt", request);

        public Task<GenerateDataKeyResponse> GenerateDataKeyAsync(GenerateDataKeyRequest request) => 
            SendAsync<GenerateDataKeyResponse>("GenerateDataKey", request);

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

            return JsonObject.Parse(responseText).As<T>();
        }

        #endregion
    }
}

// http://docs.aws.amazon.com/kms/latest/APIReference/Welcome.html
