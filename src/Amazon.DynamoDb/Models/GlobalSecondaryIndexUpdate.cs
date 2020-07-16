using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class GlobalSecondaryIndexUpdate
    {
        public GlobalSecondaryIndexUpdate()
        {
        }

        public CreateGlobalSecondaryIndexAction? Create { get; set; }
        public DeleteGlobalSecondaryIndexAction? Delete { get; set; }
        public UpdateGlobalSecondaryIndexAction? Update { get; set; }
    }
}
