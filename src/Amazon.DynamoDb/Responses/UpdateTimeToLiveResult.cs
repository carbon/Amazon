using Amazon.DynamoDb.Models;
using Carbon.Json;
using System;

namespace Amazon.DynamoDb
{
    public class UpdateTimeToLiveResult
    {
        public UpdateTimeToLiveResult(TimeToLiveSpecification timeToLiveDescription)
        {
            TimeToLiveDescription = timeToLiveDescription;
        }

        public TimeToLiveSpecification TimeToLiveDescription { get; set; }

        public static UpdateTimeToLiveResult FromJson(JsonObject json)
        {
            if (json.TryGetValue("TimeToLiveSpecification", out var ttlNode))
            {
                return new UpdateTimeToLiveResult(TimeToLiveSpecification.FromJson(ttlNode));
            }

            throw new ArgumentException("UpdateTimeToLiveResult must contain TimeToLiveSpecification");
        }
    }
}
