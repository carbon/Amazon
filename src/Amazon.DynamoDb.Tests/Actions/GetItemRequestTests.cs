using System.Collections.Generic;

using Amazon.DynamoDb.Models.Tests;

using Carbon.Data;

namespace Amazon.DynamoDb.Tests
{
    public class GetItemRequestTests
    {
        [Fact]
        public void A()
        {
            var key = RecordKey.Create<Fruit>("banana");

            var x2 = new GetItemRequest("Fruits", key);

            Assert.Equal(@"{
  ""TableName"": ""Fruits"",
  ""Key"": {
    ""name"": {
      ""S"": ""banana""
    }
  }
}", x2.ToSystemTextJsonIndented());
        }

        [Fact]
        public void B()
        {
            var key = new RecordKey(new KeyValuePair<string, object>[] {
                new ("primary", 1),
                new ("secondary", "2")
            });

            var x2 = new GetItemRequest("Products", key) {
                ConsistentRead = true,
                ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
            };

            Assert.Equal(@"{
  ""TableName"": ""Products"",
  ""Key"": {
    ""primary"": {
      ""N"": ""1""
    },
    ""secondary"": {
      ""S"": ""2""
    }
  },
  ""ConsistentRead"": true,
  ""ReturnConsumedCapacity"": ""TOTAL""
}", x2.ToSystemTextJsonIndented());
        }
    }

}
