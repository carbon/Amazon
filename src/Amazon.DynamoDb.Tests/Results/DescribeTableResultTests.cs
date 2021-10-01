using System;
using System.Text.Json;

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb.Tests
{
    public class DescribeTableResultTests
    {
        [Fact]
        public void Parse()
        {
            string text = @"
{
    ""Table"": {
        ""TableArn"": ""arn:aws:dynamodb:us-west-2:123456789012:table/Thread"",
        ""AttributeDefinitions"": [
            {
                ""AttributeName"": ""ForumName"",
                ""AttributeType"": ""S""
            },
            {
                ""AttributeName"": ""LastPostDateTime"",
                ""AttributeType"": ""S""
            },
            {
                ""AttributeName"": ""Subject"",
                ""AttributeType"": ""S""
            }
        ],
        ""CreationDateTime"": 1.363729002358E9,
        ""ItemCount"": 0,
        ""KeySchema"": [
            {
                ""AttributeName"": ""ForumName"",
                ""KeyType"": ""HASH""
            },
            {
                ""AttributeName"": ""Subject"",
                ""KeyType"": ""RANGE""
            }
        ],
        ""LocalSecondaryIndexes"": [
            {
                ""IndexArn"": ""arn:aws:dynamodb:us-west-2:123456789012:table/Thread/index/LastPostIndex"",
                ""IndexName"": ""LastPostIndex"",
                ""IndexSizeBytes"": 0,
                ""ItemCount"": 0,
                ""KeySchema"": [
                    {
                        ""AttributeName"": ""ForumName"",
                        ""KeyType"": ""HASH""
                    },
                    {
                        ""AttributeName"": ""LastPostDateTime"",
                        ""KeyType"": ""RANGE""
                    }
                ],
                ""Projection"": {
                    ""ProjectionType"": ""KEYS_ONLY""
                }
            }
        ],
        ""ProvisionedThroughput"": {
            ""NumberOfDecreasesToday"": 0,
            ""ReadCapacityUnits"": 5,
            ""WriteCapacityUnits"": 5
        },
        ""TableName"": ""Thread"",
        ""TableSizeBytes"": 0,
        ""TableStatus"": ""ACTIVE""
    }
}

";

            var result = JsonSerializer.Deserialize<DescribeTableResult>(text);

            var table = result.Table;
            var keySchema = table.KeySchema;

            Assert.Equal("arn:aws:dynamodb:us-west-2:123456789012:table/Thread", table.TableArn);

            Assert.Equal("ForumName", keySchema[0].AttributeName);
            Assert.Equal(KeyType.HASH, keySchema[0].KeyType);

            Assert.Equal("Subject", keySchema[1].AttributeName);
            Assert.Equal(KeyType.RANGE, keySchema[1].KeyType);

            Assert.Single(table.LocalSecondaryIndexes);

            Assert.Equal("LastPostIndex", table.LocalSecondaryIndexes![0].IndexName);

            Assert.Equal(0, table.ProvisionedThroughput.NumberOfDecreasesToday);
            Assert.Equal(5, table.ProvisionedThroughput.ReadCapacityUnits);
            Assert.Equal(5, table.ProvisionedThroughput.WriteCapacityUnits);

            Assert.Equal(TableStatus.ACTIVE, table.TableStatus);
            Assert.Equal(0, table.TableSizeBytes);
            Assert.Equal("Thread", table.TableName);

            Assert.Equal("2013-03-19T21:36:42.3580000Z", ((DateTimeOffset)table.CreationDateTime).UtcDateTime.ToString("o"));
        }
    }
}