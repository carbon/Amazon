namespace Amazon.DynamoDb;

public class DynamoQueryTests
{
    // [Fact]
    public void A()
    {
        var query = new DynamoQuery
        {
            TableName = "Posts",
            ConsistentRead = true,
            ProjectionExpression = "a > 0",
            Select = SelectEnum.ALL_ATTRIBUTES
        };
    }
}