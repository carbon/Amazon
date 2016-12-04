using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Amazon.Ec2
{
    // http://docs.aws.amazon.com/AWSEC2/latest/APIReference/Welcome.html

    public class Ec2Client : AwsClient
    {
        public static string Version = "2016-09-15";
        public static readonly XNamespace NS = "http://ec2.amazonaws.com/doc/2016-09-15/";

        public Ec2Client(AwsRegion region, IAwsCredentials credentials)
            : base(AwsService.Ec2, region, credentials)
        { }

        #region Shortcuts

        public async Task<Instance> DescribeInstanceAsync(string instanceId)
        {
            var result = await DescribeInstancesAsync(new DescribeInstancesRequest { InstanceIds = { instanceId } });

            return result.Instances.Count > 0 ? result.Instances[0] : null;
        }

        public async Task<Volume> DescribeVolumeAsync(string volumeId)
        {
            var result = await DescribeVolumesAsync(new DescribeVolumesRequest { VolumeIds = { volumeId } });

            return result.Volumes.Count > 0 ? result.Volumes[0] : null;
        }

        #endregion

        public async Task<DescribeVolumesResponse> DescribeVolumesAsync(DescribeVolumesRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return DescribeVolumesResponse.Parse(responseText);
        }

        public async Task<DescribeInstancesResponse> DescribeInstancesAsync(DescribeInstancesRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return DescribeInstancesResponse.Parse(responseText);
        }

        #region Helpers

        private FormUrlEncodedContent GetPostContent(AwsRequest request)
        {
            request.Add("Version", Version);

            return new FormUrlEncodedContent(request.Parameters);
        }

        protected override async Task<Exception> GetException(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync();

            throw new Exception(response.StatusCode + "/" + responseText);
        }

        #endregion
    }
}