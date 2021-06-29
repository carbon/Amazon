using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amazon.Kinesis.Firehose
{
    public sealed class KinesisFirehoseClient : AwsClient
    {
        const string Version = "20150804";
        const string TargetPrefix = "Firehose_" + Version;

        public KinesisFirehoseClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.KinesisFirehose, region, credential) { }
        
        public DeliveryStream GetStream(string name)
        {
            return new DeliveryStream(name, this);
        }

        public Task<CreateDeliveryStreamResult> CreateDeliveryStreamAsync(CreateDeliveryStreamRequest request)
        {
            return SendAsync<CreateDeliveryStreamRequest, CreateDeliveryStreamResult>("CreateDeliveryStream", request);
        }

        public Task<DeleteDeliveryStreamResult> DeleteDeliveryStreamAsync(DeleteDeliveryStreamRequest request)
        {
            return SendAsync<DeleteDeliveryStreamRequest, DeleteDeliveryStreamResult>("PutRecordBatch", request);
        }

        public Task<DescribeDeliveryStreamResult> DescribeDeliveryStreamAsync(DescribeDeliveryStreamRequest request)
        {
            return SendAsync<DescribeDeliveryStreamRequest, DescribeDeliveryStreamResult>("DescribeDeliveryStream", request);
        }

        public Task<ListDeliveryStreamsRequest> ListDeliveryStreamsAsync(ListDeliveryStreamsRequest request)
        {
            return SendAsync<ListDeliveryStreamsRequest, ListDeliveryStreamsRequest>("ListDeliveryStreams", request);
        }

        public Task<PutRecordResult> PutRecordAsync(PutRecordRequest request)
        {
            return SendAsync<PutRecordRequest, PutRecordResult>("PutRecord", request);
        }

        public Task<PutRecordBatchResult> PutRecordBatchAsync(PutRecordBatchRequest request)
        {
            return SendAsync<PutRecordBatchRequest, PutRecordBatchResult>("PutRecordBatch", request);
        }

        // public void UpdateDestinationAsync(UpdateDestinationRequest request) { }

        #region Helpers

        private async Task<TResult> SendAsync<TRequest, TResult>(string action, TRequest request)
            where TRequest : notnull
            where TResult : notnull, new()
        {
            var httpRequest = GetRequestMessage(action, request);

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return JsonSerializer.Deserialize<TResult>(responseText)!;
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // var result = JsonObject.Parse(responseText);

            throw new Exception(responseText);
        }

        private static readonly JsonSerializerOptions jso = new () {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private HttpRequestMessage GetRequestMessage<T>(string action, T data)
            where T: notnull
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(data, jso);

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
}
