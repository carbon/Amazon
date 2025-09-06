using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using Amazon.Exceptions;
using Amazon.Kinesis.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class KinesisFirehoseClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.KinesisFirehose, region, credential)
{
    const string Version = "20150804";
    const string TargetPrefix = $"Firehose_{Version}";

    private static readonly KinesisFirehoseSerializerContext jsc = KinesisFirehoseSerializerContext.Default;

    public DeliveryStream GetStream(string name)
    {
        return new DeliveryStream(name, this);
    }

    public Task<CreateDeliveryStreamResult> CreateDeliveryStreamAsync(CreateDeliveryStreamRequest request)
    {
        return SendAsync("CreateDeliveryStream", request, jsc.CreateDeliveryStreamRequest, jsc.CreateDeliveryStreamResult);
    }

    public Task<DeleteDeliveryStreamResult> DeleteDeliveryStreamAsync(DeleteDeliveryStreamRequest request)
    {
        return SendAsync("PutRecordBatch", request, jsc.DeleteDeliveryStreamRequest, jsc.DeleteDeliveryStreamResult);
    }

    public Task<DescribeDeliveryStreamResult> DescribeDeliveryStreamAsync(DescribeDeliveryStreamRequest request)
    {
        return SendAsync("DescribeDeliveryStream", request, jsc.DescribeDeliveryStreamRequest, jsc.DescribeDeliveryStreamResult);
    }

    public Task<ListDeliveryStreamsRequest> ListDeliveryStreamsAsync(ListDeliveryStreamsRequest request)
    {
        return SendAsync("ListDeliveryStreams", request, jsc.ListDeliveryStreamsRequest, jsc.ListDeliveryStreamsRequest);
    }

    public Task<PutRecordResult> PutRecordAsync(PutRecordRequest request)
    {
        return SendAsync("PutRecord", request, jsc.PutRecordRequest, jsc.PutRecordResult);
    }

    public Task<PutRecordBatchResult> PutRecordBatchAsync(PutRecordBatchRequest request)
    {
        return SendAsync("PutRecordBatch", request, jsc.PutRecordBatchRequest, jsc.PutRecordBatchResult);
    }

    // public void UpdateDestinationAsync(UpdateDestinationRequest request) { }

    #region Helpers

    private async Task<TResult> SendAsync<TRequest, TResult>(
        string action,
        TRequest request, 
        JsonTypeInfo<TRequest> requestJsonType,
        JsonTypeInfo<TResult> resultJsonType)
        where TRequest: notnull
    {
        var httpRequest = GetRequestMessage(action, request, requestJsonType);

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return JsonSerializer.Deserialize(responseText, resultJsonType)!;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        throw new AwsException(responseText, response.StatusCode);
    }

    private static readonly JsonSerializerOptions s_jso = new () {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private HttpRequestMessage GetRequestMessage<T>(string action, T data, JsonTypeInfo<T> requestJsonType)
        where T: notnull
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(data, requestJsonType);

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