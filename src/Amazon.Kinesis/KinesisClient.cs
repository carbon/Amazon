using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Kinesis
{
    public sealed class KinesisClient : AwsClient
    {
        private const string TargetPrefix = "Kinesis_" + Version;
        private const string Version = "20131202";

        public KinesisClient(IAwsCredential credential)
            : base(AwsService.Kinesis, AwsRegion.USEast1, credential)
        { }

        public KinesisStream GetStream(string name)
        {
            return new KinesisStream(name, this);
        }

        public Task<KinesisResponse> MergeShardsAsync(MergeShardsRequest request)
        {
            return SendAsync<KinesisResponse>("MergeShards", request);
        }
        public Task<PutRecordResult> PutRecordAsync(Record record)
        {
            return SendAsync<PutRecordResult>("PutRecord", record);
        }

        public Task<PutRecordsResult> PutRecordsAsync(string streamName, Record[] records)
        {
            var request = new PutRecordsRequest(streamName, records);

            // TODO: retry failures?

            return SendAsync<PutRecordsResult>("PutRecords", request);
        }

        public Task<DescribeStreamResult> DescribeStreamAsync(DescribeStreamRequest request)
        {
            return SendAsync<DescribeStreamResult>("DescribeStream", request);
        }

        public Task<GetShardIteratorResponse> GetShardIteratorAsync(GetShardIteratorRequest request)
        {
            return SendAsync<GetShardIteratorResponse>("GetShardIterator", request);
        }

        public Task<GetRecordsResponse> GetRecordsAsync(GetRecordsRequest request)
        {
            return SendAsync<GetRecordsResponse>("GetRecords", request);
        }

        #region Helpers

        private async Task<T> SendAsync<T>(string action, KinesisRequest request)
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

            var error = result.As<ErrorResult>();

            error.Text = responseText;

            return new KinesisException(error) {
                StatusCode = response.StatusCode
            };
        }

        private HttpRequestMessage GetRequestMessage(string action, KinesisRequest request)
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