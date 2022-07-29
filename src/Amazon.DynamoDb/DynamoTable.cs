using System.Linq;

using Amazon.Scheduling;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb;

using static Expression;

public class DynamoTable<T, TKey>
    where T : class
    where TKey : notnull
{
    private readonly string _tableName;
    private readonly DynamoDbClient _client;

    private static readonly DatasetInfo metadata = DatasetInfo.Get<T>();

    // TODO: Historgram of consumed capacity

    private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromMilliseconds(100),
        maxDelay     : TimeSpan.FromSeconds(3),
        maxRetries   : 5);

    public DynamoTable(string tableName, IAwsCredential credential)
        : this(tableName, new DynamoDbClient(AwsRegion.USEast1, credential))
    { }

    public DynamoTable(IAwsCredential credential)
        : this(metadata.Name, new DynamoDbClient(AwsRegion.USEast1, credential))
    { }

    public DynamoTable(DynamoDbClient client)
        : this(metadata.Name, client)
    { }

    public DynamoTable(string tableName, DynamoDbClient client)
    {
        ArgumentNullException.ThrowIfNull(tableName);
        ArgumentNullException.ThrowIfNull(client);

        _tableName = tableName;
        _client = client;

        // TODO:
        // [ ] validate the key properties
    }

    public IKeyInfo PrimaryKey => metadata.PrimaryKey;

    public async Task<bool> ExistsAsync(TKey keyValue)
    {
        var key = Key<T>.FromTuple(keyValue);

        var result = await FindAsync(new GetItemRequest(_tableName, key) {
            ConsistentRead = false,
            AttributesToGet = PrimaryKey.Names
        }).ConfigureAwait(false);

        return result is not null;
    }

    public Task<T?> FindAsync(TKey keyValue)
    {
        return FindAsync(Key<T>.FromTuple(keyValue), isConsistent: false);
    }

    public Task<T?> FindAsync(TKey keyValue, bool isConsistent)
    {
        return FindAsync(Key<T>.FromTuple(keyValue), isConsistent);
    }

    public Task<T?> FindAsync(Key<T> key)
    {
        return FindAsync(key, isConsistent: false);
    }

    public Task<T?> FindAsync(Key<T> key, bool isConsistent)
    {
        return FindAsync(new GetItemRequest(_tableName, key) {
            ConsistentRead = isConsistent
        });
    }

    internal async Task<T?> FindAsync(GetItemRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var retryCount = 0;
        Exception? lastException = null;

        do
        {
            if (retryCount > 0)
            {
                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            try
            {
                var result = await _client.GetItemAsync(request).ConfigureAwait(false);

                // TODO: NotFound Handling

                if (result.Item is null) return null;

                return result.Item.As<T>(metadata);
            }
            catch (DynamoDbException ex) when (ex.IsTransient)
            {
                lastException = ex;
            }

            retryCount++;
        }
        while (retryPolicy.ShouldRetry(retryCount));

        var key = string.Join(',', request.Key.Select(static k => k.Value));

        var errorMessage = $"Unrecoverable exception getting '{key}' from '{_tableName}'";

        throw new Exception(errorMessage, lastException);
    }

    public async Task<IReadOnlyList<T>> FindAllAsync(params Dictionary<string, DbValue>[] keys)
    {
        if (keys is null || keys.Length == 0)
        {
            return Array.Empty<T>();
        }

        var request = new BatchGetItemRequest(new TableKeys(_tableName, keys));

        var result = await _client.BatchGetItemAsync(request).ConfigureAwait(false);

        TableItemCollection r0 = result.Responses[0];

        var items = new T[r0.Count];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = r0[i].As<T>(metadata);
        }

        return items;
    }

    public Task<IReadOnlyList<T>> QueryAsync(params Expression[] expressions)
    {
        var e = new DynamoQueryExpression(PrimaryKey.Names, expressions);

        var query = new DynamoQuery
        {
            KeyConditionExpression = e.KeyExpression.Text,
            ExpressionAttributeNames = e.AttributeNames.Count > 0 ? e.AttributeNames : null,
            ExpressionAttributeValues = e.AttributeValues,
            FilterExpression = e.FilterExpression?.Text
        };

        return QueryAsync(query);
    }

    public async Task<IReadOnlyList<T>> QueryAsync(DynamoQuery query)
    {
        query.TableName = _tableName;

        var result = await _client.QueryAsync(query, retryPolicy).ConfigureAwait(false);

        return new QueryResult<T>(result);
    }

    public async Task<IReadOnlyList<T>> QueryAllAsync(DynamoQuery query)
    {
        query.TableName = _tableName;

        var remaining = query.Limit;

        var result = new List<T>();

        QueryResult a;

        do
        {
            a = await _client.QueryAsync(query, retryPolicy).ConfigureAwait(false);

            foreach (AttributeCollection item in a.Items)
            {
                result.Add(item.As<T>());
            }

            remaining -= a.Count;

            query.ExclusiveStartKey = a.LastEvaluatedKey;
            query.Limit = remaining;

        }
        while (a.LastEvaluatedKey is not null && remaining >= 0);

        return result;
    }

    public Task<int> CountAsync(params Expression[] conditions)
    {
        return CountAsync(new DynamoQuery(conditions));
    }

    public async Task<int> CountAsync(DynamoQuery query)
    {
        query.TableName = _tableName;
        query.Select = SelectEnum.COUNT;

        var result = await _client.QueryCountAsync(query).ConfigureAwait(false);

        return result.Count;
    }

    public async IAsyncEnumerable<T> ScanAsync(params Expression[] conditions) // Scans the entire table
    {
        DynamoExpression? filterExpression = null;

        if (conditions is { Length: > 0 })
        {
            filterExpression = DynamoExpression.Conjunction(conditions);
        }

        QueryResult? result = null;

        do
        {
            var request = new ScanRequest(_tableName) {
                ExclusiveStartKey = result?.LastEvaluatedKey
            };

            if (filterExpression is not null)
            {
                request.SetFilterExpression(filterExpression);
            }

            result = await _client.ScanAsync(request).ConfigureAwait(false);

            // If LastEvaluatedKey is null, then the "last page" of results has been processed and there is no more data to be retrieved.
            // If LastEvaluatedKey is anything other than null, this does not necessarily mean that there is more data in the result set. 
            // The only way to know when you have reached the end of the result set is when LastEvaluatedKey is null.

            foreach (AttributeCollection item in result.Items)
            {
                yield return item.As<T>(metadata);
            }
        }
        while (result.LastEvaluatedKey is not null);
    }

    public async Task<IReadOnlyList<T>> ScanAsync(
        IEnumerable<KeyValuePair<string, object>>? startKey = null,
        Expression[]? conditions = null,
        int take = 1000)
    {
        var request = new ScanRequest(_tableName) {
            Limit = take
        };

        if (conditions is { Length: > 0 })
        {
            request.SetFilterExpression(DynamoExpression.Conjunction(conditions));
        }

        if (startKey is not null)
        {
            request.ExclusiveStartKey = startKey.ToDictionary();
        }

        var result = await _client.ScanAsync(request).ConfigureAwait(false);

        return new QueryResult<T>(result);
    }

    // A condition put (only if exists)
    public Task<PutItemResult> CreateAsync(T entity)
    {
        return PutAsync(entity, NotExists(PrimaryKey.Names[0]));
    }

    public Task<PutItemResult> PutAsync(T entity)
    {
        var request = new PutItemRequest(_tableName, AttributeCollection.FromObject(entity, metadata));

        return _client.PutItemUsingRetryPolicyAsync(request, retryPolicy);
    }

    // Conditional put
    public Task<PutItemResult> PutAsync(T entity, params Expression[] conditions)
    {
        var request = new PutItemRequest(_tableName, AttributeCollection.FromObject(entity, metadata));

        if (conditions is not null)
        {
            request.SetConditions(conditions);
        }

        return _client.PutItemUsingRetryPolicyAsync(request, retryPolicy);
    }

    const int maxBatchSize = 25;

    public async Task<BatchResult> PutAsync(IEnumerable<T> entities)
    {
        var result = new BatchResult();

        foreach (T[] batch in entities.Chunk(maxBatchSize))
        {
            await PutBatch(batch, result).ConfigureAwait(false);
        }

        return result;
    }

    public async Task<UpdateItemResult> PatchAsync(TKey key, params Change[] changes)
    {
        var request = new UpdateItemRequest(_tableName, Key<T>.FromTuple(key).ToDictionary(), changes);

        return await _client.UpdateItemUsingRetryPolicyAsync(request, retryPolicy).ConfigureAwait(false);
    }

    public Task<UpdateItemResult> PatchAsync(
        TKey key,
        Change[] changes,
        Expression[] conditions,
        ReturnValues? returnValues = null)
    {
        var request = new UpdateItemRequest(_tableName, Key<T>.FromTuple(key).ToDictionary(), changes, conditions, returnValues);

        return _client.UpdateItemUsingRetryPolicyAsync(request, retryPolicy);
    }

    public Task<UpdateItemResult> PatchAsync(TKey key, Change[] changes, ReturnValues returnValues)
    {
        var request = new UpdateItemRequest(_tableName, Key<T>.FromTuple(key).ToDictionary(), changes, returnValues: returnValues);

        return _client.UpdateItemUsingRetryPolicyAsync(request, retryPolicy);
    }

    public Task<DeleteItemResult> DeleteAsync(T record)
    {
        var request = new DeleteItemRequest(_tableName, Key<T>.FromObject(record));

        return InternalDeleteAsync(request);
    }

    public Task<DeleteItemResult> DeleteAsync(TKey key)
    {
        var request = new DeleteItemRequest(_tableName, key: Key<T>.FromTuple(key));

        return InternalDeleteAsync(request);
    }

    public Task<DeleteItemResult> DeleteAsync(Key<T> key)
    {
        var request = new DeleteItemRequest(_tableName, key);

        return InternalDeleteAsync(request);
    }

    public Task<DeleteItemResult> DeleteAsync(Key<T> key, ReturnValues returnValues)
    {
        var request = new DeleteItemRequest(_tableName, key)
        {
            ReturnValues = returnValues
        };

        return InternalDeleteAsync(request);
    }

    public Task<DeleteItemResult> DeleteAsync(Key<T> key, params Expression[] conditions)
    {
        return DeleteAsync(key, conditions, ReturnValues.NONE);
    }

    public Task<DeleteItemResult> DeleteAsync(Key<T> key, Expression[] conditions, ReturnValues returnValues)
    {
        var request = new DeleteItemRequest(_tableName, key)
        {
            ReturnValues = returnValues
        };

        if (conditions.Length > 0)
        {
            request.SetConditions(conditions);
        }

        return InternalDeleteAsync(request);
    }

    private async Task<DeleteItemResult> InternalDeleteAsync(DeleteItemRequest request)
    {
        var retryCount = 0;
        Exception lastError;

        // TODO: Move retry logic to client...

        do
        {
            if (retryCount > 0)
            {
                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            try
            {
                return await _client.DeleteItemAsync(request).ConfigureAwait(false);
            }
            catch (DynamoDbException ex) when (ex.IsTransient)
            {
                lastError = ex;
            }

            retryCount++;
        }
        while (retryPolicy.ShouldRetry(retryCount));

        throw lastError;
    }

    #region Helpers

    private async Task<BatchResult> PutBatch(IEnumerable<T> entities, BatchResult result)
    {
        // Up to 25 items put or delete operations, with the request size not exceeding 1 MB.

        var putRequests = entities
            .Select(e => (ItemRequest)new PutRequest(AttributeCollection.FromObject(e, metadata)))
            .ToList();

        var tableBatch = new TableRequests(_tableName, putRequests);

        await BatchWriteItem(tableBatch, result).ConfigureAwait(false);

        result.BatchCount++;

        return result;
    }

    private async Task<BatchWriteItemResult> BatchWriteItem(TableRequests batch, BatchResult info)
    {
        if (batch.Requests.Count > 25)
            throw new ArgumentException($"Batch must not exceed 25 items. Contained {batch.Requests.Count} items.", nameof(batch));

        int retryCount = 0;

        Exception? lastError;

        do
        {
            if (retryCount > 0)
            {
                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            try
            {
                var result = await _client.BatchWriteItemAsync(batch).ConfigureAwait(false);

                info.ItemCount += batch.Requests.Count;
                info.RequestCount++;

                // Recursively process any item
                foreach (var unprocessedBatch in result.UnprocessedItems)
                {
                    await Task.Delay(Random.Shared.Next(250, 500)).ConfigureAwait(false); // Slow down

                    info.ItemCount -= unprocessedBatch.Requests.Count;

                    result = await BatchWriteItem(unprocessedBatch, info).ConfigureAwait(false);
                }

                return result;
            }
            catch (DynamoDbException ex) when (ex.IsTransient)
            {
                lastError = ex;
            }

            retryCount++;

        } while (retryPolicy.ShouldRetry(retryCount));

        throw lastError;
    }

    #endregion
}
