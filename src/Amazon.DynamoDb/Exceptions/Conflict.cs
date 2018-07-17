namespace Amazon.DynamoDb
{
    public sealed class Conflict : DynamoDbException
    {
        public Conflict(string message)
            : base(message)
        {
            Type = "ConditionalCheckFailedException";
        }
    }
}