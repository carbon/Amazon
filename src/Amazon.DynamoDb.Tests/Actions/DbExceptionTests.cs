namespace Amazon.DynamoDb.Tests
{
	using Xunit;

	public class DbExceptionTests
	{
		[Fact]
		public void DynamoParseException()
		{
			var text = @"{""__type"":""com.amazon.coral.service#SerializationException"",""Message"":""Start of list found where not expected""}";

			var ex = DynamoDbException.Parse(text);

			Assert.Equal("SerializationException", ex.Type);
			Assert.Equal("Start of list found where not expected", ex.Message);
		}
	}
}
