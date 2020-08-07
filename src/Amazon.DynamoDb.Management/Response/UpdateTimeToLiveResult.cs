#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb
{
    public class UpdateTimeToLiveResult
    {
        public TimeToLiveSpecification TimeToLiveSpecification { get; set; }
    }
}