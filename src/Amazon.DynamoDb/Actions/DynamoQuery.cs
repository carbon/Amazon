﻿using System.Text;

using Carbon.Data.Expressions;

namespace Amazon.DynamoDb;

public sealed class DynamoQuery
{
#nullable disable

    public DynamoQuery() { }

    public DynamoQuery(params Expression[] conditions)
    {
        if (conditions.Length is 0)
        {
            return;
        }

        var conjunction = DynamoExpression.Conjunction(conditions);

        if (conjunction.HasAttributeNames)
        {
            ExpressionAttributeNames = conjunction.AttributeNames;
        }

        if (conjunction.HasAttributeValues)
        {
            ExpressionAttributeValues = conjunction.AttributeValues;
        }

        KeyConditionExpression = conjunction.Text;
    }

#nullable enable

    public string TableName { get; set; }

    public bool? ConsistentRead { get; set; }

    public string? IndexName { get; set; }

    public string KeyConditionExpression { get; set; }

    public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

    public AttributeCollection? ExpressionAttributeValues { get; set; }

    public string? FilterExpression { get; set; }

    /// <summary>
    /// Default is true (ascending).	
    /// </summary>
    public bool? ScanIndexForward { get; set; }

    public string? ProjectionExpression { get; set; }

    public int? Limit { get; set; }

    public SelectEnum? Select { get; set; }

    /// <summary>
    /// If set to TOTAL, ConsumedCapacity is included in the response; 
    /// if set to NONE (the default), ConsumedCapacity is not included.
    /// </summary>
    public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

    public Dictionary<string, DbValue>? ExclusiveStartKey { get; set; }

    #region Helpers

    public DynamoQuery Descending()
    {
        ScanIndexForward = false;

        return this;
    }

    #endregion

    #region Builder

    private DynamoExpression _filter;

    public DynamoQuery Filter(params Expression[] conditions)
    {
        if (_filter is null)
        {
            var attributeNames = ExpressionAttributeNames ?? [];

            ExpressionAttributeValues ??= [];

            _filter = new DynamoExpression(attributeNames, ExpressionAttributeValues);
        }

        foreach (Expression condition in conditions)
        {
            _filter.Add(condition);
        }

        FilterExpression = _filter.Text;

        if (_filter.HasAttributeNames && ExpressionAttributeNames is null)
        {
            ExpressionAttributeNames = _filter.AttributeNames;
        }

        return this;
    }

    public DynamoQuery Take(int take)
    {
        Limit = take;

        return this;
    }

    public DynamoQuery Include(ReadOnlySpan<string> values)
    {
        ExpressionAttributeNames ??= [];

        var sb = StringBuilderCache.Acquire();

        foreach (string value in values)
        {
            if (sb.Length > 0)
            {
                sb.Append(',');
            }

            sb.WriteName(value, ExpressionAttributeNames);
        }

        ProjectionExpression = StringBuilderCache.ExtractAndRelease(sb);

        return this;
    }

    public DynamoQuery WithIndex(string name)
    {
        this.IndexName = name;

        return this;
    }

    #endregion
}
