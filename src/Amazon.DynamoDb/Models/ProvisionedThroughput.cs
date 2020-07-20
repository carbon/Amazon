namespace Amazon.DynamoDb
{
    public sealed class ProvisionedThroughput
    {
        public ProvisionedThroughput() { }

        public ProvisionedThroughput(int readCapacityUnits, int writeCapacityUnits)
        {
            ReadCapacityUnits = readCapacityUnits;
            WriteCapacityUnits = writeCapacityUnits;
        }

        public int ReadCapacityUnits { get; set; }

        public int WriteCapacityUnits { get; set; }
    }
}
