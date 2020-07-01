using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Scheduling;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class DynamoDbClient : AwsClient
    {
        private const string TargetPrefix = "DynamoDB_20120810";

        public DynamoDbClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.DynamoDb, region, credential)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        #region Helpers

        public DynamoTable<T, TKey> GetTable<T, TKey>(string name)
            where T : class
        {
            return new DynamoTable<T, TKey>(name, this);
        }

        public DynamoTable<T, TKey> GetTable<T, TKey>()
            where T : class
        {
            return new DynamoTable<T, TKey>(this);
        }

        #endregion

        public async Task<BatchGetItemResult> BatchGetItemAsync(BatchGetItemRequest request)
        {
            var httpRequest = Setup("BatchGetItem", request.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return BatchGetItemResult.FromJson(responseJson);
        }

        /*
        public void CreateTable()
        {
            throw new NotImplementedException();
        }
        */

        public async Task<DeleteItemResult> DeleteItemAsync(DeleteItemRequest request)
        {
            var httpRequest = Setup("DeleteItem", request.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return DeleteItemResult.FromJson(responseJson);
        }

        /*
        public void DeleteTable()
        {
            throw new NotImplementedException();
        }

        public void DescribeTable()
        {
            throw new NotImplementedException();
        }
        */

        public async Task<GetItemResult> GetItemAsync(GetItemRequest request)
        {
            var httpRequest = Setup("GetItem", request.ToJson());

            var json = await SendAndReadJsonAsync(httpRequest).ConfigureAwait(false);

            return GetItemResult.FromJson(json);
        }

        public Task ListTables()
        {
            throw new NotImplementedException();
        }

        public async Task<BatchWriteItemResult> BatchWriteItemAsync(params TableRequests[] batches)
        {
            /*
			RequestItems {
				"TableName1" :  [ Request, Request, ... ],
				"TableName2" :  [ Request, Request, ... ],
				 ...
			}
			*/

            var requestJson = new JsonObject {
                { "RequestItems", new JsonObject(batches.Select(b => b.ToJson())) }
            };

            var httpRequest = Setup("BatchWriteItem", requestJson);

            var json = await SendAndReadJsonAsync(httpRequest).ConfigureAwait(false);

            return BatchWriteItemResult.FromJson(json);
        }

        public async Task<PutItemResult> PutItemAsync(PutItemRequest request)
        {
            var httpRequest = Setup("PutItem", request.ToJson());

            var json = await SendAndReadJsonAsync(httpRequest).ConfigureAwait(false);

            return PutItemResult.FromJson(json);
        }

        public async Task<PutItemResult> PutItemUsingRetryPolicyAsync(PutItemRequest request, RetryPolicy retryPolicy)
        {
            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                try
                {
                    return await PutItemAsync(request).ConfigureAwait(false);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public async Task<QueryResult> QueryAsync(DynamoQuery query, RetryPolicy retryPolicy)
        {
            var retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                try
                {
                    return await QueryAsync(query).ConfigureAwait(false);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw new DynamoDbException($"Error querying '{query.TableName}': {lastException.Message}", lastException);
        }

        public async Task<QueryResult> QueryAsync(DynamoQuery query)
        {
            var httpRequest = Setup("Query", query.ToJson());

            var json = await SendAndReadJsonAsync(httpRequest).ConfigureAwait(false);

            return QueryResult.FromJson(json);
        }

        public async Task<CountResult> QueryCountAsync(DynamoQuery query)
        {
            query.Select = SelectEnum.COUNT;

            var httpRequest = Setup("Query", query.ToJson());

            string responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return System.Text.Json.JsonSerializer.Deserialize<CountResult>(responseText);
        }

        public async Task<QueryResult> ScanAsync(ScanRequest request)
        {
            var httpRequest = Setup("Scan", request.ToJson());

            var json = await SendAndReadJsonAsync(httpRequest).ConfigureAwait(false);

            return QueryResult.FromJson(json);
        }

        public async Task<UpdateItemResult> UpdateItemAsync(UpdateItemRequest request)
        {
            var httpRequest = Setup("UpdateItem", request.ToJson());

            var json = await SendAndReadJsonAsync(httpRequest).ConfigureAwait(false);

            return UpdateItemResult.FromJson(json);
        }

        public async Task<UpdateItemResult> UpdateItemUsingRetryPolicyAsync(UpdateItemRequest request, RetryPolicy retryPolicy)
        {
            var retryCount = 0;

            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }
                try
                {
                    return await UpdateItemAsync(request).ConfigureAwait(false);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;
            } while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public Task UpdateTable()
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private async Task<JsonObject> SendAndReadJsonAsync(HttpRequestMessage request)
        {
            await SignAsync(request).ConfigureAwait(false);

            using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw await GetExceptionAsync(response).ConfigureAwait(false);
            }

            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return await System.Text.Json.JsonSerializer.DeserializeAsync<JsonObject>(stream).ConfigureAwait(false);
        }

        private HttpRequestMessage Setup(string action, JsonObject jsonContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Headers = {
                    { "Accept-Encoding", "gzip" },
                    { "x-amz-target", TargetPrefix + "." + action }
                }
            };

            if (jsonContent != null)
            {
                byte[] utf8Json = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(jsonContent);

                request.Content = new ByteArrayContent(utf8Json) {
                    Headers = { { "Content-Type", "application/x-amz-json-1.0" } }
                };
            }

            return request;
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var ex = DynamoDbException.Parse(responseText);

            ex.StatusCode = (int)response.StatusCode;

            return ex;
        }

        #endregion
    }
}