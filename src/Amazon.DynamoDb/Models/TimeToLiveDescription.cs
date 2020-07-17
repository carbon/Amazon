namespace Amazon.DynamoDb
{
    public sealed class TimeToLiveDescription
    {
        public string? AttributeName { get; set; }

        public TimeToLiveStatus TimeToLiveStatus { get; set; }
    }
}