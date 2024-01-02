using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using KSC = Amazon.Kinesis.Serialization.KinesisSerializerContext;

namespace Amazon.Kinesis;

public sealed class KinesisClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Kinesis, region, credential)
{
    private const string TargetPrefix = $"Kinesis_{Version}";
    private const string Version = "20131202";

    public KinesisStream GetStream(string name)
    {
        return new KinesisStream(name, this);
    }

    public Task MergeShardsAsync(MergeShardsRequest request)
    {
        return SendAsync("MergeShards", request, KSC.Default.MergeShardsRequest);
    }

    public Task<PutRecordResult> PutRecordAsync(Record record)
    {
        return SendAsync("PutRecord", record, KSC.Default.Record, KSC.Default.PutRecordResult);
    }

    public Task<PutRecordsResult> PutRecordsAsync(string streamName, Record[] records)
    {
        var request = new PutRecordsRequest(streamName, records);

        // TODO: retry failures?

        return SendAsync("PutRecords", request, KSC.Default.PutRecordsRequest, KSC.Default.PutRecordsResult);
    }

    public Task<DescribeStreamResult> DescribeStreamAsync(DescribeStreamRequest request)
    {
        return SendAsync("DescribeStream", request, KSC.Default.DescribeStreamRequest, KSC.Default.DescribeStreamResult);
    }

    public Task<GetShardIteratorResult> GetShardIteratorAsync(GetShardIteratorRequest request)
    {
        return SendAsync("GetShardIterator", request, KSC.Default.GetShardIteratorRequest, KSC.Default.GetShardIteratorResult);
    }

    public Task<GetRecordsResult> GetRecordsAsync(GetRecordsRequest request)
    {
        return SendAsync("GetRecords", request, KSC.Default.GetRecordsRequest, KSC.Default.GetRecordsResult);
    }

    #region Helpers

    private async Task<TResult> SendAsync<TRequest, TResult>(
        string action, 
        TRequest request,
        JsonTypeInfo<TRequest> requestTypeInfo,
        JsonTypeInfo<TResult> resultTypeInfo)
        where TRequest : KinesisRequest
        where TResult : KinesisResult
    {
        var requestMessage = GetRequestMessage(action, request, requestTypeInfo);

        await SignAsync(requestMessage).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync(resultTypeInfo).ConfigureAwait(false);

        return result!;
    }

    private async Task SendAsync<TRequest>(string action, TRequest request, JsonTypeInfo<TRequest> requestTypeInfo)
       where TRequest : KinesisRequest
    {
        var message = GetRequestMessage(action, request, requestTypeInfo);

        _ = await SendAsync(message).ConfigureAwait(false);
    }


    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string xmlText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        ErrorResult error = JsonSerializer.Deserialize(xmlText, KSC.Default.ErrorResult)!;

        error.Text = xmlText;

        return new KinesisException(error, response.StatusCode);
    }

    private HttpRequestMessage GetRequestMessage<T>(string action, T request, JsonTypeInfo<T> requestTypeInfo)
        where T : notnull, KinesisRequest
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, requestTypeInfo);

        return new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Headers = {
                { "x-amz-target", $"{TargetPrefix}.{action}" }
            },
            Content = new ByteArrayContent(jsonBytes) {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.1" }
                }
            }
        };
    }

    #endregion
}
