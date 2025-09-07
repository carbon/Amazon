using Amazon.DynamoDb.Models.Tests;

using Carbon.Data;

namespace Amazon.DynamoDb.Tests;

public class GetItemRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new GetItemRequest(
            tableName : "Fruits", 
            key       : RecordKey.Create<Fruit>("banana")
        );

        Assert.Equal(
            """
            {
              "TableName": "Fruits",
              "Key": {
                "name": {
                  "S": "banana"
                }
              }
            }
            """, request.ToIndentedJsonString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void CanSerializeComplex()
    {
        var key = new RecordKey([
            new ("primary", 1),
            new ("secondary", "2")
        ]);

        var request = new GetItemRequest("Products", key) {
            ConsistentRead = true,
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
        };

        Assert.Equal(
            """
            {
              "TableName": "Products",
              "Key": {
                "primary": {
                  "N": "1"
                },
                "secondary": {
                  "S": "2"
                }
              },
              "ConsistentRead": true,
              "ReturnConsumedCapacity": "TOTAL"
            }
            """, request.ToIndentedJsonString(), ignoreLineEndingDifferences: true);
    }
}