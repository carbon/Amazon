using Amazon.DynamoDb.Models.Tests;

using Carbon.Data.Expressions;

using Xunit;

namespace Amazon.DynamoDb.Tests
{
    public class QueryRequestTests
	{
        [Fact]
        public void QueryRequestTest1()
        {
            var request = new DynamoQuery(Expression.Eq("id", 1)) {
                TableName = "Libraries",
                Limit = 1
            };

            Assert.Equal(@"{
  ""TableName"": ""Libraries"",
  ""KeyConditionExpression"": ""id = :v0"",
  ""ExpressionAttributeValues"": {
    "":v0"": {
      ""N"": ""1""
    }
  },
  ""Limit"": 1
}", request.ToSystemTextJsonIndented());
        }

        [Fact]
        public void QueryRequestTest5()
        {
            var query = new DynamoQuery(Expression.Eq("userId", 1))
                .Filter(Expression.Contains("participantIds", Expression.Constant(2)));

            query.TableName = "Conversations";

            Assert.Equal(@"{
  ""TableName"": ""Conversations"",
  ""KeyConditionExpression"": ""userId = :v0"",
  ""ExpressionAttributeValues"": {
    "":v0"": {
      ""N"": ""1""
    },
    "":v1"": {
      ""N"": ""2""
    }
  },
  ""FilterExpression"": ""contains(participantIds, :v1)""
}", query.ToSystemTextJsonIndented());
        }

        // You can use any attribute name in a document path, provided that the first character is a-z or A-Z and the second character (if present) is a-z, A-Z, or 0-9. If an attribute name does not meet this requirement, you will need to define an expression attribute name as a placeholder. 

        /*
        [Fact]
        public void QueryTestWithNumber()
        {
            var query = new DynamoQuery(Expression.Eq("1", 1))
                .Filter(Expression.Contains("participantIds", Expression.Constant(2)));

            query.TableName = "Conversations";

            Assert.Equal(@"{
  ""TableName"": ""Conversations"",
  ""KeyConditionExpression"": ""userId = :v0"",
  ""ExpressionAttributeValues"": {
    "":v0"": {
      ""N"": ""1""
    },
    "":v1"": {
      ""N"": ""2""
    }
  },
  ""FilterExpression"": ""contains(participantIds, :v1)""
}", query.ToJson().ToString());



        }
        */


        [Fact]
        public void QueryRequestTest2()
        {
            var request = new DynamoQuery {
                TableName = "Libraries",
                KeyConditionExpression = "id = :v1",
                ExpressionAttributeValues = new AttributeCollection {
                    [":v1"] = new DbValue(1)
                },
                Limit = 1
            };


            Assert.Equal(@"{
  ""TableName"": ""Libraries"",
  ""KeyConditionExpression"": ""id = :v1"",
  ""ExpressionAttributeValues"": {
    "":v1"": {
      ""N"": ""1""
    }
  },
  ""Limit"": 1
}", request.ToSystemTextJsonIndented());
        }

        [Fact]
        public void QueryRequestTest4()
        {
            var request = new DynamoQuery
            {
                TableName = "Libraries",
                KeyConditionExpression = "id = :v1",
                ExpressionAttributeValues = new AttributeCollection
                {
                    [":v1"] = new DbValue(1)
                },
                Limit = 1
            }.Include("name", "version");


            Assert.Equal(@"{
  ""TableName"": ""Libraries"",
  ""KeyConditionExpression"": ""id = :v1"",
  ""ExpressionAttributeNames"": {
    ""#name"": ""name""
  },
  ""ExpressionAttributeValues"": {
    "":v1"": {
      ""N"": ""1""
    }
  },
  ""ProjectionExpression"": ""#name,version"",
  ""Limit"": 1
}", request.ToSystemTextJsonIndented());
        }

        [Fact]
        public void QueryRequestTest3()
        {
            var query = DynamoExpression.FromExpression(Expression.Eq("name", "sortable"));

            var request = new DynamoQuery {
                TableName = "Libraries",
                KeyConditionExpression = query.Text,
                ExpressionAttributeValues = query.AttributeValues,
                ExpressionAttributeNames = query.AttributeNames,
                Limit = 1
            };

            Assert.Equal(@"{
  ""TableName"": ""Libraries"",
  ""KeyConditionExpression"": ""#name = :v0"",
  ""ExpressionAttributeNames"": {
    ""#name"": ""name""
  },
  ""ExpressionAttributeValues"": {
    "":v0"": {
      ""S"": ""sortable""
    }
  },
  ""Limit"": 1
}", request.ToSystemTextJsonIndented());
        }
    }
}