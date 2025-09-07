using Amazon.DynamoDb.Models.Tests;

using Carbon.Data;

namespace Amazon.DynamoDb.Tests;

public class DeleteItemRequestTests
{
    [Fact]
    public void A()
    {
        var key = RecordKey.Create<Fruit>("banana");

        var x2 = new DeleteItemRequest("Fruits", key);

        Assert.Equal("""
            {
              "TableName": "Fruits",
              "Key": {
                "name": {
                  "S": "banana"
                }
              }
            }
            """, x2.ToIndentedJsonString(), ignoreLineEndingDifferences: true);
    }
}