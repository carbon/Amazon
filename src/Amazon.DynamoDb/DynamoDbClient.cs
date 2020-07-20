using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Scheduling;

namespace Amazon.DynamoDb
{
    public sealed class DynamoDbClient : AwsClient
    {
        private const string TargetPrefix = "DynamoDB_20120810";

        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions {
            Converters = {
                new JsonConverters.DateTimeOffsetConverter()
            },
            IgnoreNullValues = true,
        };

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

            var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

            return BatchGetItemResult.FromJsonElement(json);
        }

        public async Task<CreateTableResult> CreateTableAsync(CreateTableRequest request)
        {
            return await HandleRequestAsync<CreateTableRequest, CreateTableResult>("CreateTable", request);
        }

        public async Task<DeleteItemResult> DeleteItemAsync(DeleteItemRequest request)
        {
            return await HandleRequestAsync<DeleteItemRequest, DeleteItemResult>("DeleteItem", request);
        }

        public async Task<DeleteTableResult> DeleteTableAsync(string tableName)
        {
            return await HandleRequestAsync<TableRequest, DeleteTableResult>("DeleteTable", new TableRequest(tableName));
        }

        public async Task<DescribeTableResult> DescribeTableAsync(string tableName)
        {
            return await HandleRequestAsync<TableRequest, DescribeTableResult>("DescribeTable", new TableRequest(tableName));
        }

        public async Task<DescribeTimeToLiveResult> DescribeTimeToLiveAsync(string tableName)
        {
            return await HandleRequestAsync<TableRequest, DescribeTimeToLiveResult>("DescribeTimeToLive", new TableRequest(tableName));
        }

        public async Task<GetItemResult> GetItemAsync(GetItemRequest request)
        {
            var httpRequest = Setup("GetItem", request.ToJson());

            var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

            return GetItemResult.FromJsonElement(json);
        }

        public async Task<ListTablesResult> ListTablesAsync(ListTablesRequest request)
        {
            return await HandleRequestAsync<ListTablesRequest, ListTablesResult>("ListTables", request);
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

            var requestJson = new Carbon.Json.JsonObject {
                { "RequestItems", new Carbon.Json.JsonObject(batches.Select(b => b.ToJson())) }
            };

            var httpRequest = Setup("BatchWriteItem", requestJson);

            var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

            return BatchWriteItemResult.FromJsonElement(json);
        }

        public async Task<PutItemResult> PutItemAsync(PutItemRequest request)
        {
            return await HandleRequestAsync<PutItemRequest, PutItemResult>("PutItem", request);
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
            return await HandleRequestAsync<DynamoQuery, QueryResult>("Query", query);
        }

        public async Task<CountResult> QueryCountAsync(DynamoQuery query)
        {
            query.Select = SelectEnum.COUNT;

            return await HandleRequestAsync<DynamoQuery, CountResult>("Query", query);
        }

        public async Task<QueryResult> ScanAsync(ScanRequest request)
        {
            return await HandleRequestAsync<ScanRequest, QueryResult>("Scan", request);
        }

        public async Task<TransactGetItemsResult> TransactGetItems(TransactGetItemRequest request)
        {
            return await HandleRequestAsync<TransactGetItemRequest, TransactGetItemsResult>("TransactGetItems", request);
        }

        public async Task<TransactWriteItemsResult> TransactGetItems(TransactWriteItemsRequest request)
        {
            return await HandleRequestAsync<TransactWriteItemsRequest, TransactWriteItemsResult>("TransactWriteItems", request);
        }

        public async Task<UpdateItemResult> UpdateItemAsync(UpdateItemRequest request)
        {
            return await HandleRequestAsync<UpdateItemRequest, UpdateItemResult>("UpdateItem", request);
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

        public async Task<UpdateTableResult> UpdateTableAsync(UpdateTableRequest request)
        {
            return await HandleRequestAsync<UpdateTableRequest, UpdateTableResult>("UpdateTable", request);
        }

        public async Task<UpdateTimeToLiveResult> UpdateTimeToLiveAsync(UpdateTimeToLiveRequest request)
        {
            return await HandleRequestAsync<UpdateTimeToLiveRequest, UpdateTimeToLiveResult>("UpdateTimeToLive", request);
        }

        #region Helpers

        private async Task<TResult> HandleRequestAsync<TRequest, TResult>(string action, TRequest request)
        {
            var httpRequest = Setup(action, JsonSerializer.SerializeToUtf8Bytes(request));

            return await SendAndReadObjectAsync<TResult>(httpRequest).ConfigureAwait(false);
        }

        private async Task<JsonElement> SendAndReadJsonElementAsync(HttpRequestMessage request)
        {
            await SignAsync(request).ConfigureAwait(false);

            using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw await GetExceptionAsync(response).ConfigureAwait(false);
            }

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<JsonElement>(stream).ConfigureAwait(false);
        }

        private async Task<T> SendAndReadObjectAsync<T>(HttpRequestMessage request)
        {
            await SignAsync(request).ConfigureAwait(false);

            using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw await GetExceptionAsync(response).ConfigureAwait(false);
            }

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<T>(stream, serializerOptions).ConfigureAwait(false);
        }

        private HttpRequestMessage Setup(string action, Carbon.Json.JsonObject jsonContent)
        {
            if (jsonContent is null)
            {
                return Setup(action, (byte[]?)null);
            }

            return Setup(action, JsonSerializer.SerializeToUtf8Bytes(jsonContent));
        }

        private HttpRequestMessage Setup(string action, byte[]? utf8Json)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Headers = {
                    { "Accept-Encoding", "gzip" },
                    { "x-amz-target", TargetPrefix + "." + action }
                }
            };

            if (utf8Json != null)
            {
                request.Content = new ByteArrayContent(utf8Json) {
                    Headers = { { "Content-Type", "application/x-amz-json-1.0" } }
                };
            }

            return request;
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var ex = await DynamoDbException.DeserializeAsync(stream).ConfigureAwait(false);

            ex.StatusCode = (int)response.StatusCode;

            return ex;
        }

        #endregion
    }
}