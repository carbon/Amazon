using System.Text.Json;

using Amazon.DynamoDb.Transactions;
using Amazon.Scheduling;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Tests;

public class DynamoDbClientTests
{
    private static readonly RetryPolicy s_fastRetries = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromMilliseconds(1),
        maxDelay     : TimeSpan.FromMilliseconds(2),
        maxRetries   : 3);

    private static Dictionary<string, DbValue> Key(long id) => new() { { "id", new DbValue(id) } };

    private static AttributeCollection Item(long id) => new() { { "id", new DbValue(id) } };

    private static UpdateItemRequest UpdateRequest() => new("Widgets", Key(1), [Change.Replace("name", "a")]);

    [Fact]
    public async Task SignsAndAddressesRequests()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        await FakeDynamo.CreateClient(handler).GetItemAsync(new GetItemRequest("Widgets", Key(1)));

        var request = handler.Requests[0];

        Assert.Equal("https://dynamodb.us-east-1.amazonaws.com/", request.Uri.ToString());
        Assert.Equal("DynamoDB_20120810.GetItem", request.Target);
        Assert.Equal("application/x-amz-json-1.0", request.ContentType);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", request.Authorization);
        Assert.Contains("/us-east-1/dynamodb/aws4_request", request.Authorization);
    }

    [Fact]
    public async Task GetItemAsync_ReturnsItem()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Item":{"id":{"N":"1"}},"ConsumedCapacity":{"TableName":"Widgets","CapacityUnits":0.5}}"""));

        var result = await FakeDynamo.CreateClient(handler).GetItemAsync(new GetItemRequest("Widgets", Key(1)));

        Assert.Equal(1, result.Item["id"].ToInt());
        Assert.Equal("Widgets", result.ConsumedCapacity.Value.TableName);
        Assert.Contains("\"TableName\":\"Widgets\"", handler.Requests[0].Body);
    }

    [Fact]
    public async Task GetItemAsync_ItemIsNullWhenMissing()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        var result = await FakeDynamo.CreateClient(handler).GetItemAsync(new GetItemRequest("Widgets", Key(1)));

        Assert.Null(result.Item);
    }

    [Fact]
    public async Task PutItemAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Attributes":{"id":{"N":"1"}}}"""));

        await FakeDynamo.CreateClient(handler).PutItemAsync(new PutItemRequest("Widgets", Item(1)) {
            ReturnValues = ReturnValues.ALL_OLD
        });

        Assert.Equal("DynamoDB_20120810.PutItem", handler.Requests[0].Target);
        Assert.Contains("\"Item\":{\"id\":{\"N\":\"1\"}}", handler.Requests[0].Body);
    }

    [Fact]
    public async Task PutItemAsync_ReturnsOldAttributes()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Attributes":{"id":{"N":"1"}}}"""));

        var result = await FakeDynamo.CreateClient(handler).PutItemAsync(new PutItemRequest("Widgets", Item(1)) {
            ReturnValues = ReturnValues.ALL_OLD
        });

        Assert.Equal(1, result.Attributes["id"].ToInt());
    }

    [Fact]
    public async Task DeleteItemAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Attributes":{"id":{"N":"1"}}}"""));

        var result = await FakeDynamo.CreateClient(handler).DeleteItemAsync(new DeleteItemRequest("Widgets", Key(1)));

        Assert.Equal(1, result.Attributes["id"].ToInt());
        Assert.Equal("DynamoDB_20120810.DeleteItem", handler.Requests[0].Target);
    }

    [Fact]
    public async Task UpdateItemAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Attributes":{"name":{"S":"a"}}}"""));

        var result = await FakeDynamo.CreateClient(handler).UpdateItemAsync(UpdateRequest());

        Assert.Equal("a", result.Attributes["name"].ToString());
        Assert.Equal("DynamoDB_20120810.UpdateItem", handler.Requests[0].Target);
    }

    [Fact]
    public async Task QueryAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Count":1,"ScannedCount":2,"Items":[{"id":{"N":"1"}}],"LastEvaluatedKey":{"id":{"N":"1"}}}"""));

        var result = await FakeDynamo.CreateClient(handler).QueryAsync(new DynamoQuery(Expression.Eq("id", 1)) { TableName = "Widgets" });

        Assert.Equal(1, result.Count);
        Assert.Equal(2, result.ScannedCount);
        Assert.Single(result.Items);
        Assert.Equal(1, result.LastEvaluatedKey["id"].ToInt());
        Assert.Equal("DynamoDB_20120810.Query", handler.Requests[0].Target);
    }

    [Fact]
    public async Task QueryCountAsync_SelectsCount()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Count":5}"""));

        var result = await FakeDynamo.CreateClient(handler).QueryCountAsync(new DynamoQuery(Expression.Eq("id", 1)) { TableName = "Widgets" });

        Assert.Equal(5, result.Count);
        Assert.Contains("\"Select\":\"COUNT\"", handler.Requests[0].Body);
    }

    [Fact]
    public async Task ScanAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Count":1,"Items":[{"id":{"N":"1"}}]}"""));

        var result = await FakeDynamo.CreateClient(handler).ScanAsync(new ScanRequest("Widgets"));

        Assert.Single(result.Items);
        Assert.Equal("DynamoDB_20120810.Scan", handler.Requests[0].Target);
    }

    [Fact]
    public async Task BatchGetItemAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Responses":{"Widgets":[{"id":{"N":"1"}}]}}"""));

        var result = await FakeDynamo.CreateClient(handler).BatchGetItemAsync(new BatchGetItemRequest(new TableKeys("Widgets", Key(1))));

        Assert.Equal("Widgets", result.Responses[0].Name);
        Assert.Equal(1, result.Responses[0][0]["id"].ToInt());
        Assert.Equal("DynamoDB_20120810.BatchGetItem", handler.Requests[0].Target);
        Assert.Contains("\"RequestItems\":{\"Widgets\":{\"Keys\":[{\"id\":{\"N\":\"1\"}}]}}", handler.Requests[0].Body);
    }

    [Fact]
    public async Task BatchWriteItemAsync_ReturnsUnprocessedItems()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json(
            """{"UnprocessedItems":{"Widgets":[{"PutRequest":{"Item":{"id":{"N":"2"}}}}]}}"""));

        var batch = new TableRequests("Widgets", [new PutRequest(Item(1)), new DeleteRequest(Item(2))]);

        var result = await FakeDynamo.CreateClient(handler).BatchWriteItemAsync(batch);

        var unprocessed = Assert.Single(result.UnprocessedItems);

        Assert.Equal("Widgets", unprocessed.TableName);
        Assert.Single(unprocessed.Requests);

        Assert.Equal("DynamoDB_20120810.BatchWriteItem", handler.Requests[0].Target);

        using var body = JsonDocument.Parse(handler.Requests[0].Body);

        Assert.Equal(2, body.RootElement.GetProperty("RequestItems").GetProperty("Widgets").GetArrayLength());
    }

    [Fact]
    public async Task TransactGetItemsAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("""{"Responses":[{"Item":{"id":{"N":"1"}}},{}]}"""));

        var get = new Get("Widgets", Key(1)) { TableName = "Widgets", Key = Key(1) };

        var result = await FakeDynamo.CreateClient(handler).TransactGetItemsAsync(new TransactGetItemRequest([new TransactGetItem(get)]));

        Assert.Equal(1, result.Responses[0].Item["id"].ToInt());
        Assert.Null(result.Responses[1].Item);
        Assert.Equal("DynamoDB_20120810.TransactGetItems", handler.Requests[0].Target);
        Assert.Contains("\"Get\":{\"TableName\":\"Widgets\"", handler.Requests[0].Body);
    }

    [Fact]
    public async Task TransactWriteItemsAsync()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Json("{}"));

        TransactWriteItem[] items = [
            new TransactWriteItem { Put = new PutItem("Widgets", Item(1)) { TableName = "Widgets", Item = Item(1) } },
            new TransactWriteItem { Delete = new DeleteItem("Widgets", Item(2)) { TableName = "Widgets", Key = Item(2) } },
            new TransactWriteItem { Update = new UpdateItem("Widgets", Item(3), [Change.Replace("name", "a")]) { TableName = "Widgets", Key = Item(3) } },
            new TransactWriteItem { ConditionCheck = new ConditionCheck("Widgets", Key(4), "attribute_exists(id)") }
        ];

        await FakeDynamo.CreateClient(handler).TransactWriteItemsAsync(new TransactWriteItemsRequest(items) {
            TransactItems = items
        });

        using var body = JsonDocument.Parse(handler.Requests[0].Body);

        var written = body.RootElement.GetProperty("TransactItems");

        Assert.Equal(4, written.GetArrayLength());
        Assert.Equal("1", written[0].GetProperty("Put").GetProperty("Item").GetProperty("id").GetProperty("N").GetString());
        Assert.Equal("SET #name = :v0", written[2].GetProperty("Update").GetProperty("UpdateExpression").GetString());
        Assert.Equal("attribute_exists(id)", written[3].GetProperty("ConditionCheck").GetProperty("ConditionExpression").GetString());
    }

    [Fact]
    public void TransactionUpdate_OmitsEmptyAttributeValues()
    {
        var update = new UpdateItem("Widgets", Item(1), [Change.Remove("name")]) { TableName = "Widgets", Key = Item(1) };

        Assert.DoesNotContain("\"ExpressionAttributeValues\":{}", JsonSerializer.Serialize(update));
    }

    [Fact]
    public void TransactionUpdate_ShareNamesAndValuesWithConditions()
    {
        var update = new UpdateItem("Widgets", Item(1), [Change.Replace("name", "a")], [Expression.Eq("name", "b")]) { TableName = "Widgets", Key = Item(1) };

        Assert.Equal("SET #name = :v0", update.UpdateExpression);
        Assert.Equal("#name = :v1", update.ConditionExpression);
        Assert.Equal("name", update.ExpressionAttributeNames["#name"]);
        Assert.Equal("b", update.ExpressionAttributeValues[":v1"].ToString());
    }

    [Fact]
    public void TransactionRequests_ValidateArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new ConditionCheck(null, Key(1), "a"));
        Assert.Throws<ArgumentNullException>(() => new ConditionCheck("t", null, "a"));
        Assert.Throws<ArgumentNullException>(() => new ConditionCheck("t", Key(1), null));
        Assert.Throws<ArgumentNullException>(() => new TransactGetItemRequest(null));
        Assert.Throws<ArgumentNullException>(() => new TransactGetItem(null));
    }

    [Fact]
    public async Task PutItemUsingRetryPolicyAsync_RetriesTransientErrors()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled, () => FakeDynamo.Json("{}"));

        await FakeDynamo.CreateClient(handler).PutItemUsingRetryPolicyAsync(new PutItemRequest("Widgets", Item(1)), s_fastRetries);

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task PutItemUsingRetryPolicyAsync_ThrowsWhenExhausted()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled);

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateClient(handler).PutItemUsingRetryPolicyAsync(new PutItemRequest("Widgets", Item(1)), s_fastRetries));

        Assert.Equal("ProvisionedThroughputExceededException", ex.Type);
        Assert.True(handler.Requests.Count > 1);
    }

    [Fact]
    public async Task UpdateItemUsingRetryPolicyAsync_RetriesTransientErrors()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled, () => FakeDynamo.Json("{}"));

        await FakeDynamo.CreateClient(handler).UpdateItemUsingRetryPolicyAsync(UpdateRequest(), s_fastRetries);

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task UpdateItemUsingRetryPolicyAsync_ThrowsWhenExhausted()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled);

        await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateClient(handler).UpdateItemUsingRetryPolicyAsync(UpdateRequest(), s_fastRetries));
    }

    [Fact]
    public async Task QueryAsync_RetryPolicy_RetriesTransientErrors()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled, () => FakeDynamo.Json("""{"Count":0,"Items":[]}"""));

        await FakeDynamo.CreateClient(handler).QueryAsync(new DynamoQuery(Expression.Eq("id", 1)) { TableName = "Widgets" }, s_fastRetries);

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task QueryAsync_RetryPolicy_WrapsLastErrorWhenExhausted()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Error("ThrottlingException", "Slow down"));

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateClient(handler).QueryAsync(new DynamoQuery(Expression.Eq("id", 1)) { TableName = "Widgets" }, s_fastRetries));

        Assert.Equal("Error querying 'Widgets': Slow down", ex.Message);
        Assert.IsType<DynamoDbException>(ex.InnerException);
    }

    [Fact]
    public async Task DoesNotRetryPermanentErrors()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Error("ValidationException"));

        await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateClient(handler).UpdateItemUsingRetryPolicyAsync(UpdateRequest(), s_fastRetries));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task BatchRequests_ThrowOnError()
    {
        var client = FakeDynamo.CreateClient(new FakeDynamoHandler(() => FakeDynamo.Error("ValidationException")));

        await Assert.ThrowsAsync<DynamoDbException>(() => client.BatchGetItemAsync(new BatchGetItemRequest(new TableKeys("Widgets", Key(1)))));
    }

    [Fact]
    public void TransactionUpdate_KeepsAttributeValues()
    {
        var update = new UpdateItem("Widgets", Item(1), [Change.Replace("name", "a")]) { TableName = "Widgets", Key = Item(1) };

        Assert.Equal("a", update.ExpressionAttributeValues[":v0"].ToString());
    }
}
