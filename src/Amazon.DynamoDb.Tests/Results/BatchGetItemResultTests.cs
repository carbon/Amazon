using System.Text.Json;

namespace Amazon.DynamoDb.Results.Tests;

public class BatchGetItemResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        using var doc = JsonDocument.Parse(
            """
            {
                "Responses": {
                    "Forum": [
                        {
                            "Name":{
                                "S":"Amazon DynamoDB"
                            },
                            "Threads":{
                                "N":"5"
                            },
                            "Messages":{
                                "N":"19"
                            },
                            "Views":{
                                "N":"35"
                            }
                        },
                        {
                            "Name":{
                                "S":"Amazon RDS"
                            },
                            "Threads":{
                                "N":"8"
                            },
                            "Messages":{
                                "N":"32"
                            },
                            "Views":{
                                "N":"38"
                            }
                        },
                        {
                            "Name":{
                                "S":"Amazon Redshift"
                            },
                            "Threads":{
                                "N":"12"
                            },
                            "Messages":{
                                "N":"55"
                            },
                            "Views":{
                                "N":"47"
                            }
                        }
                    ],
                    "Thread": [
                        {
                            "Tags":{
                                "SS":["Reads","MultipleUsers"]
                            },
                            "Message":{
                                "S":"How many users can read a single data item at a time? Are there any limits?"
                            }
                        }
                    ]
                },
                "UnprocessedKeys": {
                },
                "ConsumedCapacity": [
                    {
                        "TableName": "Forum",
                        "CapacityUnits": 3
                    },
                    {
                        "TableName": "Thread",
                        "CapacityUnits": 1
                    }
                ]
            }
            """);

        var result = BatchGetItemResult.FromJsonElement(doc.RootElement);

        Assert.Equal(2, result.Responses.Count);
        Assert.Equal(3, result.Responses[0].Count);

        Assert.Equal("Forum", result.Responses[0].Name);

        Assert.Equal("Amazon DynamoDB", result.Responses[0][0].GetString("Name"));
        Assert.Equal("Amazon RDS", result.Responses[0][1].GetString("Name"));

        Assert.Equal("Thread", result.Responses[1].Name);

        var thread_0 = result.Responses[1][0];

        Assert.Equal("How many users can read a single data item at a time? Are there any limits?", thread_0.GetString("Message"));

        Assert.Equal([ "Reads", "MultipleUsers" ], thread_0.Get("Tags").ToArray<string>());

        Assert.Equal(2, result.ConsumedCapacity.Length);
    }
}