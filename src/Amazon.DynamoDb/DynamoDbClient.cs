using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    using Scheduling;

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

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return GetItemResult.FromJson(responseJson);
        }

        public Task ListTables()
        {
            throw new NotImplementedException();
        }

        public async Task<BatchWriteItemResult> BatchWriteItemAsync(params TableRequests[] batches)
        {
            #region Preconditions

            if (batches == null) throw new ArgumentNullException(nameof(batches));

            #endregion

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

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return BatchWriteItemResult.FromJson(responseJson);
        }

        public async Task<PutItemResult> PutItemAsync(PutItemRequest request)
        {
            var httpRequest = Setup("PutItem", request.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return PutItemResult.FromJson(responseJson);
        }


        public async Task<PutItemResult> PutItemUsingRetryPolicyAsync(PutItemRequest request, RetryPolicy retryPolicy)
        {
            var retryCount = 0;
            Exception lastError = null;

            do
            {
                try
                {
                    return await PutItemAsync(request).ConfigureAwait(false);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastError = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastError;
        }

        public async Task<QueryResult> QueryAsync(DynamoQuery query, RetryPolicy retryPolicy)
        {
            var retryCount = 0;
            Exception lastError = null;

            do
            {
                try
                {
                    return await QueryAsync(query).ConfigureAwait(false);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastError = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw new DynamoDbException($"Error querying '{query.TableName}': {lastError.Message}", lastError);

        }

        public async Task<QueryResult> QueryAsync(DynamoQuery query)
        {
            #region Preconditions

            if (query == null) throw new ArgumentNullException(nameof(query));

            #endregion

            var httpRequest = Setup("Query", query.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return QueryResult.FromJson(responseJson);

        }

        public async Task<CountResult> QueryCountAsync(DynamoQuery query)
        {
            #region Preconditions

            if (query == null) throw new ArgumentNullException(nameof(query));

            #endregion

            query.Select = SelectEnum.COUNT;

            var httpRequest = Setup("Query", query.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return CountResult.FromJson(responseJson);
        }

        public async Task<QueryResult> ScanAsync(ScanRequest request)
        {
            var httpRequest = Setup("Scan", request.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return QueryResult.FromJson(responseJson);
        }

        public async Task<UpdateItemResult> UpdateItemAsync(UpdateItemRequest request)
        {
            var httpRequest = Setup("UpdateItem", request.ToJson());

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);
            var responseJson = JsonObject.Parse(responseText);

            return UpdateItemResult.FromJson(responseJson);
        }

        public async Task<UpdateItemResult> UpdateItemUsingRetryPolicyAsync(UpdateItemRequest request, RetryPolicy retryPolicy)
        {
            var retryCount = 0;
            Exception lastError = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    return await UpdateItemAsync(request).ConfigureAwait(false);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastError = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            throw lastError;
        }

        public Task UpdateTable()
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private HttpRequestMessage Setup(string action, JsonObject jsonContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Headers = {
                    { "Accept-Encoding", "gzip" },
                    { "x-amz-target", TargetPrefix + "." + action }
                }
            };

            if (jsonContent != null)
            {
                var postBody = jsonContent.ToString(pretty: false);

                request.Content = new StringContent(postBody, Encoding.UTF8, "application/x-amz-json-1.0");
            }

            return request;
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var ex = DynamoDbException.FromJson(JsonObject.Parse(responseText));

            ex.StatusCode = (int)response.StatusCode;

            return ex;
        }

        #endregion
    }
}