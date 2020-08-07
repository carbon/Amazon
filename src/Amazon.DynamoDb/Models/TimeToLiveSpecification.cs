namespace Amazon.DynamoDb
{
    public sealed class TimeToLiveSpecification
    {
#nullable disable
        public TimeToLiveSpecification() { }
#nullable enable

        public TimeToLiveSpecification(string attributeName, bool enabled)
        {
            AttributeName = attributeName;
            Enabled = enabled;
        }

        public string AttributeName { get; set; }

        public bool Enabled { get; set; }
    }
}