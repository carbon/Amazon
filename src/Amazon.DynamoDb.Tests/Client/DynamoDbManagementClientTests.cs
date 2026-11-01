using System.Text.Json;

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb.Tests;

public class DynamoDbManagementClientTests
{
    private static DynamoDbManagementClient CreateClient(FakeDynamoHandler handler)
    {
        return FakeDynamo.Use(new DynamoDbManagementClient(AwsRegion.USEast1, FakeDynamo.Credential), handler);
    }

    private static CreateTableRequest CreateRequest() => new(
        tableName            : "Widgets",
        attributeDefinitions : [new AttributeDefinition { AttributeName = "id", AttributeType = AttributeType.N }],
        keySchema            : [new KeySchemaElement("id", KeyType.HASH)]);

    [Fact]
    public async Task CreateTableAsync_SendsDefinition()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"TableDescription":{"TableName":"Widgets"}}"""));

        var request = CreateRequest();

        request.BillingMode = BillingMode.PAY_PER_REQUEST;
        request.ProvisionedThroughput = new ProvisionedThroughput(1, 2);
        request.Tags = [new Tag("a", "b")];
        request.StreamSpecification = new StreamSpecification(true, StreamViewType.NEW_IMAGE);
        request.GlobalSecondaryIndexes = [new GlobalSecondaryIndex("g", [new KeySchemaElement("id", KeyType.HASH)], new Projection([], ProjectionType.ALL))];
        request.LocalSecondaryIndexes = [new LocalSecondaryIndex("l", [new KeySchemaElement("id", KeyType.RANGE)], new Projection(["a"], ProjectionType.INCLUDE))];

        var result = await CreateClient(handler).CreateTableAsync(request);

        Assert.Equal("Widgets", result.TableDescription.TableName);
        Assert.Equal("DynamoDB_20120810.CreateTable", handler.Requests[0].Target);

        var body = JsonDocument.Parse(handler.Requests[0].Body).RootElement;

        Assert.Equal("Widgets", body.GetProperty("TableName").GetString());
        Assert.Equal("PAY_PER_REQUEST", body.GetProperty("BillingMode").GetString());
        Assert.Equal("N", body.GetProperty("AttributeDefinitions")[0].GetProperty("AttributeType").GetString());
        Assert.Equal("HASH", body.GetProperty("KeySchema")[0].GetProperty("KeyType").GetString());
        Assert.Equal(2, body.GetProperty("ProvisionedThroughput").GetProperty("WriteCapacityUnits").GetInt32());
        Assert.Equal("a", body.GetProperty("Tags")[0].GetProperty("Key").GetString());
        Assert.Equal("NEW_IMAGE", body.GetProperty("StreamSpecification").GetProperty("StreamViewType").GetString());
        Assert.Equal("ALL", body.GetProperty("GlobalSecondaryIndexes")[0].GetProperty("Projection").GetProperty("ProjectionType").GetString());
        Assert.Equal("RANGE", body.GetProperty("LocalSecondaryIndexes")[0].GetProperty("KeySchema")[0].GetProperty("KeyType").GetString());
    }

    [Theory]
    [InlineData("DeleteTable")]
    [InlineData("DescribeTable")]
    [InlineData("DescribeTimeToLive")]
    public async Task TableActions_SendTableName(string action)
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        var client = CreateClient(handler);

        _ = action switch {
            "DeleteTable"   => (Task)client.DeleteTableAsync("Widgets"),
            "DescribeTable" => client.DescribeTableAsync("Widgets"),
            _               => client.DescribeTimeToLiveAsync("Widgets")
        };

        await Task.Yield();

        while (handler.Requests.Count is 0)
        {
            await Task.Delay(1);
        }

        Assert.Equal($"DynamoDB_20120810.{action}", handler.Requests[0].Target);
        Assert.Equal("""{"TableName":"Widgets"}""", handler.Requests[0].Body);
    }

    [Fact]
    public async Task DescribeTimeToLiveAsync_ReadsStatus()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"TimeToLiveDescription":{"AttributeName":"expires","TimeToLiveStatus":"ENABLED"}}"""));

        var result = await CreateClient(handler).DescribeTimeToLiveAsync("Widgets");

        Assert.Equal("expires", result.TimeToLiveDescription.AttributeName);
        Assert.Equal(TimeToLiveStatus.ENABLED, result.TimeToLiveDescription.TimeToLiveStatus);
    }

    [Fact]
    public async Task ListTablesAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"TableNames":["a","b"],"LastEvaluatedTableName":"b"}"""));

        var result = await CreateClient(handler).ListTablesAsync(new ListTablesRequest { Limit = 2, ExclusiveStartTableName = "start" });

        Assert.Equal(["a", "b"], result.TableNames);
        Assert.True(result.HasMoreTables);

        Assert.Equal("""{"ExclusiveStartTableName":"start","Limit":2}""", handler.Requests[0].Body);
    }

    [Fact]
    public void ListTablesResult_HasNoMoreTablesWithoutLastName()
    {
        using var json = JsonDocument.Parse("""{"TableNames":[]}""");

        Assert.False(ListTablesResult.FromJsonElement(json.RootElement).HasMoreTables);
    }

    [Fact]
    public async Task UpdateTableAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"TableDescription":{"TableName":"Widgets"}}"""));

        var result = await CreateClient(handler).UpdateTableAsync(new UpdateTableRequest("Widgets") {
            BillingMode = BillingMode.PROVISIONED
        });

        Assert.Equal("Widgets", result.TableDescription.TableName);

        var body = JsonDocument.Parse(handler.Requests[0].Body).RootElement;

        Assert.Equal("PROVISIONED", body.GetProperty("BillingMode").GetString());
    }

    [Fact]
    public async Task UpdateTimeToLiveAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"TimeToLiveSpecification":{"AttributeName":"expires","Enabled":true}}"""));

        var result = await CreateClient(handler).UpdateTimeToLiveAsync(new UpdateTimeToLiveRequest("Widgets", "expires", true));

        Assert.Equal("expires", result.TimeToLiveSpecification.AttributeName);
        Assert.True(result.TimeToLiveSpecification.Enabled);

        Assert.Equal("""{"TableName":"Widgets","TimeToLiveSpecification":{"AttributeName":"expires","Enabled":true}}""", handler.Requests[0].Body);
    }

    [Fact]
    public async Task ThrowsDynamoDbExceptionOnError()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Error("ResourceNotFoundException", "Missing"));

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => CreateClient(handler).DescribeTableAsync("Widgets"));

        Assert.Equal("ResourceNotFoundException", ex.Type);
        Assert.Equal("Missing", ex.Message);
    }

    [Fact]
    public void RequestsValidateArguments()
    {
        KeySchemaElement[] key = [new KeySchemaElement("id", KeyType.HASH)];
        var projection = new Projection([], ProjectionType.ALL);

        Assert.Throws<ArgumentNullException>(() => new CreateTableRequest(null, [], key));
        Assert.Throws<ArgumentNullException>(() => new CreateTableRequest("t", null, key));
        Assert.Throws<ArgumentNullException>(() => new CreateTableRequest("t", [], null));
        Assert.Throws<ArgumentNullException>(() => new UpdateTableRequest(null));
        Assert.Throws<ArgumentException>(() => new UpdateTableRequest(""));
        Assert.Throws<ArgumentNullException>(() => new UpdateTimeToLiveRequest(null, "a", true));
        Assert.Throws<ArgumentNullException>(() => new KeySchemaElement(null, KeyType.HASH));
        Assert.Throws<ArgumentNullException>(() => new Tag(null, "b"));
        Assert.Throws<ArgumentNullException>(() => new Tag("a", null));
        Assert.Throws<ArgumentNullException>(() => new GlobalSecondaryIndex(null, key, projection));
        Assert.Throws<ArgumentNullException>(() => new GlobalSecondaryIndex("i", null, projection));
        Assert.Throws<ArgumentNullException>(() => new GlobalSecondaryIndex("i", key, null));
        Assert.Throws<ArgumentNullException>(() => new LocalSecondaryIndex(null, key, projection));
        Assert.Throws<ArgumentNullException>(() => new LocalSecondaryIndex("i", null, projection));
        Assert.Throws<ArgumentNullException>(() => new LocalSecondaryIndex("i", key, null));
        Assert.Throws<ArgumentNullException>(() => new CreateGlobalSecondaryIndexAction(null, key, projection));
        Assert.Throws<ArgumentNullException>(() => new CreateGlobalSecondaryIndexAction("i", null, projection));
        Assert.Throws<ArgumentNullException>(() => new CreateGlobalSecondaryIndexAction("i", key, null));
    }
}
