using System.Net.Http;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

// http://docs.aws.amazon.com/AmazonCloudWatch/latest/APIReference/Welcome.html

public sealed class CloudWatchClient(AwsRegion region, IAwsCredential credentials) 
    : AwsClient(AwsService.Monitoring, region, credentials)
{
    public const string Version = "2010-08-01";

    public static readonly string Namespace = "http://monitoring.amazonaws.com/doc/2010-08-01/";

    #region Logs

    #endregion


    #region Metrics

    /*
    public async Task DescribeAlarmHistoryAsync() { }

    public async Task DescribeAlarmsForMetricAsync() { }
    */

    /// <summary>
    /// Deletes up to 100 alarms. Alarms with correct names are still deleted if other names are incorrect.
    /// </summary>
    public Task DeleteAlarmsAsync(IReadOnlyList<string> alarmNames)
    {
        return SendAlarmNamesAsync("DeleteAlarms", alarmNames);
    }

    public async Task<DescribeAlarmsResult> DescribeAlarmsAsync(DescribeAlarmsRequest request)
    {
        var responseText = await PostAsync(request.ToParameters()).ConfigureAwait(false);

        return DescribeAlarmsResponse.Deserialize(responseText).DescribeAlarmsResult;
    }

    public Task DisableAlarmActionsAsync(IReadOnlyList<string> alarmNames)
    {
        return SendAlarmNamesAsync("DisableAlarmActions", alarmNames);
    }

    public Task EnableAlarmActionsAsync(IReadOnlyList<string> alarmNames)
    {
        return SendAlarmNamesAsync("EnableAlarmActions", alarmNames);
    }

    public async Task<GetMetricDataResult> GetMetricDataAsync(GetMetricDataRequest request)
    {
        var responseText = await PostAsync(request.ToParameters()).ConfigureAwait(false);

        return GetMetricDataResponse.Deserialize(responseText).GetMetricDataResult;
    }

    public async Task<GetMetricStatisticsResponse> GetMetricStatisticsAsync(GetMetricStatisticsRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParameters())
        };

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return GetMetricStatisticsResponse.Deserialize(responseText);
    }

    public async Task<ListMetricsResult> ListMetricsAsync(ListMetricsRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParameters())
        };

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return ListMetricsResponse.Deserialize(responseText).ListMetricsResult;
    }

    public async Task<List<Metric>> ListMetricsAsyncAsync(ListMetricsRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParameters())
        };

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return ListMetricsResponse.Parse(responseText);
    }

    public async Task PutMetricAlarmAsync(PutMetricAlarmRequest request)
    {
        await PostAsync(request.ToParameters()).ConfigureAwait(false);
    }

    public async Task PutMetricDataAsync(PutMetricDataRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = GetPostContent(request.ToParameters())
        };

        await SendAsync(httpRequest).ConfigureAwait(false);

        // return PutMetricDataResult.Parse(responseText);
    }

    public async Task SetAlarmStateAsync(SetAlarmStateRequest request)
    {
        await PostAsync(request.ToParameters()).ConfigureAwait(false);
    }

    #endregion

    #region Helpers

    private async Task<byte[]> PostAsync(List<KeyValuePair<string, string>> parameters)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(parameters)
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    private async Task SendAlarmNamesAsync(string action, IReadOnlyList<string> alarmNames)
    {
        ArgumentNullException.ThrowIfNull(alarmNames);

        if (alarmNames.Count is 0)
        {
            throw new ArgumentException("Must include at least one alarm name", nameof(alarmNames));
        }

        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", action)
        };

        parameters.AddMembers("AlarmNames", alarmNames);

        await PostAsync(parameters).ConfigureAwait(false);
    }

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