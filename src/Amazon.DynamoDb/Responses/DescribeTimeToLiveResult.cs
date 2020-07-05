using Amazon.DynamoDb.Models;
using Carbon.Json;
using System;

namespace Amazon.DynamoDb
{
    public class DescribeTimeToLiveResult
    {
        public DescribeTimeToLiveResult(TimeToLiveDescription timeToLiveDescription)
        {
            TimeToLiveDescription = timeToLiveDescription;
        }

        public TimeToLiveDescription TimeToLiveDescription { get; set; }

        public static DescribeTimeToLiveResult FromJson(JsonObject json)
        {
            if (json.TryGetValue("TimeToLiveDescription", out var ttlNode))
            {
                return new DescribeTimeToLiveResult(ttlNode.As<TimeToLiveDescription>());
            }

            throw new ArgumentException("DescribeTimeToLiveResult must contain TimeToLiveDescription");
        }
    }
}
