using Carbon.Data;
using Carbon.Json;
using Xunit;

namespace Amazon.DynamoDb.Models.Tests
{
    public class BatchTests
    {
        [Fact]
        public void BatchGetItemsResult1()
        {
            var text =
@"{
    ""Responses"": {
        ""Forum"": [
            {
                ""Name"":{
                    ""S"":""Amazon DynamoDB""
                },
                ""Threads"":{
                    ""N"":""5""
                },
                ""Messages"":{
                    ""N"":""19""
                },
                ""Views"":{
                    ""N"":""35""
                }
            },
            {
                ""Name"":{
                    ""S"":""Amazon RDS""
                },
                ""Threads"":{
                    ""N"":""8""
                },
                ""Messages"":{
                    ""N"":""32""
                },
                ""Views"":{
                    ""N"":""38""
                }
            },
            {
                ""Name"":{
                    ""S"":""Amazon Redshift""
                },
                ""Threads"":{
                    ""N"":""12""
                },
                ""Messages"":{
                    ""N"":""55""
                },
                ""Views"":{
                    ""N"":""47""
                }
            }
        ]
        ""Thread"": [
            {
                ""Tags"":{
                    ""SS"":[""Reads"",""MultipleUsers""]
                },
                ""Message"":{
                    ""S"":""How many users can read a single data item at a time? Are there any limits?""
                }
            }
        ]
    },
    ""UnprocessedKeys"": {
    },
    ""ConsumedCapacity"": [
        {
            ""TableName"": ""Forum"",
            ""CapacityUnits"": 3
        },
        {
            ""TableName"": ""Thread"",
            ""CapacityUnits"": 1
        }
    ]
}";

            var result = BatchGetItemResult.FromJson(JsonObject.Parse(text));

            Assert.Equal(2, result.Responses.Length);

            Assert.Equal(3, result.Responses[0].Count);

            Assert.Equal("Amazon DynamoDB", result.Responses[0][0].GetString("Name"));
            Assert.Equal("How many users can read a single data item at a time? Are there any limits?", result.Responses[1][0].GetString("Message"));

        }
    }
}