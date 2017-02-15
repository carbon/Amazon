using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Amazon.CloudWatch
{
    // http://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/Welcome.html

    public class CloudWatchClient : AwsClient
    {
        public static string Version = "2010-08-01";
        public static readonly XNamespace NS = "http://monitoring.amazonaws.com/doc/2010-08-01/";


        public CloudWatchClient(AwsRegion region, IAwsCredentials credentials)
            : base(AwsService.Monitoring, region, credentials)
        { }

        /*
        public async Task DeleteAlarmsAsync() { }

        public async Task DescribeAlarmHistoryAsync() { }

        public async Task DescribeAlarmsAsync() { }

        public async Task DesribeAlarmsForMetricAsync() { }

        public async Task DisableAlarmActions() { }

        public async Task EnableAlarmActionsAsync() { }
        */

        public async Task<GetMetricStatatisticsResponse> GetMetricStatisticsAsync(GetMetricStatisticsRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return GetMetricStatatisticsResponse.Parse(responseText);

            // return PutMetricDataResult.Parse(responseText);
        }

        public async Task<List<Metric>> ListMetricsAsyncAsync(ListMetricsRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return ListMetricsResponse.Parse(responseText);
        }

        /*
        public async Task PutMetricAlarmAsync()
        {
        }
        */

        public async Task PutMetricDataAsync(PutMetricDataRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            // return PutMetricDataResult.Parse(responseText);
        }

        /*
        public async Task SetAlarmStateAsync()
        {
        }
        */

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