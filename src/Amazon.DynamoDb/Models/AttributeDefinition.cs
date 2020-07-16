#nullable disable

using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class AttributeDefinition
    {
        public string AttributeName { get; set; }
        public AttributeType AttributeType { get; set; }
    }
}
