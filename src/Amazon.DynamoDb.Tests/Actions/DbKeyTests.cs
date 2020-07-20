namespace Amazon.DynamoDb
{
	using Xunit;

	public class DbKeyTests
	{
		[Fact]
		public void KeyFromDbItem()
		{
			var item = new AttributeCollection {
				{ "a", new DbValue(123) },
				{ "b", new DbValue("abc") }
			};

			var key = item.ToKey();

			Assert.Equal("a",   key[0].Key);
			Assert.Equal("b",   key[1].Key);
			Assert.Equal(123L,  key[0].Value);
			Assert.Equal("abc", key[1].Value);
		}

		[Fact]
		public void ReservedKeywords()
        {
			Assert.True(DynamoKeyword.IsReserved("TTL"));
			Assert.True(DynamoKeyword.IsReserved("ttl"));
			Assert.False(DynamoKeyword.IsReserved("notakeyword"));
        }
	}
}
