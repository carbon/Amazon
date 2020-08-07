using System.Linq;

using Carbon.Data;

using Xunit;

namespace Amazon.DynamoDb.Tests
{
    public class BatchGetItemRequestTests
    {
        [Fact]
        public void A()
        {
            var request = new BatchGetItemRequest(
                new TableKeys("Table1", new[] {
                    new RecordKey("id", 1).ToDictionary(x => x.Key, y => new DbValue(y.Value)),
                    new RecordKey("id", 2).ToDictionary(x => x.Key, y => new DbValue(y.Value))
                }),
                new TableKeys("Table2", new[] {
                    new RecordKey("id", 3).ToDictionary(x => x.Key, y => new DbValue(y.Value)),
                    new RecordKey("id", 4).ToDictionary(x => x.Key, y => new DbValue(y.Value))
                })
            );

            var expected = @"{""RequestItems"":{""Table1"":{""Keys"":[{""id"":{""N"":""1""}},{""id"":{""N"":""2""}}]},""Table2"":{""Keys"":[{""id"":{""N"":""3""}},{""id"":{""N"":""4""}}]}}}";

            Assert.Equal(expected, request.ToSystemTextJson());
        }

        [Fact]
        public void B()
        {
            var request = new BatchGetItemRequest(
                new TableKeys("Table1", new RecordKey("id", 1), new RecordKey("id", 2)),
                new TableKeys("Table2", new RecordKey("id", 3), new RecordKey("id", 4))
            );

            var expected = @"{""RequestItems"":{""Table1"":{""Keys"":[{""id"":{""N"":""1""}},{""id"":{""N"":""2""}}]},""Table2"":{""Keys"":[{""id"":{""N"":""3""}},{""id"":{""N"":""4""}}]}}}";

            Assert.Equal(expected, request.ToSystemTextJson());
        }
    }
}