using System.Net.Http;

namespace Amazon.CloudWatch;

// http://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/Welcome.html

public sealed class CloudWatchClient(AwsRegion region, IAwsCredential credentials) 
    : AwsClient(AwsService.Monitoring, region, credentials)
{
    public const string Version = "2010-08-01";

    public static readonly string NS = "http://monitoring.amazonaws.com/doc/2010-08-01/";

    /*
    public async Task DeleteAlarmsAsync() { }

    public async Task DescribeAlarmHistoryAsync() { }

    public async Task DescribeAlarmsAsync() { }

    public async Task DescribeAlarmsForMetricAsync() { }

    public async Task DisableAlarmActions() { }

    public async Task EnableAlarmActionsAsync() { }
    */

    public async Task<GetMetricStatisticsResponse> GetMetricStatisticsAsync(GetMetricStatisticsRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParameters())
        };

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return GetMetricStatisticsResponse.Deserialize(responseText);
    }

    public async Task<List<Metric>> ListMetricsAsyncAsync(ListMetricsRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParameters())
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
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = GetPostContent(request.ToParameters())
        };

        await SendAsync(httpRequest).ConfigureAwait(false);

        // return PutMetricDataResult.Parse(responseText);
    }

    /*
    public async Task SetAlarmStateAsync()
    {
    }
    */

    #region Helpers

    private static FormUrlEncodedContent GetPostContent(List<KeyValuePair<string, string>> nvc)
    {
        nvc.Add(new("Version", Version));

        return new FormUrlEncodedContent(nvc);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        var responseText = await response.Content.ReadAsStringAsync();

        throw new Exception($"{response.StatusCode} - {responseText}");
    }

    #endregion
}
