namespace Amazon.DynamoDb.Models
{
    public sealed class TimeToLiveDescription
    {
        public string? AttributeName { get; set; }

        public TimeToLiveStatus TimeToLiveStatus { get; set; }
    }
}