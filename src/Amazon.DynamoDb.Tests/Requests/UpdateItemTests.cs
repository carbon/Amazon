﻿using Carbon.Data;

namespace Amazon.DynamoDb.Tests;

public class UpdateItemRequestTests
{
    [Fact]
    public void UpdateItemRequest1()
    {
        var date = DateTimeOffset.FromUnixTimeSeconds(1497282355);

        var request = new UpdateItemRequest("Entities", Key<Entity>.FromValues([1]).ToDictionary(x => x.Key, y => new DbValue(y.Value)), [
            Change.Replace("locked", date)
        ]);

        var expect = """{"TableName":"Entities","Key":{"id":{"N":"1"}},"ExpressionAttributeValues":{":v0":{"N":"1497282355"}},"UpdateExpression":"SET locked = :v0"}""";

        Assert.Equal(expect, request.ToJsonString());
    }

    [Fact]
    public void UpdateItemRequest2()
    {
        var date = DateTimeOffset.FromUnixTimeSeconds(1497282355);

        var request = new UpdateItemRequest("Entities", ((Key<Entity>)(1)).ToDictionary(x => x.Key, y => new DbValue(y.Value)), [
            Change.Replace("locked", date),
            Change.Remove("deleted")
        ]);

        var expect = """{"TableName":"Entities","Key":{"id":{"N":"1"}},"ExpressionAttributeValues":{":v0":{"N":"1497282355"}},"UpdateExpression":"SET locked = :v0\r\nREMOVE deleted"}""";

        Assert.Equal(expect, request.ToJsonString());
    }

    [Fact]
    public void UpdateWithSingleRemove()
    {
        var key = new Dictionary<string, DbValue> {
            { "id", new DbValue(1) }
        };

        var request = new UpdateItemRequest("Conversations", key, [
            Change.Remove("deleted")
        ]);

        Assert.Null(request.ExpressionAttributeNames);
        Assert.Null(request.ExpressionAttributeValues);

        var expect = """{"TableName":"Conversations","Key":{"id":{"N":"1"}},"UpdateExpression":"REMOVE deleted"}""";

        Assert.Equal(expect, request.ToJsonString());
    }
}