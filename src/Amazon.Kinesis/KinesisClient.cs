using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amazon.Kinesis;

public sealed class KinesisClient : AwsClient
{
    private const string TargetPrefix = "Kinesis_" + Version;
    private const string Version = "20131202";

    public KinesisClient(IAwsCredential credential)
        : base(AwsService.Kinesis, AwsRegion.USEast1, credential)
    { }
    
    public KinesisClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Kinesis, region.USEast1, credential)
    { }

    public KinesisStream GetStream(string name)
    {
        return new KinesisStream(name, this);
    }

    public Task<KinesisResponse> MergeShardsAsync(MergeShardsRequest request)
    {
        return SendAsync<MergeShardsRequest, KinesisResponse>("MergeShards", request);
    }

    public Task<PutRecordResult> PutRecordAsync(Record record)
    {
        return SendAsync<Record, PutRecordResult>("PutRecord", record);
    }

    public Task<PutRecordsResult> PutRecordsAsync(string streamName, Record[] records)
    {
        var request = new PutRecordsRequest(streamName, records);

        // TODO: retry failures?

        return SendAsync<PutRecordsRequest, PutRecordsResult>("PutRecords", request);
    }

    public Task<DescribeStreamResult> DescribeStreamAsync(DescribeStreamRequest request)
    {
        return SendAsync<DescribeStreamRequest, DescribeStreamResult>("DescribeStream", request);
    }

    public Task<GetShardIteratorResponse> GetShardIteratorAsync(GetShardIteratorRequest request)
    {
        return SendAsync<GetShardIteratorRequest, GetShardIteratorResponse>("GetShardIterator", request);
    }

    public Task<GetRecordsResponse> GetRecordsAsync(GetRecordsRequest request)
    {
        return SendAsync<GetRecordsRequest, GetRecordsResponse>("GetRecords", request);
    }

    #region Helpers

    private async Task<TResult> SendAsync<TRequest, TResult>(string action, TRequest request)
        where TRequest : KinesisRequest
        where TResult : KinesisResponse
    {
        var message = GetRequestMessage(action, request);

        string responseText = await SendAsync(message).ConfigureAwait(false);

        return JsonSerializer.Deserialize<TResult>(responseText)!;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string xmlText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var error = JsonSerializer.Deserialize<ErrorResult>(xmlText)!;

        error.Text = xmlText;

        return new KinesisException(error, response.StatusCode);
    }

    private static readonly JsonSerializerOptions jso = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private HttpRequestMessage GetRequestMessage<T>(string action, T request)
        where T : notnull, KinesisRequest
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, jso);

        return new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Headers = {
                { "x-amz-target", TargetPrefix  + "." + action }
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
