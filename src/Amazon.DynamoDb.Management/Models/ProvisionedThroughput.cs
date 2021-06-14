namespace Amazon.DynamoDb.Models
{
    public sealed class ProvisionedThroughput
    {
        public ProvisionedThroughput() { }

        public ProvisionedThroughput(int readCapacityUnits, int writeCapacityUnits)
        {
            ReadCapacityUnits = readCapacityUnits;
            WriteCapacityUnits = writeCapacityUnits;
        }

        public int NumberOfDecreasesToday { get; init; }

        public int ReadCapacityUnits { get; init; }

        public int WriteCapacityUnits { get; init; }
    }
}