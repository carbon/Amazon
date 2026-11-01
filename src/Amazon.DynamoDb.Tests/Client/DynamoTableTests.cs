using System.Text.Json;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Tests;

public class DynamoTableTests
{
    private static string ItemJson(long id, string name = "a") => $$$"""{"id":{"N":"{{{id}}}"},"name":{"S":"{{{name}}}"}}""";

    private static string Page(string lastKey, params long[] ids)
    {
        var items = string.Join(',', ids.Select(static id => ItemJson(id)));
        var next  = lastKey is null ? "" : $$$""","LastEvaluatedKey":{"id":{"N":"{{{lastKey}}}"}}""";

        return $$$"""{"Count":{{{ids.Length}}},"Items":[{{{items}}}]{{{next}}}}""";
    }

    private static Func<System.Net.Http.HttpResponseMessage> Json(string json) => () => FakeDynamo.Json(json);

    private static JsonElement Body(RecordedRequest request) => JsonDocument.Parse(request.Body).RootElement;

    [Fact]
    public void ValidatesArguments()
    {
        var client = FakeDynamo.CreateClient(new FakeDynamoHandler(FakeDynamo.Throttled));

        Assert.Throws<ArgumentNullException>(() => new DynamoTable<Widget, long>(null, client));
        Assert.Throws<ArgumentNullException>(() => new DynamoTable<Widget, long>("Widgets", (DynamoDbClient)null));
    }

    [Fact]
    public async Task GetTable_UsesDatasetNameByDefault()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        var client = FakeDynamo.CreateClient(handler);

        await client.GetTable<Widget, long>().FindAsync(1);
        await client.GetTable<Widget, long>("Other").FindAsync(1);

        Assert.Equal("Widgets", Body(handler.Requests[0]).GetProperty("TableName").GetString());
        Assert.Equal("Other", Body(handler.Requests[1]).GetProperty("TableName").GetString());
    }

    [Fact]
    public void PrimaryKey_ComesFromDataset()
    {
        var table = FakeDynamo.CreateTable(new FakeDynamoHandler(FakeDynamo.Throttled));

        Assert.Equal(["id"], table.PrimaryKey.Names);
    }

    [Fact]
    public async Task FindAsync_MapsItem()
    {
        var handler = new FakeDynamoHandler(Json($$$"""{"Item":{{{ItemJson(5, "five")}}}}"""));

        var widget = await FakeDynamo.CreateTable(handler).FindAsync(5, isConsistent: true);

        Assert.Equal(5, widget.Id);
        Assert.Equal("five", widget.Name);

        var body = Body(handler.Requests[0]);

        Assert.True(body.GetProperty("ConsistentRead").GetBoolean());
        Assert.Equal("5", body.GetProperty("Key").GetProperty("id").GetProperty("N").GetString());
    }

    [Fact]
    public async Task FindAsync_ReturnsNullWhenMissing()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        Assert.Null(await FakeDynamo.CreateTable(handler).FindAsync(5));
    }

    [Fact]
    public async Task FindAsync_RetriesTransientErrors()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled, Json($$$"""{"Item":{{{ItemJson(5)}}}}"""));

        Assert.Equal(5, (await FakeDynamo.CreateTable(handler).FindAsync(5)).Id);

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task FindAsync_WrapsLastErrorWhenExhausted()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled);

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateTable(handler).FindAsync(5));

        Assert.Equal("Unrecoverable exception getting '5' from 'Widgets'", ex.Message);
        Assert.IsType<DynamoDbException>(ex.InnerException);
    }

    [Fact]
    public async Task ExistsAsync()
    {
        var handler = new FakeDynamoHandler(Json($$$"""{"Item":{{{ItemJson(5)}}}}"""), Json("{}"));

        var table = FakeDynamo.CreateTable(handler);

        Assert.True(await table.ExistsAsync(5));
        Assert.False(await table.ExistsAsync(6));

        Assert.Equal("id", Body(handler.Requests[0]).GetProperty("AttributesToGet")[0].GetString());
    }

    [Fact]
    public async Task FindAllAsync_ReturnsNothingWithoutKeys()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled);

        Assert.Empty(await FakeDynamo.CreateTable(handler).FindAllAsync());
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task FindAllAsync_MapsItems()
    {
        var handler = new FakeDynamoHandler(Json($$$"""{"Responses":{"Widgets":[{{{ItemJson(1)}}},{{{ItemJson(2)}}}]}}"""));

        var widgets = await FakeDynamo.CreateTable(handler).FindAllAsync(
            new Dictionary<string, DbValue> { { "id", new DbValue(1) } },
            new Dictionary<string, DbValue> { { "id", new DbValue(2) } });

        Assert.Equal([1L, 2L], widgets.Select(static widget => widget.Id));
    }

    [Fact]
    public async Task FindAllAsync_FetchesUnprocessedKeys()
    {
        var handler = new FakeDynamoHandler(
            Json($$$"""{"Responses":{"Widgets":[{{{ItemJson(1)}}}]},""" + """ "UnprocessedKeys":{"Widgets":{"Keys":[{"id":{"N":"2"}}]}}}"""),
            Json($$$"""{"Responses":{"Widgets":[{{{ItemJson(2)}}}]}}"""));

        var widgets = await FakeDynamo.CreateTable(handler).FindAllAsync(
            new Dictionary<string, DbValue> { { "id", new DbValue(1) } },
            new Dictionary<string, DbValue> { { "id", new DbValue(2) } });

        Assert.Equal(2, widgets.Count);
    }

    [Fact]
    public async Task QueryAsync_SplitsKeyAndFilterConditions()
    {
        var handler = new FakeDynamoHandler(Json(Page(null, 1)));

        var widgets = await FakeDynamo.CreateTable(handler).QueryAsync(
            Expression.Eq("id", 1),
            Expression.Eq("name", "a"));

        Assert.Equal(1, Assert.Single(widgets).Id);

        var body = Body(handler.Requests[0]);

        Assert.Equal("Widgets", body.GetProperty("TableName").GetString());
        Assert.Equal("id = :v0", body.GetProperty("KeyConditionExpression").GetString());
        Assert.Equal("#name = :v1", body.GetProperty("FilterExpression").GetString());
    }

    [Fact]
    public async Task QueryAsync_RejectsUnsupportedExpressions()
    {
        var table = FakeDynamo.CreateTable(new FakeDynamoHandler(FakeDynamo.Throttled));

        await Assert.ThrowsAsync<Exception>(() => table.QueryAsync(Expression.Func("attribute_exists", new Symbol("a"))));
    }

    [Fact]
    public async Task QueryAllAsync_ReadsEveryPageWithoutLimit()
    {
        var handler = new FakeDynamoHandler(Json(Page("1", 1)), Json(Page(null, 2)));

        var widgets = await FakeDynamo.CreateTable(handler).QueryAllAsync(new DynamoQuery(Expression.Eq("id", 1)));

        Assert.Equal([1L, 2L], widgets.Select(static widget => widget.Id));
    }

    [Fact]
    public async Task QueryAllAsync_ContinuesFromLastEvaluatedKeyWithLimit()
    {
        var handler = new FakeDynamoHandler(Json(Page("1", 1)), Json(Page(null, 2)));

        var widgets = await FakeDynamo.CreateTable(handler).QueryAllAsync(new DynamoQuery(Expression.Eq("id", 1)) { Limit = 5 });

        Assert.Equal([1L, 2L], widgets.Select(static widget => widget.Id));
        Assert.Equal(4, Body(handler.Requests[1]).GetProperty("Limit").GetInt32());
        Assert.Equal("1", Body(handler.Requests[1]).GetProperty("ExclusiveStartKey").GetProperty("id").GetProperty("N").GetString());
    }

    [Fact]
    public async Task QueryAllAsync_StopsWhenLimitIsReached()
    {
        var handler = new FakeDynamoHandler(Json(Page("2", 1, 2)), Json(Page(null, 3)));

        var widgets = await FakeDynamo.CreateTable(handler).QueryAllAsync(new DynamoQuery(Expression.Eq("id", 1)) { Limit = 2 });

        Assert.Equal(2, widgets.Count);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task CountAsync_SelectsCount()
    {
        var handler = new FakeDynamoHandler(Json("""{"Count":5}"""));

        Assert.Equal(5, await FakeDynamo.CreateTable(handler).CountAsync(Expression.Eq("id", 1)));

        var body = Body(handler.Requests[0]);

        Assert.Equal("COUNT", body.GetProperty("Select").GetString());
        Assert.Equal("Widgets", body.GetProperty("TableName").GetString());
    }

    [Fact]
    public async Task CountAsync_AddsEveryPage()
    {
        var handler = new FakeDynamoHandler(
            Json("""{"Count":5,"LastEvaluatedKey":{"id":{"N":"1"}}}"""),
            Json("""{"Count":3}"""));

        Assert.Equal(8, await FakeDynamo.CreateTable(handler).CountAsync(Expression.Eq("id", 1)));
    }

    [Fact]
    public async Task ScanAsync_Enumerable_FollowsPages()
    {
        var handler = new FakeDynamoHandler(Json(Page("1", 1)), Json(Page(null, 2)));

        var ids = new List<long>();

        await foreach (var widget in FakeDynamo.CreateTable(handler).ScanAsync(Expression.Eq("name", "a")))
        {
            ids.Add(widget.Id);
        }

        Assert.Equal([1L, 2L], ids);
        Assert.Equal("#name = :v0", Body(handler.Requests[0]).GetProperty("FilterExpression").GetString());
        Assert.Equal("1", Body(handler.Requests[1]).GetProperty("ExclusiveStartKey").GetProperty("id").GetProperty("N").GetString());
    }

    [Fact]
    public async Task ScanAsync_Page_AppliesOptions()
    {
        var handler = new FakeDynamoHandler(Json(Page("1", 1)));

        var result = await FakeDynamo.CreateTable(handler).ScanAsync(
            startKey   : new Dictionary<string, object> { { "id", 7L } },
            conditions : [Expression.Eq("name", "a")],
            take       : 10);

        Assert.Single(result);

        var page = Assert.IsType<QueryResult<Widget>>(result);

        Assert.Equal(1, page.LastEvaluatedKey["id"].ToInt());
        Assert.Equal(1, page[0].Id);

        var body = Body(handler.Requests[0]);

        Assert.Equal(10, body.GetProperty("Limit").GetInt32());
        Assert.Equal("7", body.GetProperty("ExclusiveStartKey").GetProperty("id").GetProperty("N").GetString());
    }

    [Fact]
    public async Task CreateAsync_RequiresKeyToNotExist()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        await FakeDynamo.CreateTable(handler).CreateAsync(new Widget { Id = 1, Name = "a" });

        var body = Body(handler.Requests[0]);

        Assert.Equal("attribute_not_exists(id)", body.GetProperty("ConditionExpression").GetString());
        Assert.Equal("a", body.GetProperty("Item").GetProperty("name").GetProperty("S").GetString());
    }

    [Fact]
    public async Task CreateAsync_ThrowsConflictWhenExists()
    {
        var handler = new FakeDynamoHandler(() => FakeDynamo.Error("ConditionalCheckFailedException", "Exists"));

        var ex = await Assert.ThrowsAsync<ConflictException>(() => FakeDynamo.CreateTable(handler).CreateAsync(new Widget { Id = 1, Name = "a" }));

        Assert.Equal("Exists", ex.Message);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task PutAsync_SendsItem()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        await FakeDynamo.CreateTable(handler).PutAsync(new Widget { Id = 1, Name = "a" });

        var body = Body(handler.Requests[0]);

        Assert.Equal("1", body.GetProperty("Item").GetProperty("id").GetProperty("N").GetString());
        Assert.False(body.TryGetProperty("ConditionExpression", out var condition) && condition.ValueKind is not JsonValueKind.Null);
    }

    [Fact]
    public async Task PutAsync_Batch_SplitsIntoTwentyFives()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        var widgets = Enumerable.Range(1, 30).Select(static i => new Widget { Id = i, Name = "a" });

        var result = await FakeDynamo.CreateTable(handler).PutAsync(widgets);

        Assert.Equal(2, result.BatchCount);
        Assert.Equal(2, result.RequestCount);
        Assert.Equal(30, result.ItemCount);
        Assert.Equal(25, Body(handler.Requests[0]).GetProperty("RequestItems").GetProperty("Widgets").GetArrayLength());
        Assert.Equal(5, Body(handler.Requests[1]).GetProperty("RequestItems").GetProperty("Widgets").GetArrayLength());
    }

    [Fact]
    public async Task PutAsync_Batch_ResendsUnprocessedItems()
    {
        var handler = new FakeDynamoHandler(
            Json($$$"""{"UnprocessedItems":{"Widgets":[{"PutRequest":{"Item":{{{ItemJson(2)}}}}}]}}"""),
            Json("{}"));

        var result = await FakeDynamo.CreateTable(handler).PutAsync([new Widget { Id = 1, Name = "a" }, new Widget { Id = 2, Name = "a" }]);

        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(1, Body(handler.Requests[1]).GetProperty("RequestItems").GetProperty("Widgets").GetArrayLength());
        Assert.Equal(1, result.BatchCount);
        Assert.Equal(2, result.RequestCount);
        Assert.Equal(2, result.ItemCount);
    }

    [Fact]
    public async Task PutAsync_Batch_RetriesTransientErrors()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled, Json("{}"));

        var result = await FakeDynamo.CreateTable(handler).PutAsync([new Widget { Id = 1, Name = "a" }]);

        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(1, result.ItemCount);
    }

    [Fact]
    public async Task PatchAsync_SendsChanges()
    {
        var handler = new FakeDynamoHandler(Json("""{"Attributes":{"name":{"S":"b"}}}"""));

        var result = await FakeDynamo.CreateTable(handler).PatchAsync(1, Change.Replace("name", "b"));

        Assert.Equal("b", result.Attributes["name"].ToString());

        var body = Body(handler.Requests[0]);

        Assert.Equal("SET #name = :v0", body.GetProperty("UpdateExpression").GetString());
        Assert.Equal("1", body.GetProperty("Key").GetProperty("id").GetProperty("N").GetString());
    }

    [Fact]
    public async Task PatchAsync_SendsConditionsAndReturnValues()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        var table = FakeDynamo.CreateTable(handler);

        await table.PatchAsync(1, [Change.Replace("name", "b")], [Expression.Eq("name", "a")], ReturnValues.ALL_NEW);
        await table.PatchAsync(1, [Change.Replace("name", "b")], ReturnValues.ALL_OLD);

        Assert.Equal("#name = :v1", Body(handler.Requests[0]).GetProperty("ConditionExpression").GetString());
        Assert.Equal("ALL_NEW", Body(handler.Requests[0]).GetProperty("ReturnValues").GetString());
        Assert.Equal("ALL_OLD", Body(handler.Requests[1]).GetProperty("ReturnValues").GetString());
    }

    [Fact]
    public async Task DeleteAsync_AcceptsRecordKeyAndValue()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        var table = FakeDynamo.CreateTable(handler);

        await table.DeleteAsync(new Widget { Id = 1 });
        await table.DeleteAsync(2);
        await table.DeleteAsync(Key<Widget>.FromTuple(3L));

        Assert.Equal(["1", "2", "3"], handler.Requests.Select(static request => Body(request).GetProperty("Key").GetProperty("id").GetProperty("N").GetString()));
    }

    [Fact]
    public async Task DeleteAsync_SendsConditionsAndReturnValues()
    {
        var handler = new FakeDynamoHandler(Json("{}"));

        var table = FakeDynamo.CreateTable(handler);
        var key   = Key<Widget>.FromTuple(1L);

        await table.DeleteAsync(key, ReturnValues.ALL_OLD);
        await table.DeleteAsync(key, Expression.Eq("name", "a"));
        await table.DeleteAsync(key, [], ReturnValues.ALL_OLD);

        Assert.Equal("ALL_OLD", Body(handler.Requests[0]).GetProperty("ReturnValues").GetString());
        Assert.Equal("#name = :v0", Body(handler.Requests[1]).GetProperty("ConditionExpression").GetString());
        Assert.Equal("a", Body(handler.Requests[1]).GetProperty("ExpressionAttributeValues").GetProperty(":v0").GetProperty("S").GetString());
        Assert.Equal(JsonValueKind.Null, Body(handler.Requests[2]).GetProperty("ConditionExpression").ValueKind);
    }

    [Fact]
    public async Task DeleteAsync_RetriesTransientErrors()
    {
        var handler = new FakeDynamoHandler(FakeDynamo.Throttled, Json("{}"));

        await FakeDynamo.CreateTable(handler).DeleteAsync(1);

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task FindAsync_LeavesNullAttributesUnset()
    {
        var handler = new FakeDynamoHandler(Json("""{"Item":{"id":{"N":"5"},"name":{"NULL":true}}}"""));

        var widget = await FakeDynamo.CreateTable(handler).FindAsync(5);

        Assert.Equal(5, widget.Id);
        Assert.Null(widget.Name);
    }

    [Fact]
    public async Task FindAsync_RetriesNonJsonServerErrors()
    {
        var handler = new FakeDynamoHandler(
            () => new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable) {
                Content = new System.Net.Http.StringContent("<html>Service Unavailable</html>")
            },
            Json("""{"Item":{"id":{"N":"5"}}}"""));

        Assert.Equal(5, (await FakeDynamo.CreateTable(handler).FindAsync(5)).Id);
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task FindAllAsync_ResendsOnlyUnprocessedKeys()
    {
        var handler = new FakeDynamoHandler(
            Json("""{"Responses":{"Widgets":[]},"UnprocessedKeys":{"Widgets":{"Keys":[{"id":{"N":"2"}}],"ConsistentRead":true,"AttributesToGet":["id"]}}}"""),
            Json("""{"Responses":{"Widgets":[{"id":{"N":"2"}}]},"UnprocessedKeys":{}}"""));

        var widgets = await FakeDynamo.CreateTable(handler).FindAllAsync(new Dictionary<string, DbValue> { { "id", new DbValue(2) } });

        Assert.Equal(2, Assert.Single(widgets).Id);

        var table = Body(handler.Requests[1]).GetProperty("RequestItems").GetProperty("Widgets");

        Assert.Equal("2", Assert.Single(table.GetProperty("Keys").EnumerateArray()).GetProperty("id").GetProperty("N").GetString());
        Assert.True(table.GetProperty("ConsistentRead").GetBoolean());
        Assert.Equal("id", table.GetProperty("AttributesToGet")[0].GetString());
    }

    [Fact]
    public async Task FindAllAsync_ThrowsWhenKeysStayUnprocessed()
    {
        var handler = new FakeDynamoHandler(Json("""{"Responses":{},"UnprocessedKeys":{"Widgets":{"Keys":[{"id":{"N":"2"}}]}}}"""));

        var ex = await Assert.ThrowsAsync<DynamoDbException>(() => FakeDynamo.CreateTable(handler).FindAllAsync(new Dictionary<string, DbValue> { { "id", new DbValue(2) } }));

        Assert.Equal("Unprocessed keys remain in 'Widgets' after retrying", ex.Message);
        Assert.True(handler.Requests.Count > 1);
    }

    [Fact]
    public async Task FindAllAsync_ReturnsNothingForEmptyResponses()
    {
        var handler = new FakeDynamoHandler(Json("""{"Responses":{}}"""));

        Assert.Empty(await FakeDynamo.CreateTable(handler).FindAllAsync(new Dictionary<string, DbValue> { { "id", new DbValue(2) } }));
    }

    [Fact]
    public async Task CountAsync_ContinuesFromLastEvaluatedKey()
    {
        var handler = new FakeDynamoHandler(
            Json("""{"Count":5,"LastEvaluatedKey":{"id":{"N":"1"}}}"""),
            Json("""{"Count":3}"""));

        await FakeDynamo.CreateTable(handler).CountAsync(Expression.Eq("id", 1));

        Assert.Equal("1", Body(handler.Requests[1]).GetProperty("ExclusiveStartKey").GetProperty("id").GetProperty("N").GetString());
    }

    [Fact]
    public async Task CountAsync_StopsAtLimit()
    {
        var handler = new FakeDynamoHandler(Json("""{"Count":5,"LastEvaluatedKey":{"id":{"N":"1"}}}"""));

        var count = await FakeDynamo.CreateTable(handler).CountAsync(new DynamoQuery(Expression.Eq("id", 1)) { Limit = 5 });

        Assert.Equal(5, count);
        Assert.Single(handler.Requests);
    }
}
