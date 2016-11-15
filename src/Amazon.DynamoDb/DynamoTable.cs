using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Helpers;

using Carbon.Data;

namespace Amazon.DynamoDb
{    
    using Scheduling;

    using static Expression;

    public class DynamoTable<T>
        where T : class
    {
        private readonly string tableName;
        private readonly DynamoDbClient client;

        private static readonly DatasetInfo metadata = DatasetInfo.Get<T>();

        // TODO: Historgram of consumed capasity

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
            initialDelay : TimeSpan.FromMilliseconds(100),
            maxDelay     : TimeSpan.FromSeconds(3),
            maxRetries   : 5);

        public DynamoTable(string tableName, IAwsCredentials credentials)
            : this(tableName, new DynamoDbClient(credentials))
        { }

        public DynamoTable(IAwsCredentials credentials)
            : this(metadata.Name, new DynamoDbClient(credentials))
        { }

        public DynamoTable(DynamoDbClient client)
            : this(metadata.Name, client)
        { }

        public DynamoTable(string tableName, DynamoDbClient client)
        {
            #region Preconditions

            if (tableName == null) throw new ArgumentNullException(nameof(tableName));
            if (client == null) throw new ArgumentNullException(nameof(client));

            #endregion

            this.tableName = tableName;
            this.client = client;

            // TODO: Validate the key properties
        }

        public IKeyInfo PrimaryKey 
            => metadata.PrimaryKey;

        public RecordKey GetKey(params object[] keyValues) 
            => PrimaryKey.WithValues(keyValues);

        public async Task<bool> ExistsAsync(RecordKey key)
        {
            var result = await FindAsync(new GetItemRequest(tableName, key) {
                ConsistentRead = false,
                ReturnConsumedCapacity = false,
                AttributesToGet = key.Select(k => k.Key).ToArray()
            }).ConfigureAwait(false);

            return result != null;
        }

        public Task<T> FindAsync(RecordKey key)
            => FindAsync(key, consistent: false);

        public Task<T> FindAsync(object hash)
            => FindAsync(GetKey(hash), consistent: false);

        public Task<T> FindAsync(object hash, object range)
            => FindAsync(GetKey(hash, range), consistent: false);

        public Task<T> FindAsync(RecordKey key, bool consistent)
            => FindAsync(new GetItemRequest(tableName, key) {
                ConsistentRead = true,
                ReturnConsumedCapacity = false
            });

        internal async Task<T> FindAsync(GetItemRequest request)
        {
            var retryCount = 0;
            Exception lastException = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    var result = await client.GetItem(request).ConfigureAwait(false);

                    // TODO: NotFound Handling

                    if (result.Item == null) return null;

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

        public async Task<IList<T>> FindAllAsync(params RecordKey[] keys)
        {
            #region Preconditions

            if (keys == null) throw new ArgumentNullException(nameof(keys));

            if (keys.Length == 0) throw new ArgumentException("Must not be 0", paramName: "keys.Length");

            #endregion

            var request = new BatchGetItemRequest(new TableKeys(tableName, keys));

            var result = await client.BatchGetItem(request).ConfigureAwait(false);

            return result.Responses[0].Select(i => i.As<T>(metadata)).ToArray();
        }

        public Task<IList<T>> QueryAsync(params Expression[] expressions)
            => QueryAsync(new Query(expressions));

        public Task<IList<T>> QueryAsync(Query q)
        {
            var e = new DynamoQueryExpression(PrimaryKey.Names, q.Expressions);

            var query = new DynamoQuery {
                IndexName                 = q.Index?.Name,
                Limit                     = q.Take ?? 0,
                KeyConditionExpression    = e.KeyExpression.Text,
                ExpressionAttributeNames  = (e.HasAttributeNames) ? e.AttributeNames : null,
                ExpressionAttributeValues = e.AttributeValues,
                FilterExpression          = e.FilterExpression?.Text
            };

            if (q.Orders != null && q.Orders[0].IsDescending)
            {
                query.ScanIndexForward = false;
            }

            return QueryAsync(query);
        }

        public async Task<IList<T>> QueryAsync(DynamoQuery query)
        {
            query.TableName = tableName;

            var result = await client.QueryAsync(query, retryPolicy).ConfigureAwait(false);

            return new QueryResult<T>(result);
        }

        public async Task<IList<T>> QueryAllAsync(DynamoQuery query)
        {
            query.TableName = tableName;

            var remaining = query.Limit;

            var result = new List<T>();

            QueryResult a;

            do
            {
                a = await client.QueryAsync(query, retryPolicy).ConfigureAwait(false);

                foreach (var item in a.Items)
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
            => CountAsync(new DynamoQuery(conditions));

        public async Task<int> CountAsync(DynamoQuery query)
        {
            query.TableName = tableName;
            query.Select = SelectEnum.COUNT;

            var result = await client.QueryCount(query).ConfigureAwait(false);

            return result.Count;
        }

        public IEnumerable<T> Enumerate(params Expression[] conditions) // Scans the entire table
        {
            // Each scan may return upto 1MB of data
            // TODO, consider parellel scans

            DynamoExpression filterExpression = null;

            if (conditions.Length > 0)
            {
                filterExpression = DynamoExpression.Conjunction(conditions);
            }

            var result = new QueryResult();

            do
            {
                var request = new ScanRequest(tableName)
                {
                    Limit = 1000,
                    ExclusiveStartKey = result.LastEvaluatedKey
                };

                if (filterExpression != null)
                {
                    request.SetFilterExpression(filterExpression);
                }

                result = client.Scan(request).Result;

                // If LastEvaluatedKey is null, then the "last page" of results has been processed and there is no more data to be retrieved.
                // If LastEvaluatedKey is anything other than null, this does not necessarily mean that there is more data in the result set. 
                // The only way to know when you have reached the end of the result set is when LastEvaluatedKey is null.

                foreach (var item in result.Items)
                {
                    yield return item.As<T>(metadata);
                }
            }
            while (result.LastEvaluatedKey != null);
        }

        public async Task<IList<T>> ScanAsync(RecordKey? startKey = null,
            Expression[] conditions = null,
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
                request.ExclusiveStartKey = AttributeCollection.FromJson(startKey.Value.ToJson());
            }

            var result = await client.Scan(request).ConfigureAwait(false);

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

            return client.PutItemWithRetryPolicy(request, retryPolicy);
        }

        // Conditional put
        public Task<PutItemResult> PutAsync(T entity, params Expression[] conditions)
        {
            var request = new PutItemRequest(tableName, AttributeCollection.FromObject(entity, metadata));

            if (conditions != null)
            {
                request.SetConditions(conditions);
            }

            return client.PutItemWithRetryPolicy(request, retryPolicy);
        }

        public async Task<BatchResult> PutAsync(IEnumerable<T> entities)
        {
            #region Preconditions

            if (entities == null) throw new ArgumentNullException(nameof(entities));

            #endregion

            var sw = Stopwatch.StartNew();

            var result = new BatchResult();

            // Batch in groups of 25
            foreach (var batch in entities.Batch(25))
            {
                await PutBatch(batch, result).ConfigureAwait(false);
            }

            result.ResponseTime = sw.Elapsed;

            return result;
        }

        public async Task<UpdateItemResult> PatchAsync(RecordKey key, params Change[] changes)
        {
            #region Preconditions

            if (changes == null)
                throw new ArgumentNullException(nameof(changes));

            #endregion

            var request = new UpdateItemRequest(tableName, key, changes);

            return await client.UpdateItemWithRetryPolicy(request, retryPolicy).ConfigureAwait(false);
        }

        // Conditional patch
        public Task<UpdateItemResult> PatchAsync(RecordKey key,
            IList<Change> changes,
            Expression[] conditions,
            ReturnValues? returnValues = null)
        {
            #region Preconditions

            if (changes == null)
                throw new ArgumentNullException(nameof(changes));

            if (conditions == null)
                throw new ArgumentNullException(nameof(conditions));

            #endregion

            var request = new UpdateItemRequest(tableName, key, changes) {
                ReturnValues = returnValues
            };

            if (conditions.Length > 0)
            {
                request.SetConditions(conditions);
            }

            return client.UpdateItemWithRetryPolicy(request, retryPolicy);
        }

        public Task<UpdateItemResult> PatchAsync(RecordKey key, IList<Change> changes, ReturnValues returnValues)
        {
            var request = new UpdateItemRequest(tableName, key, changes)  {
                ReturnValues = returnValues
            };

            return client.UpdateItemWithRetryPolicy(request, retryPolicy);
        }

        public Task<DeleteItemResult> DeleteAsync(RecordKey key, ReturnValues returnValues)
            => DeleteAsync(key, null, returnValues);

        public Task<DeleteItemResult> DeleteAsync(RecordKey key, params Expression[] conditions)
            => DeleteAsync(key, conditions, ReturnValues.NONE);

        public async Task<DeleteItemResult> DeleteAsync(RecordKey key, Expression[] conditions, ReturnValues returnValues)
        {
            var request = new DeleteItemRequest(tableName, key);

            if (conditions != null && conditions.Length > 0)
            {
                request.SetConditions(conditions);
            }

            if (returnValues != ReturnValues.NONE)
            {
                request.ReturnValues = returnValues;
            }

            var retryCount = 0;
            Exception lastError = null;

            do
            {
                try
                {
                    return await client.DeleteItem(request).ConfigureAwait(false);
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
            #region Preconditions

            if (entities == null) throw new ArgumentNullException(nameof(entities));

            #endregion

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
            #region Preconditions

            if (batch == null) throw new ArgumentNullException(nameof(batch));

            if (batch.Requests.Count > 25) throw new ArgumentException("Must be 25 or less.", "batch.Items");

            #endregion

            var retryCount = 0;
            Exception lastError = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    var result = await client.BatchWriteItem(batch).ConfigureAwait(false);

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

    public struct BatchResult
    {
        public TimeSpan ResponseTime { get; set; }

        public int BatchCount { get; set; }

        public int ItemCount { get; set; }

        public int ErrorCount { get; set; }

        public int RequestCount { get; set; }
    }
}