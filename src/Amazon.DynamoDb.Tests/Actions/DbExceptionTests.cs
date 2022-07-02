namespace Amazon.DynamoDb.Tests;

public class DbExceptionTests
{
    /*
    [Fact]
    public async Task DynamoParseException()
    {
        var text = """{"__type":"com.amazon.coral.service#SerializationException","Message":"Start of list found where not expected"}""";

        var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));

        var ex = await DynamoDbException.FromResponseAsync(ms);

        Assert.Equal("SerializationException", ex.Type);
        Assert.Equal("Start of list found where not expected", ex.Message);
    }

    [Fact]
    public async Task DynamoParseException_Lowercase()
    {
        var text = """{"__type":"Exception","message":"Something went wrong"}""";

        var ms = new MemoryStream(Encoding.UTF8.GetBytes(text));

        var ex = await DynamoDbException.FromResponseAsync(ms);

        Assert.Equal("Exception", ex.Type);
        Assert.Equal("Something went wrong", ex.Message);
    }
    */
}
