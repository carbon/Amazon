
using Amazon.DynamoDb.Models.Tests;
using Xunit;

namespace Amazon.DynamoDb.Tests
{
    public class PutItemRequestTests
    {
        [Fact]
        public void Construct()
        {
            var item = new PutItemRequest("Accounts", new AttributeCollection { { "id", 1 } });

            Assert.Equal("Accounts", item.TableName);
           
            Assert.Equal(@"{""TableName"":""Accounts"",""Item"":{""id"":{""N"":""1""}}}", item.ToSystemTextJson());
        }

        [Fact]
        public void A()
        {
            var item = new PutItemRequest("Accounts", new AttributeCollection { { "id", 1 } }) { ReturnValues = ReturnValues.UPDATED_NEW };

            Assert.Equal(@"{""TableName"":""Accounts"",""Item"":{""id"":{""N"":""1""}},""ReturnValues"":""UPDATED_NEW""}", item.ToSystemTextJson());
        }


        [Fact]
        public void B()
        {
            var item = new PutItemRequest("Accounts", new AttributeCollection { { "id", 1 } }) { ReturnValues = ReturnValues.UPDATED_OLD };

            Assert.Equal(@"{""TableName"":""Accounts"",""Item"":{""id"":{""N"":""1""}},""ReturnValues"":""UPDATED_OLD""}", item.ToSystemTextJson());
        }
    }

}
