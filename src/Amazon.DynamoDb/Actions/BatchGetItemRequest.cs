using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Amazon.DynamoDb.JsonConverters;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(BatchGetItemRequestConverter))]
    public sealed class BatchGetItemRequest
    {
        public BatchGetItemRequest(params TableKeys[] sets)
        {
            Sets = sets ?? throw new ArgumentNullException(nameof(sets));
        }

        public TableKeys[] Sets { get; }
    }

    public sealed class TableKeys
    {
        public TableKeys(string tableName, params Dictionary<string, DbValue>[] keys)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Keys = keys;
        }

        public TableKeys(string tableName, params IEnumerable<KeyValuePair<string, object>>[] keys)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Keys = new Dictionary<string, DbValue>[keys.Length];

            for (int i = 0; i < Keys.Length; i++)
            {
                Keys[i] = keys[i].ToDictionary();
            }
        }

        public string TableName { get; }

        public Dictionary<string, DbValue>[] Keys { get; }

        public string[]? AttributesToGet { get; set; }

        public bool ConsistentRead { get; set; }
    }
}
