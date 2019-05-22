#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Scheduling;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    using static Expression;

    public class DynamoTable<T, TKey>
        where T: class
    {
        private readonly string tableName;
        private readonly DynamoDbClient client;

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
            this.tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            this.client    = client    ?? throw new ArgumentNullException(nameof(client));

            // TODO: Validate the key properties
        }

        public IKeyInfo PrimaryKey => metadata.PrimaryKey;

        public async Task<bool> ExistsAsync(TKey keyValue)
        {
            var key = Key<T>.FromTuple(keyValue);

            var result = await FindAsync(new GetItemRequest(tableName, key) {
                ConsistentRead = false,
                ReturnConsumedCapacity = false,
                AttributesToGet = key.Select(k => k.Key).ToArray()
            }).ConfigureAwait(false);

            return result != null;
        }

        public Task<T?> FindAsync(Key<T> key)
        {
            return FindAsync(key, isConsistent: false);
        }

        public Task<T?> FindAsync(TKey keyValue)
        {
            return FindAsync(Key<T>.FromTuple(keyValue), isConsistent: false);
        }

        public Task<T?> FindAsync(Key<T> key, bool isConsistent) => FindAsync(new GetItemRequest(tableName, key) {
            ConsistentRead = isConsistent,
            ReturnConsumedCapacity = false
        });

        internal async Task<T?> FindAsync(GetItemRequest request)
        {
            var retryCount = 0;
            Exception? lastException = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    var result = await client.GetItemAsync(request).ConfigureAwait(false);

                    // TODO: NotFound Handling

                    if (result.Item is null) return null;

                    return result.Item.As<T>(metadata);
                }
                catch (DynamoDbException ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            var key = string.Join(",", request.Key.Select(k => k.Value));

            var errorMessage = $"Unrecoverable exception getting '{key}' from '{tableName}'";

            throw new Exception(errorMessage, lastException);
        }

        public async Task<IReadOnlyList<T>> FindAllAsync(params IEnumerable<KeyValuePair<string, object>>[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return Array.Empty<T>();
            }

            var request = new BatchGetItemRequest(new TableKeys(tableName, keys));

            var result = await client.BatchGetItemAsync(request).ConfigureAwait(false);

            return result.Responses[0].Select(i => i.As<T>(metadata)).ToArray();
        }

        public Task<IReadOnlyList<T>> QueryAsync(params Expression[] expressions)
        { 
            var e = new DynamoQueryExpression(PrimaryKey.Names, expressions);

            var query = new DynamoQuery {
                KeyConditionExpression    = e.KeyExpression.Text,
                ExpressionAttributeNames  = (e.HasAttributeNames) ? e.AttributeNames : null,
                ExpressionAttributeValues = e.AttributeValues,
                FilterExpression          = e.FilterExpression?.Text
            };
            
            return QueryAsync(query);
        }

        public async Task<IReadOnlyList<T>> QueryAsync(DynamoQuery query)
        {
            query.TableName = tableName;

            var result = await client.QueryAsync(query, retryPolicy).ConfigureAwait(false);

            return new QueryResult<T>(result);
        }

        public async Task<IReadOnlyList<T>> QueryAllAsync(DynamoQuery query)
        {
            query.TableName = tableName;

            var remaining = query.Limit;

            var result = new List<T>();

            QueryResult a;

            do
            {
                a = await client.QueryAsync(query, retryPolicy).ConfigureAwait(false);

                foreach (AttributeCollection item in a.Items)
                {
                    result.Add(item.As<T>());
                }

                remaining -= a.Count;

                query.ExclusiveStartKey = a.LastEvaluatedKey;
                query.Limit = remaining;

            }
            while (a.LastEvaluatedKey != null && remaining >= 0);

            return result;
        }

        public Task<int> CountAsync(params Expression[] conditions)
        {
            return CountAsync(new DynamoQuery(conditions));
        }

        public async Task<int> CountAsync(DynamoQuery query)
        {
            query.TableName = tableName;
            query.Select = SelectEnum.COUNT;

            var result = await client.QueryCountAsync(query).ConfigureAwait(false);

            return result.Count;
        }

        public IEnumerable<T> Enumerate(params Expression[] conditions) // Scans the entire table
        {
            // Each scan may return upto 1MB of data
            // TODO, consider parellel scans

            DynamoExpression? filterExpression = null;

            if (conditions.Length > 0)
            {
                filterExpression = DynamoExpression.Conjunction(conditions);
            }

            QueryResult? result = null;

            do
            {
                var request = new ScanRequest(tableName) {
                    Limit = 1000,
                    ExclusiveStartKey = result?.LastEvaluatedKey
                };

                if (filterExpression != null)
                {
                    request.SetFilterExpression(filterExpression);
                }

                result = client.ScanAsync(request).Result;

                // If LastEvaluatedKey is null, then the "last page" of results has been processed and there is no more data to be retrieved.
                // If LastEvaluatedKey is anything other than null, this does not necessarily mean that there is more data in the result set. 
                // The only way to know when you have reached the end of the result set is when LastEvaluatedKey is null.

                foreach (AttributeCollection item in result.Items)
                {
                    yield return item.As<T>(metadata);
                }
            }
            while (result.LastEvaluatedKey != null);
        }

        public async Task<IReadOnlyList<T>> ScanAsync(
            IEnumerable<KeyValuePair<string, object>>? startKey = null,
            Expression[]? conditions = null,
            int take = 1000)
        {
            var request = new ScanRequest(tableName)
            {
                Limit = take
            };

            if (conditions != null && conditions.Length > 0)
            {
                request.SetFilterExpression(DynamoExpression.Conjunction(conditions));
            }

            if (startKey != null)
            {
                request.ExclusiveStartKey = AttributeCollection.FromJson(startKey.ToJson());
            }

            var result = await client.ScanAsync(request).ConfigureAwait(false);

            return new QueryResult<T>(result);
        }

        // A condition put (only if exists)
        public Task<PutItemResult> CreateAsync(T entity)
        {
            return PutAsync(entity, NotExists(PrimaryKey.Names[0]));
        }

        public Task<PutItemResult> PutAsync(T entity)
        {
            var request = new PutItemRequest(tableName, AttributeCollection.FromObject(entity, metadata));

            return client.PutItemUsingRetryPolicyAsync(request, retryPolicy);
        }

        // Conditional put
        public Task<PutItemResult> PutAsync(T entity, params Expression[] conditions)
        {
            var request = new PutItemRequest(tableName, AttributeCollection.FromObject(entity, metadata));

            if (conditions != null)
            {
                request.SetConditions(conditions);
            }

            return client.PutItemUsingRetryPolicyAsync(request, retryPolicy);
        }

        public async Task<BatchResult> PutAsync(IEnumerable<T> entities)
        {
            var sw = Stopwatch.StartNew();

            var result = new BatchResult();

            // Batch in groups of 25
            foreach (IEnumerable<T> batch in entities.Batch(25))
            {
                await PutBatch(batch, result).ConfigureAwait(false);
            }

            result.ResponseTime = sw.Elapsed;

            return result;
        }

        public async Task<UpdateItemResult> PatchAsync(Key<T> key, params Change[] changes)
        {
            var request = new UpdateItemRequest(tableName, key, changes);

            return await client.UpdateItemUsingRetryPolicyAsync(request, retryPolicy).ConfigureAwait(false);
        }

        // Conditional patch
        public Task<UpdateItemResult> PatchAsync(
            Key<T> key,
            Change[] changes,
            Expression[] conditions,
            ReturnValues? returnValues = null)
        {
            var request = new UpdateItemRequest(tableName, key, changes, conditions, returnValues);

            return client.UpdateItemUsingRetryPolicyAsync(request, retryPolicy);
        }

        public Task<UpdateItemResult> PatchAsync(Key<T> key, Change[] changes, ReturnValues returnValues)
        {
            var request = new UpdateItemRequest(tableName, key, changes, returnValues: returnValues);

            return client.UpdateItemUsingRetryPolicyAsync(request, retryPolicy);
        }

        public Task<DeleteItemResult> DeleteAsync(T record)
        {
            if (record is null)
                throw new ArgumentNullException(nameof(record));

            var request = new DeleteItemRequest(
                tableName : tableName,
                key       : Key<T>.FromObject(record)
            );

            return InternalDelete(request);
        }

        public Task<DeleteItemResult> DeleteAsync(TKey key)
        {
            var request = new DeleteItemRequest(tableName, key: Key<T>.FromTuple(key));

            return InternalDelete(request);
        }

        public Task<DeleteItemResult> DeleteAsync(Key<T> key)
        {
            var request = new DeleteItemRequest(tableName, key);

            return InternalDelete(request);
        }

        public Task<DeleteItemResult> DeleteAsync(Key<T> key, ReturnValues returnValues)
        {
            var request = new DeleteItemRequest(tableName, key) {
                ReturnValues = returnValues
            };
       
            return InternalDelete(request);
        }

        public Task<DeleteItemResult> DeleteAsync(Key<T> key, params Expression[] conditions) => 
            DeleteAsync(key, conditions, ReturnValues.NONE);

        public Task<DeleteItemResult> DeleteAsync(Key<T> key, Expression[] conditions, ReturnValues returnValues)
        {
            var request = new DeleteItemRequest(tableName, key) {
                ReturnValues = returnValues
            };

            if (conditions.Length > 0)
            {
                request.SetConditions(conditions);
            }
        
            return InternalDelete(request);
        }

        private async Task<DeleteItemResult> InternalDelete(DeleteItemRequest request)
        {
            var retryCount = 0;
            Exception lastError;

            // TODO: Move retry logic to client...

            do
            {
                try
                {
                    return await client.DeleteItemAsync(request).ConfigureAwait(false);
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

        #region Helpers

        private async Task<BatchResult> PutBatch(IEnumerable<T> entities, BatchResult result)
        {
            // Up to 25 items put or delete operations, with the request size not exceeding 1 MB.

            var putRequests = entities
                .Select(e => (ItemRequest)new PutRequest(AttributeCollection.FromObject(e, metadata)))
                .ToList();

            var tableBatch = new TableRequests(tableName, putRequests);

            await BatchWriteItem(tableBatch, result).ConfigureAwait(false);

            result.BatchCount++;

            return result;
        }

        private async Task<BatchWriteItemResult> BatchWriteItem(TableRequests batch, BatchResult info)
        {
            if (batch.Requests.Count > 25)
                throw new ArgumentException("Must be 25 or less.", "batch.Items");

            var retryCount = 0;
            Exception? lastError = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    var result = await client.BatchWriteItemAsync(batch).ConfigureAwait(false);

                    info.ItemCount += batch.Requests.Count;
                    info.RequestCount++;

                    // Recursively process any item
                    foreach (var unprocessedBatch in result.UnprocessedItems)
                    {
                        await Task.Delay(100).ConfigureAwait(false); // Slow down

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

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            throw lastError;
        }

        #endregion
    }
}