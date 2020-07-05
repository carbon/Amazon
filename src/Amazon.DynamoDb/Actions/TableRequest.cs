using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal class TableRequest
    {
        internal TableRequest(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string TableName { get; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName },
            };

            return json;
        }
    }
}

/*
{
   "TableName": "string"
}
*/
