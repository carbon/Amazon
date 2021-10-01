using System;
using System.Collections.Generic;

using Carbon.Data;

namespace Amazon.DynamoDb.Tests
{
    public class UpdateExpressionTests
	{
        [Fact]
        public void UpdateTest1()
        {
            var locked = DateTimeOffset.FromUnixTimeSeconds(1497212690).UtcDateTime;

            var attributeNames = new Dictionary<string, string>();

            var values = new AttributeCollection();

            var expression = new UpdateExpression(new[] {
                Change.Replace("locked", locked)
            }, attributeNames, values);

            Assert.Equal(@"SET locked = :v0", expression.ToString());

            Assert.Equal(1497212690L, values.Get(":v0").Value);
        }

        [Fact]
        public void UpdateTest2()
        {
            var attrNames = new Dictionary<string, string>();
            var attrValues = new AttributeCollection();

            var expression = new UpdateExpression(new[]
            {
                Change.Add("list", new[] { "A", "B", "C" }),
                Change.Remove("deleted")
            }, attrNames, attrValues);

            Assert.Equal(@"REMOVE deleted
ADD #list :v0", expression.ToString());

            Assert.Single(attrNames);
            Assert.Equal("list", attrNames["#list"]);
        }

        [Fact]
        public void UpdateTest3()
        {
            var expression = new UpdateExpression(new[]
            {
                Change.Replace("deleted", new DateTime(2015, 01, 01)),
                Change.Replace("colors", new [] { "red", "yellow", "blue" }),
                Change.Replace("version", 1)
            }, new Dictionary<string, string>(), new AttributeCollection());

            Assert.Equal(@"SET deleted = :v0, colors = :v1, version = :v2", expression.ToString());
        }

        [Fact]
        public void UpdateTest4()
        {
            var expression = new UpdateExpression(new[]
            {
                Change.Replace("deleted", new DateTime(2015, 01, 01)),
                Change.Replace("colors", new [] { "red", "yellow", "blue" }),
                Change.Remove("deleted"),
                Change.Add("version", 1),
                Change.Replace("modified", DateTime.UtcNow)
            }, new Dictionary<string, string>(), new AttributeCollection());

            Assert.Equal(@"SET deleted = :v0, colors = :v1, modified = :v3
REMOVE deleted
ADD version :v2", expression.ToString());
        }
    }
}