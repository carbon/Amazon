using Amazon.DynamoDb.Models;
using Carbon.Json;
using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb.Extensions
{
    public static class JsonListExtensions
    {
        public static JsonNode ToJson(this IEnumerable<KeySchemaElement> keySchema)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in keySchema)
            {
                jsonList.Add(item.ToJson());
            }
            return jsonList;
        }

        public static JsonNode ToJson(this IEnumerable<LocalSecondaryIndex> indexes)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in indexes)
            {
                jsonList.Add(item.ToJson());
            }
            return jsonList;
        }

        public static JsonNode ToJson(this IEnumerable<GlobalSecondaryIndex> indexes)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in indexes)
            {
                jsonList.Add(item.ToJson());
            }
            return jsonList;
        }

        public static JsonNode ToJson(this IEnumerable<GlobalSecondaryIndexUpdate> indexes)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in indexes)
            {
                jsonList.Add(item.ToJson());
            }
            return jsonList;
        }

        public static JsonNode ToJson(this IEnumerable<KeyValuePair<string, string>> tags)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in tags)
            {
                jsonList.Add(new JsonObject { { item.Key, item.Value } });
            }
            return jsonList;
        }

        public static JsonNode ToJson(this IEnumerable<ReplicaGlobalSecondaryIndex> indexes)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in indexes)
            {
                jsonList.Add(item.ToJson());
            }
            return jsonList;
        }

        public static JsonNode ToJson(this IEnumerable<ReplicationGroupUpdate> replicas)
        {
            var jsonList = new JsonNodeList();
            foreach (var item in replicas)
            {
                jsonList.Add(item.ToJson());
            }
            return jsonList;
        }
    }
}
