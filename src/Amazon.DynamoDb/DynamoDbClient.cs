using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Amazon.DynamoDb.Transactions;
using Amazon.Scheduling;

namespace Amazon.DynamoDb;

public sealed class DynamoDbClient : AwsClient
{
    private const string TargetPrefix = "DynamoDB_20120810";

    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

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
        var httpRequest = Setup("BatchGetItem", JsonSerializer.SerializeToUtf8Bytes(request));

        var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

        return BatchGetItemResult.FromJsonElement(json);
    }

    public async Task<DeleteItemResult> DeleteItemAsync(DeleteItemRequest request)
    {
        return await HandleRequestAsync<DeleteItemRequest, DeleteItemResult>("DeleteItem", request).ConfigureAwait(false);
    }

    public async Task<GetItemResult> GetItemAsync(GetItemRequest request)
    {
        return await HandleRequestAsync<GetItemRequest, GetItemResult>("GetItem", request).ConfigureAwait(false);
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

        var httpRequest = Setup("BatchWriteItem", JsonSerializer.SerializeToUtf8Bytes(new
        {
            RequestItems = tableBatches
        }));

        var json = await SendAndReadJsonElementAsync(httpRequest).ConfigureAwait(false);

        return BatchWriteItemResult.FromJsonElement(json);
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

    public async Task<QueryResult> QueryAsync(DynamoQuery query)
    {
        return await HandleRequestAsync<DynamoQuery, QueryResult>("Query", query).ConfigureAwait(false);
    }

    public async Task<CountResult> QueryCountAsync(DynamoQuery query)
    {
        query.Select = SelectEnum.COUNT;

        return await HandleRequestAsync<DynamoQuery, CountResult>("Query", query).ConfigureAwait(false);
    }

    public async Task<QueryResult> ScanAsync(ScanRequest request)
    {
        return await HandleRequestAsync<ScanRequest, QueryResult>("Scan", request).ConfigureAwait(false);
    }

    public async Task<TransactGetItemsResult> TransactGetItemsAsync(TransactGetItemRequest request)
    {
        return await HandleRequestAsync<TransactGetItemRequest, TransactGetItemsResult>("TransactGetItems", request).ConfigureAwait(false);
    }

    public async Task<TransactWriteItemsResult> TransactWriteItemsAsync(TransactWriteItemsRequest request)
    {
        return await HandleRequestAsync<TransactWriteItemsRequest, TransactWriteItemsResult>("TransactWriteItems", request).ConfigureAwait(false);
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

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
    private async ValueTask<JsonElement> SendAndReadJsonElementAsync(HttpRequestMessage request)
    {
        await SignAsync(request).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await DynamoDbException.FromResponseAsync(response).ConfigureAwait(false);
        }

        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<JsonElement>(stream).ConfigureAwait(false);
    }

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
    private async ValueTask<TResult> HandleRequestAsync<TRequest, TResult>(string action, TRequest request)
        where TResult : notnull
    {
        var httpRequest = Setup(action, JsonSerializer.SerializeToUtf8Bytes(request));

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await DynamoDbException.FromResponseAsync(response).ConfigureAwait(false);
        }

        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        var result = await JsonSerializer.DeserializeAsync<TResult>(stream, serializerOptions).ConfigureAwait(false);

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
