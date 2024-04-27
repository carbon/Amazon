using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

using Amazon.DynamoDb.Serialization;
using Amazon.DynamoDb.Transactions;
using Amazon.Scheduling;

namespace Amazon.DynamoDb;

public sealed class DynamoDbClient : AwsClient
{
    private const string TargetPrefix = "DynamoDB_20120810";

    public DynamoDbClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.DynamoDb, region, credential)
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    #region Helpers

    public DynamoTable<T, TKey> GetTable<T, TKey>(string name)
        where T : class
        where TKey : notnull
    {
        return new DynamoTable<T, TKey>(name, this);
    }

    public DynamoTable<T, TKey> GetTable<T, TKey>()
        where T : class
        where TKey : notnull
    {
        return new DynamoTable<T, TKey>(this);
    }

    #endregion

    public async Task<BatchGetItemResult> BatchGetItemAsync(BatchGetItemRequest request)
    {
        byte[] utf8JsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, DynamoDbSerializationContext.Default.BatchGetItemRequest);

        var httpRequest = Setup("BatchGetItem", utf8JsonBytes);

        var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

        return BatchGetItemResult.Deserialize(json);
    }

    public Task<DeleteItemResult> DeleteItemAsync(DeleteItemRequest request)
    {
        return HandleRequestAsync<DeleteItemRequest, DeleteItemResult>("DeleteItem", request);
    }

    public Task<GetItemResult> GetItemAsync(GetItemRequest request)
    {
        return HandleRequestAsync<GetItemRequest, GetItemResult>("GetItem", request);
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

        var tableBatches = new Dictionary<string, object>(batches.Length);

        foreach (var batch in batches)
        {
            tableBatches.Add(batch.TableName, batch.SerializeList());
        }

        var httpRequest = Setup("BatchWriteItem", JsonSerializer.SerializeToUtf8Bytes(new {
            RequestItems = tableBatches
        }));

        var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

        return BatchWriteItemResult.Deserialize(json);
    }

    public async Task<PutItemResult> PutItemAsync(PutItemRequest request)
    {
        return await HandleRequestAsync<PutItemRequest, PutItemResult>("PutItem", request).ConfigureAwait(false);
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

    public Task<QueryResult> QueryAsync(DynamoQuery query)
    {
        return HandleRequestAsync<DynamoQuery, QueryResult>("Query", query);
    }

    public Task<CountResult> QueryCountAsync(DynamoQuery query)
    {
        query.Select = SelectEnum.COUNT;

        return HandleRequestAsync<DynamoQuery, CountResult>("Query", query);
    }

    public Task<QueryResult> ScanAsync(ScanRequest request)
    {
        return HandleRequestAsync<ScanRequest, QueryResult>("Scan", request);
    }

    public Task<TransactGetItemsResult> TransactGetItemsAsync(TransactGetItemRequest request)
    {
        return HandleRequestAsync<TransactGetItemRequest, TransactGetItemsResult>("TransactGetItems", request);
    }

    public Task<TransactWriteItemsResult> TransactWriteItemsAsync(TransactWriteItemsRequest request)
    {
        return HandleRequestAsync<TransactWriteItemsRequest, TransactWriteItemsResult>("TransactWriteItems", request);
    }

    public async Task<UpdateItemResult> UpdateItemAsync(UpdateItemRequest request)
    {
        return await HandleRequestAsync<UpdateItemRequest, UpdateItemResult>("UpdateItem", request).ConfigureAwait(false);
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
        }
        while (retryPolicy.ShouldRetry(retryCount));

        throw lastException;
    }

    #region Helpers

    private async Task<JsonElement> SendAndReadJsonElementAsync(HttpRequestMessage request)
    {
        await SignAsync(request).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await DynamoDbException.FromResponseAsync(response).ConfigureAwait(false);
        }

        return await response.Content.ReadFromJsonAsync<JsonElement>(JsonSerializerOptions.Default).ConfigureAwait(false);
    }

    private async Task<TResult> HandleRequestAsync<TRequest, TResult>(string action, TRequest request)
        where TResult : notnull
    {
        var httpRequest = Setup(action, JsonSerializer.SerializeToUtf8Bytes(request));

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await DynamoDbException.FromResponseAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<TResult>(JsonSerializerOptions.Default).ConfigureAwait(false);

        return result!;
    }

    private HttpRequestMessage Setup(string action, byte[]? utf8Json)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Headers = {
                { "Accept-Encoding", "gzip" },
                { "x-amz-target", $"{TargetPrefix}.{action}" }
            }
        };

        if (utf8Json is not null)
        {
            request.Content = new ByteArrayContent(utf8Json) {
                Headers = { { "Content-Type", "application/x-amz-json-1.0" } }
            };
        }

        return request;
    }

    #endregion
}
