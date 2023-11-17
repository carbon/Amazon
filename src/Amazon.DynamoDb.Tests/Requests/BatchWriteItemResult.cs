using System.Text.Json;

namespace Amazon.DynamoDb.Models.Tests;

public class DynamoBatchTests
{
    [Fact]
    public void BatchRequestTest1()
    {
        ItemRequest[] requests = [
            new PutRequest(new AttributeCollection {
                { "title", "awesomeness" },
                { "ownerId", 1 }
            }),
            new DeleteRequest(new AttributeCollection {
                { "title", "notawesome" },
                { "ownerId", 2 }
            })
        ];

        var tableBatch = new TableRequests("Posts", requests).SerializeList();

        Assert.Equal(
            """
            [
              {
                "PutRequest": {
                  "Item": {
                    "title": {
                      "S": "awesomeness"
                    },
                    "ownerId": {
                      "N": "1"
                    }
                  }
                }
              },
              {
                "DeleteRequest": {
                  "Key": {
                    "title": {
                      "S": "notawesome"
                    },
                    "ownerId": {
                      "N": "2"
                    }
                  }
                }
              }
            ]
            """, tableBatch.ToIndentedJsonString());
    }

    [Fact]
    public void BatchWriteRequest1()
    {
        using var doc = JsonDocument.Parse(
            """
            {
             "UnprocessedItems":  {
            	"Slugs": [ 
            		{ 
            			"PutRequest": { "Item": { "name": { "S": "apples" }, "ownerId": { "N": "1" }, "type": { "N": "1" } } } 
            		}, 
            		{
            			"PutRequest": { "Item": { "name": { "S": "bananas" }, "ownerId": { "N": "2" }, "type": { "N": "1" } } } 
            		},
            		{
            			"DeleteRequest": { "Key": { "name": { "S": "oranges" }, "ownerId": { "N": "3" } } } 
            		}
            	] 
              } 
            }
            """);

        var result = BatchWriteItemResult.Deserialize(doc.RootElement);

        Assert.Single(result.UnprocessedItems);
        Assert.Equal(3, result.UnprocessedItems[0].Requests.Count);

        Assert.Equal("Slugs", result.UnprocessedItems[0].TableName);

        var request0_0 = (PutRequest)result.UnprocessedItems[0].Requests[0];
        var request0_1 = (PutRequest)result.UnprocessedItems[0].Requests[1];
        var request0_2 = (DeleteRequest)result.UnprocessedItems[0].Requests[2];

        Assert.Equal("apples", request0_0.Item.GetString("name"));
        Assert.Equal(1, request0_0.Item.GetInt("ownerId"));

        Assert.Equal("bananas", request0_1.Item.GetString("name"));
        Assert.Equal(2, request0_1.Item.GetInt("ownerId"));

        Assert.Equal("oranges", request0_2.Key.GetString("name"));
        Assert.Equal(3, request0_2.Key.GetInt("ownerId"));
    }
}