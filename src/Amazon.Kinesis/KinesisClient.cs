using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Kinesis.Serialization;

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

    public Task<KinesisResult> MergeShardsAsync(MergeShardsRequest request)
    {
        return SendAsync<MergeShardsRequest, KinesisResult>("MergeShards", request);
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

    public Task<GetShardIteratorResult> GetShardIteratorAsync(GetShardIteratorRequest request)
    {
        return SendAsync<GetShardIteratorRequest, GetShardIteratorResult>("GetShardIterator", request);
    }

    public Task<GetRecordsResult> GetRecordsAsync(GetRecordsRequest request)
    {
        return SendAsync<GetRecordsRequest, GetRecordsResult>("GetRecords", request);
    }

    #region Helpers

    private async Task<TResult> SendAsync<TRequest, TResult>([ConstantExpected] string action, TRequest request)
        where TRequest : KinesisRequest
        where TResult : KinesisResult
    {
        var message = GetRequestMessage(action, request);

        string responseText = await SendAsync(message).ConfigureAwait(false);

        return JsonSerializer.Deserialize<TResult>(responseText)!;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string xmlText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var error = JsonSerializer.Deserialize<ErrorResult>(xmlText, KinesisSerializerContext.Default.ErrorResult)!;

        error.Text = xmlText;

        return new KinesisException(error, response.StatusCode);
    }

    private static readonly JsonSerializerOptions s_jso = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private HttpRequestMessage GetRequestMessage<T>(string action, T request)
        where T : notnull, KinesisRequest
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, s_jso);

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
