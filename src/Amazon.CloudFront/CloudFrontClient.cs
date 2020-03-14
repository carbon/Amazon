using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.CloudFront
{
    public sealed class CloudFrontClient : AwsClient
    {
        private const string version = "2015-04-17";

        private readonly string baseUrl;

        public CloudFrontClient(AwsCredential credentials)
            : base(AwsService.Cloudfront, AwsRegion.USEast1, credentials)
        {
            baseUrl = Endpoint + version;
        }

        public async Task<string> CreateInvalidationBatch(string distributionId, InvalidationBatch batch)
        {
            var requestUri = $"{baseUrl}/distribution/{distributionId}/invalidation";

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri) {
                Content = new StringContent(batch.ToXml().ToString(), Encoding.UTF8, "text/xml")
            };

            return await SendAsync(httpRequest).ConfigureAwait(false);
        }

        public async Task<string> CreateDistribution(DistributionConfig request)
        {
            var requestUri = baseUrl + "/distribution";

            var body = @"<?xml version=""1.0"" encoding=""UTF-8""?>\n" + request.ToXml().ToString();

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(body, Encoding.UTF8, "text/xml")
            };

            return await SendAsync(httpRequest).ConfigureAwait(false);
        }

        public async Task<string> PutDistribution(string id, DistributionConfig request)
        {
            var requestUri = $"{baseUrl}/distribution/{id}/config";

            var httpRequest = new HttpRequestMessage(HttpMethod.Put, requestUri) {
                Content = new StringContent(
                    content: @"<?xml version=""1.0"" encoding=""UTF-8""?>\n" + request.ToXml().ToString(),
                    encoding: Encoding.UTF8,
                    mediaType: "text/xml"
                )
            };

            return await SendAsync(httpRequest).ConfigureAwait(false);
        }
    }
}

// http://docs.aws.amazon.com/AmazonCloudFront/latest/APIReference/Welcome.html