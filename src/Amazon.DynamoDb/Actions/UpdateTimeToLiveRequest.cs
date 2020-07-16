using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class UpdateTimeToLiveRequest
    {
        public UpdateTimeToLiveRequest(string tableName, string attributeName, bool enabled)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            TimeToLiveSpecification = new TimeToLiveSpecification(attributeName, enabled);
        }

        public string TableName { get; }
        public TimeToLiveSpecification TimeToLiveSpecification { get; }
    }
}

/*
{
   "TableName": "string",
   "TimeToLiveSpecification": { 
      "AttributeName": "string",
      "Enabled": boolean
   }
}
*/
