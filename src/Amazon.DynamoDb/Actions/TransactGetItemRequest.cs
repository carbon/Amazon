using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class TransactGetItemRequest
    {
        public TransactGetItem[] TransactItems { get; set; }

        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

        public TransactGetItemRequest(TransactGetItem[] transactItems)
        {
            TransactItems = transactItems ?? throw new ArgumentNullException(nameof(transactItems));
        }
    }

    public class TransactGetItem
    {
        public Get Get { get; set; }

        public TransactGetItem(Get get)
        {
            Get = get ?? throw new ArgumentNullException(nameof(get));
        }
    }

    public class Get
    {
        public IReadOnlyDictionary<string, DbValue> Key { get; set; }

        public string TableName { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public string? ProjectionExpression { get; set; }

        public Get(string tableName, IReadOnlyDictionary<string, DbValue> key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
    }
}

//{
//   "ReturnConsumedCapacity": "string",
//   "TransactItems": [
//      { 
//         "Get": { 
//            "ExpressionAttributeNames": { 
//               "string" : "string" 
//            },
//            "Key": { 
//               "string" : { 
//                  "B": blob,
//                  "BOOL": boolean,
//                  "BS": [blob],
//                  "L": [
//                     "AttributeValue"
//                  ],
//                  "M": { 
//                     "string" : "AttributeValue"
//                  },
//                  "N": "string",
//                  "NS": [ "string" ],
//                  "NULL": boolean,
//                  "S": "string",
//                  "SS": [ "string" ]
//               }
//            },
//            "ProjectionExpression": "string",
//            "TableName": "string"
//         }
//      }
//   ]
//}