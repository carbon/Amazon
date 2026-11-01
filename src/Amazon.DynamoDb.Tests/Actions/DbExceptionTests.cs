using System.Net;
using System.Net.Http;
using System.Text;

namespace Amazon.DynamoDb.Tests;

public class DbExceptionTests
{
    private static Task<DynamoDbException> ParseAsync(string text, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return DynamoDbException.FromResponseAsync(new HttpResponseMessage(statusCode) {
            Content = new StringContent(text, Encoding.UTF8, "application/x-amz-json-1.0")
        });
    }

    [Fact]
    public async Task DynamoParseException()
    {
        var ex = await ParseAsync("""{"__type":"com.amazon.coral.service#SerializationException","Message":"Start of list found where not expected"}""");

        Assert.Equal("SerializationException", ex.Type);
        Assert.Equal("Start of list found where not expected", ex.Message);
        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.False(ex.IsTransient);
    }

    [Fact]
    public async Task DynamoParseException_Lowercase()
    {
        var ex = await ParseAsync("""{"__type":"Exception","message":"Something went wrong"}""");

        Assert.Equal("Exception", ex.Type);
        Assert.Equal("Something went wrong", ex.Message);
    }

    [Fact]
    public async Task ConditionalCheckFailureIsConflict()
    {
        var ex = await ParseAsync("""{"__type":"com.amazonaws.dynamodb.v20120810#ConditionalCheckFailedException","message":"Failed"}""");

        var conflict = Assert.IsType<ConflictException>(ex);

        Assert.Equal("ConditionalCheckFailedException", conflict.Type);
        Assert.False(conflict.IsTransient);
    }

    [Theory]
    [InlineData("InternalServerError")]
    [InlineData("InternalFailure")]
    [InlineData("ProvisionedThroughputExceededException")]
    [InlineData("ThrottlingException")]
    [InlineData("RequestLimitExceeded")]
    [InlineData("LimitExceededException")]
    [InlineData("ReplicatedWriteConflictException")]
    [InlineData("TransactionInProgressException")]
    public async Task TransientTypes(string type)
    {
        var ex = await ParseAsync($$"""{"__type":"com.amazonaws.dynamodb.v20120810#{{type}}","message":"Failed"}""");

        Assert.True(ex.IsTransient);
    }

    [Fact]
    public async Task ServerErrorsAreTransient()
    {
        var ex = await ParseAsync("""{"__type":"com.amazon.coral.service#ServiceUnavailable","message":"Unavailable"}""", HttpStatusCode.ServiceUnavailable);

        Assert.True(ex.IsTransient);
    }

    [Fact]
    public async Task NonJsonErrorsAreDynamoDbExceptions()
    {
        var ex = await ParseAsync("<html>Service Unavailable</html>", HttpStatusCode.ServiceUnavailable);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.HttpStatusCode);
    }

    [Fact]
    public async Task NonObjectJsonIsDynamoDbException()
    {
        var ex = await ParseAsync("[]", HttpStatusCode.BadGateway);

        Assert.Equal("Unexpected response. status = BadGateway | []", ex.Message);
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError, true)]
    [InlineData(HttpStatusCode.ServiceUnavailable, true)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.OK, false)]
    public void IsTransient_UsesStatusCode(HttpStatusCode statusCode, bool expected)
    {
        Assert.Equal(expected, new DynamoDbException("Failed", statusCode).IsTransient);
    }
}
