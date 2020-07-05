using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class ListTablesRequest
    {
        public string? ExclusiveStartTableName { get; set; }
        public int? Limit { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject();

            if (ExclusiveStartTableName != null) json.Add("ExclusiveStartTableName", ExclusiveStartTableName);
            if (Limit.HasValue)                  json.Add("Limit", Limit.Value);

            return json;
        }
    }
}

/*
{
   "TableName": "string"
}
*/
