namespace Amazon.DynamoDb
{
    public class Conflict : DynamoDbException
    {
        public Conflict(string message)
            : base(message)
        {
            Type = "ConditionalCheckFailedException";
        }
    }
}