﻿namespace Amazon.DynamoDb
{
    public class DynamoKeywordTests
	{
		[Fact]
		public void ReservedKeywords()
		{
			Assert.True(DynamoKeyword.IsReserved("TTL"));
			Assert.True(DynamoKeyword.IsReserved("ttl"));
			Assert.False(DynamoKeyword.IsReserved("notakeyword"));
		}
	}
}