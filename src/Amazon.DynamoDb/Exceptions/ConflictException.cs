namespace Amazon.DynamoDb
{
    public sealed class ConflictException : DynamoDbException
    {
        public ConflictException(string message)
            : base(message)
        {
            Type = "ConditionalCheckFailedException";
        }
    }
}