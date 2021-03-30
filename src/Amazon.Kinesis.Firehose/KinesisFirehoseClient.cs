using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
            where TResult : notnull, new()
        {
            var httpRequest = GetRequestMessage<TRequest>(action, request);

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return JsonSerializer.Deserialize<TResult>(responseText)!;
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // var result = JsonObject.Parse(responseText);

            throw new Exception(responseText);
        }

        private static readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions { IgnoreNullValues = true };

        private HttpRequestMessage GetRequestMessage<T>(string action, T request)
        {
            var json = JsonSerializer.Serialize(request, serializationOptions);

            return new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Headers = {
                    { "x-amz-target", TargetPrefix  + "." + action }
                },
                Content = new StringContent(json, Encoding.UTF8, "application/x-amz-json-1.1")
            };
        }

        #endregion
    }
}
