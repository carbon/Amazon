using Amazon.DynamoDb.Models.Tests;
using Carbon.Data;

using Xunit;

namespace Amazon.DynamoDb.Tests
{
    public class ModelTests
    {
        [Fact]
        public void SerializeEnumTest()
        {
            var entity = new Thing { Color = Color.Red, Y = null };

            var record = AttributeCollection.FromObject(entity);

            Assert.Equal(2, record["color"].ToInt());

            Assert.Equal(Color.Red, record.As<Thing>().Color);
            Assert.Null(record.As<Thing>().Y);

            entity = new Thing { Color = Color.Red, Y = Color2.C };

            record = AttributeCollection.FromObject(entity);

            Assert.Equal(Color2.C, record.As<Thing>().Y);
        }

        [Fact]
        public void TestIntegerColumns()
        {
            var point = new Point { X = 1, Y = 2, Z = 3 };


            var x = AttributeCollection.FromObject(point);

            Assert.Equal(@"{
  ""1"": {
    ""N"": ""1""
  },
  ""2"": {
    ""N"": ""2""
  },
  ""3"": {
    ""N"": ""3""
  }
}", x.ToJson().ToString());
        }

        [Fact]
        public void TestColumns()
        {
            var orange = new Fruit {
                Name = "orange",
                Calories = 50,
                Description = "Amazing",
                IsHealthy = true
            };

            var x = AttributeCollection.FromObject(orange);

            Assert.Equal(@"{
  ""name"": {
    ""S"": ""orange""
  },
  ""calories"": {
    ""N"": ""50""
  },
  ""description"": {
    ""S"": ""Amazing""
  }
}", x.ToJson().ToString());
            
            var key = RecordKey.Create<Fruit>("banana");

            var x2 = new GetItemRequest("Fruits", key);

            Assert.Equal(@"{
  ""TableName"": ""Fruits"",
  ""Key"": {
    ""name"": {
      ""S"": ""banana""
    }
  }
}", x2.ToJson().ToString());

        }
    }
}
