using System.Threading.Tasks;
using System.Net.Http;

namespace Amazon.Iam
{
    public sealed class IamClient : AwsClient
    {
        public const string Version = "2010-05-08";

        public IamClient(AwsCredential credential)
            : base(AwsService.Iam, AwsRegion.USEast1, credential)
        { }

        public async Task CreateAccessKey(string userName)
        {
            var parameters = new AwsRequest {
                { "Action", "CreateAccessKey" },
                { "UserName", userName }
            };

            await SendAsync(parameters);
        }

        public async Task CreateUser(string userName)
        {
            var parameters = new AwsRequest {
                { "Action", "CreateUser" },
                { "UserName", userName }
            };

            await SendAsync(parameters);
        }

        public async Task PutUserPolicy(string userName, string policyName, string policyDocument)
        {
            var parameters = new AwsRequest {
                { "Action", "PutUserPolicy" },
                { "UserName", userName },
                { "PolicyName", policyName },
                { "PolicyDocument", policyDocument }
            };

            await SendAsync(parameters);
        }

        #region Helpers

        private Task<string> SendAsync(AwsRequest request)
        {
            request.Add("Version", Version);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = new FormUrlEncodedContent(request.Parameters)
            };

            return SendAsync(httpRequest);
        }

        #endregion
    }
}

// http://docs.aws.amazon.com/IAM/latest/APIReference/Welcome.html