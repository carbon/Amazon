using System;
using System.Threading.Tasks;
using Xunit;

namespace Amazon.CloudWatch.Tests
{
    public class CloudWatchTests
    {
        private readonly AwsCredential credential = new AwsCredential("x", "x");

       //  [Fact]
        public async Task PostMetric()
        {

            // var client = new CloudWatchClient(AwsRegion.USEast1, key);

            // var result = await client.PutMetricDatum(new ListMetricsRequest { Namespace = "AWS/ELB", MetricName = "HealthyHostCount" });

            // throw new Exception(result);
        }

       // [Fact]
        public async Task Test1()
        {

            var client = new CloudWatchClient(AwsRegion.USEast1, credential);

            var result = await client.ListMetricsAsyncAsync(new ListMetricsRequest { Namespace = "AWS/ELB", MetricName = "HealthyHostCount" });

            // throw new Exception(result);
        }

        //[Fact]
        public async Task Test2()
        {
            var client = new CloudWatchClient(AwsRegion.USEast1, credential);

            var result = await client.GetMetricStatisticsAsync(new GetMetricStatisticsRequest("AWS/ELB", "HealthyHostCount") {
                StartTime  = DateTime.UtcNow.AddHours(-12),
                EndTime    = DateTime.UtcNow,
                Period     = TimeSpan.FromHours(1),
                Statistics = new[] { Statistic.Average }
            });

            // throw new System.Exception(result);
        }

        
    }
}
 