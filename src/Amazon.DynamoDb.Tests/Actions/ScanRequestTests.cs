using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Tests;

public class ScanRequestTests
{
    [Fact]
    public void Test1()
    {
        var request = new ScanRequest("Libraries", conditions: [Expression.Lt("id", 10)]);

        Assert.Equal("""
            {
              "TableName": "Libraries",
              "FilterExpression": "id \u003C :v0",
              "ExpressionAttributeValues": {
                ":v0": {
                  "N": "10"
                }
              }
            }
            """, request.ToIndentedJsonString(), ignoreLineEndingDifferences: true);
    }
}