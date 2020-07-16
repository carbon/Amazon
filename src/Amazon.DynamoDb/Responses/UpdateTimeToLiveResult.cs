#nullable disable

using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class UpdateTimeToLiveResult
    {
        public TimeToLiveSpecification TimeToLiveSpecification { get; set; }
    }
}
