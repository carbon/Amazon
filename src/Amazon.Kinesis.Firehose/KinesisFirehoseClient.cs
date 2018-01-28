using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Kinesis.Firehose
{
    public sealed class KinesisFirehoseClient : AwsClient
    {
        const string Version = "20150804";
        const string TargetPrefix = "Firehose_" + Version;

        public KinesisFirehoseClient(AwsRegion region, IAwsCredential credential)
            : base("firehose", region, credential) { }
        
        public DeliveryStream GetStream(string name)
        {
            return new DeliveryStream(name, this);
        }

        public Task<CreateDeliveryStreamResult> CreateDeliveryStreamAsync(CreateDeliveryStreamRequest request)
        {
            return SendAsync<CreateDeliveryStreamResult>("CreateDeliveryStream", request);
        }

        public Task<DeleteDeliveryStreamResult> DeleteDeliveryStreamAsync(DeleteDeliveryStreamRequest request)
        {
            return SendAsync<DeleteDeliveryStreamResult>("PutRecordBatch", request);
        }

        public Task<DescribeDeliveryStreamResult> DescribeDeliveryStreamAsync(DescribeDeliveryStreamRequest request)
        {
            return SendAsync<DescribeDeliveryStreamResult>("DescribeDeliveryStream", request);
        }

        public Task<ListDeliveryStreamsRequest> ListDeliveryStreamsAsync(ListDeliveryStreamsRequest request)
        {
            return SendAsync<ListDeliveryStreamsRequest>("ListDeliveryStreams", request);
        }

        public Task<PutRecordResult> PutRecordAsync(PutRecordRequest request)
        {
            return SendAsync<PutRecordResult>("PutRecord", request);
        }

        public Task<PutRecordBatchResult> PutRecordBatchAsync(PutRecordBatchRequest request)
        {
            return SendAsync<PutRecordBatchResult>("PutRecordBatch", request);
        }

        public void UpdateDestination(UpdateDestinationRequest request) { }

        #region Helpers

        private async Task<T> SendAsync<T>(string action, object request)
            where T : new()
        {
            var httpRequest = GetRequestMessage(action, request);

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return JsonObject.Parse(responseText).As<T>();
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = JsonObject.Parse(responseText);

            throw new Exception(responseText);
        }

        private HttpRequestMessage GetRequestMessage(string action, object request)
        {
            var json = (JsonObject)new JsonSerializer().Serialize(request,
                    new SerializationOptions(ingoreNullValues: true));

            var postBody = json.ToString(pretty: false);

            return new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Headers = {
                    { "x-amz-target", TargetPrefix  + "." + action }
                },
                Content = new StringContent(postBody, Encoding.UTF8, "application/x-amz-json-1.1")
            };
        }

        #endregion
    }
}
