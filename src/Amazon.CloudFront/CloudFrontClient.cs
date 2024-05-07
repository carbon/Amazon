using System.Net.Http;
using System.Threading.Tasks;

using Amazon.CloudFront.Serialization;

namespace Amazon.CloudFront;

public sealed class CloudFrontClient : AwsClient
{
    private const string version = "2020-05-31";
    internal const string Namespace = "http://cloudfront.amazonaws.com/doc/2020-05-31/";

    private readonly string _baseUrl;

    public CloudFrontClient(AwsRegion region, IAwsCredential credentials)
        : base(AwsService.CloudFront, region, credentials)
    {
        _baseUrl = Endpoint + version;
    }

    public async Task<byte[]> CreateInvalidationBatch(string distributionId, InvalidationBatch batch)
    {
        var requestUri = $"{_baseUrl}/distribution/{distributionId}/invalidation";

        var bytes = CloudFrontSerializer<InvalidationBatch>.SerializeToUtf8Bytes(batch);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri) {
            Content = new ByteArrayContent(bytes) {
                Headers = {
                    {  "Content-Type", "text/xml" }
                }
            }
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    public async Task<byte[]> CreateDistribution(DistributionConfig request)
    {
        var requestUri = $"{_baseUrl}/distribution";

        var bytes = CloudFrontSerializer<DistributionConfig>.SerializeToUtf8Bytes(request);
       
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri) {
            Content = new ByteArrayContent(bytes) {
                Headers = {
                    {  "Content-Type", "text/xml" }
                }
            }
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    public async Task<byte[]> PutDistribution(string id, DistributionConfig request)
    {
        var requestUri = $"{_baseUrl}/distribution/{id}/config";

        var bytes = CloudFrontSerializer<DistributionConfig>.SerializeToUtf8Bytes(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri) {
            Content = new ByteArrayContent(bytes) {
                Headers = {
                    {  "Content-Type", "text/xml" }
                }
            }
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }
}

// http://docs.aws.amazon.com/AmazonCloudFront/latest/APIReference/Welcome.html