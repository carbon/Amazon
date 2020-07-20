using Carbon.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public sealed class TransactWriteItemsRequest
    {
        public TransactWriteItem[] TransactItems { get; set; }
        public string? ClientRequestToken { get; set; }
        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }
        public ReturnItemCollectionMetrics? ReturnItemCollectionMetrics { get; set; }

        public TransactWriteItemsRequest(TransactWriteItem[] transactItems)
        {
            TransactItems = transactItems ?? throw new ArgumentNullException(nameof(transactItems));
        }
    }

    public class TransactWriteItem
    {
        public ConditionCheck? ConditionCheck { get; set; }
        public DeleteItem? Delete { get; set; }
        public PutItem? Put { get; set; }
        public UpdateItem? Update { get; set; }
    }

    public class ConditionCheck
    {
        public string ConditionExpression { get; set; }
        public Dictionary<string, DbValue> Key { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }
        public AttributeCollection? ExpressionAttributeValues { get; set; }
        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }

        public ConditionCheck(string tableName, Dictionary<string, DbValue> key, string conditionExpression)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ConditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
        }

        internal void SetConditions(Expression[] conditions)
        {
            var expression = DynamoExpression.Conjunction(conditions);

            ConditionExpression = expression.Text;

            if (expression.HasAttributeNames)
            {
                ExpressionAttributeNames = expression.AttributeNames;
            }

            if (expression.HasAttributeValues)
            {
                ExpressionAttributeValues = expression.AttributeValues;
            }
        }
    }

    public class DeleteItem
    {
        public AttributeCollection Key { get; set; }
        public string TableName { get; set; }
        public string? ConditionExpression { get; set; }
        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }
        public AttributeCollection? ExpressionAttributeValues { get; set; }
        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }

        public DeleteItem(string tableName, AttributeCollection key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
    }

    public class PutItem
    {
        public AttributeCollection Item { get; set; }
        public string TableName { get; set; }
        public string? ConditionExpression { get; set; }
        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }
        public AttributeCollection? ExpressionAttributeValues { get; set; }
        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }

        public PutItem(string tableName, AttributeCollection item)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }

    public class UpdateItem
    {
        public AttributeCollection Key { get; set; }
        public string TableName { get; set; }
        public string UpdateExpression { get; set; }
        public string? ConditionExpression { get; set; }
        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }
        public AttributeCollection? ExpressionAttributeValues { get; set; }
        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }

        public UpdateItem(string tableName, AttributeCollection key, string updateExpression)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            UpdateExpression = updateExpression ?? throw new ArgumentNullException(nameof(updateExpression));
        }
    }
}
