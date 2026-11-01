using System.Text.Json;

namespace Amazon.DynamoDb.Tests;

public class ExecuteStatementTests
{
    private const string Select = """SELECT * FROM "Widgets" WHERE id = ?""";

    private static JsonElement Body(FakeDynamoHandler handler, int index = 0) => JsonDocument.Parse(handler.Requests[index].Body).RootElement;

    [Fact]
    public void RequiresStatement()
    {
        Assert.Throws<ArgumentNullException>(() => new ExecuteStatementRequest(null));
        Assert.Throws<ArgumentException>(() => new ExecuteStatementRequest(""));
    }

    [Fact]
    public async Task SendsStatementAndParameters()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Items":[{"id":{"N":"1"},"name":{"S":"a"}}]}"""));

        var result = await FakeDynamo.CreateClient(handler).ExecuteStatementAsync(Select, new DbValue(1), new DbValue("a"));

        Assert.Equal("a", Assert.Single(result.Items)["name"].ToString());
        Assert.Null(result.NextToken);

        Assert.Equal("DynamoDB_20120810.ExecuteStatement", handler.Requests[0].Target);

        var body = Body(handler);

        Assert.Equal(Select, body.GetProperty("Statement").GetString());
        Assert.Equal("1", body.GetProperty("Parameters")[0].GetProperty("N").GetString());
        Assert.Equal("a", body.GetProperty("Parameters")[1].GetProperty("S").GetString());
    }

    [Fact]
    public async Task SendsNoParametersWhenEmpty()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        await FakeDynamo.CreateClient(handler).ExecuteStatementAsync("""SELECT * FROM "Widgets" """);

        Assert.Equal(JsonValueKind.Null, Body(handler).GetProperty("Parameters").ValueKind);
    }

    [Fact]
    public async Task ItemsAreEmptyForWrites()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        var result = await FakeDynamo.CreateClient(handler).ExecuteStatementAsync("""INSERT INTO "Widgets" VALUE {'id': ?}""", new DbValue(1));

        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task SendsOptions()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        await FakeDynamo.CreateClient(handler).ExecuteStatementAsync(new ExecuteStatementRequest(Select, new DbValue(1)) {
            ConsistentRead = true,
            Limit = 5,
            NextToken = "token",
            ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL,
            ReturnValuesOnConditionCheckFailure = ReturnValuesOnConditionCheckFailure.ALL_OLD
        });

        var body = Body(handler);

        Assert.True(body.GetProperty("ConsistentRead").GetBoolean());
        Assert.Equal(5, body.GetProperty("Limit").GetInt32());
        Assert.Equal("token", body.GetProperty("NextToken").GetString());
        Assert.Equal("TOTAL", body.GetProperty("ReturnConsumedCapacity").GetString());
        Assert.Equal("ALL_OLD", body.GetProperty("ReturnValuesOnConditionCheckFailure").GetString());
    }

    [Fact]
    public async Task ReadsPagingAndCapacity()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json(
            """{"Items":[],"NextToken":"next","LastEvaluatedKey":{"id":{"N":"1"}},"ConsumedCapacity":{"TableName":"Widgets","CapacityUnits":0.5}}"""));

        var result = await FakeDynamo.CreateClient(handler).ExecuteStatementAsync(Select, new DbValue(1));

        Assert.Equal("next", result.NextToken);
        Assert.Equal(1, result.LastEvaluatedKey["id"].ToInt());
        Assert.Equal("Widgets", result.ConsumedCapacity.Value.TableName);
    }

    [Fact]
    public async Task ThrowsOnError()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Error("ValidationException", "Statement wasn't well formed"));

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateClient(handler).ExecuteStatementAsync("bad"));

        Assert.Equal("ValidationException", ex.Type);
        Assert.Equal("Statement wasn't well formed", ex.Message);
    }

    [Fact]
    public async Task DuplicateInsertKeepsItsErrorType()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Error("DuplicateItemException", "Exists"));

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateClient(handler).ExecuteStatementAsync("""INSERT INTO "Widgets" VALUE {'id': ?}""", new DbValue(1)));

        Assert.Equal("DuplicateItemException", ex.Type);
        Assert.False(ex.IsTransient);
    }
}
