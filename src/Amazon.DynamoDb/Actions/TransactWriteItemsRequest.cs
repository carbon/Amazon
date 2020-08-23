using System;
using System.Collections.Generic;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    public sealed class TransactWriteItemsRequest
    {
        public TransactWriteItemsRequest(params TransactWriteItem[] transactItems)
        {
            TransactItems = transactItems ?? throw new ArgumentNullException(nameof(transactItems));
        }

        public TransactWriteItem[] TransactItems { get; set; }

        public string? ClientRequestToken { get; set; }

        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

        public ReturnItemCollectionMetrics? ReturnItemCollectionMetrics { get; set; }
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
        public ConditionCheck(
            string tableName, 
            IReadOnlyDictionary<string, DbValue> key, 
            string conditionExpression)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ConditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
        }

        public string ConditionExpression { get; set; }

        public IReadOnlyDictionary<string, DbValue> Key { get; set; }

        public string TableName { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }

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
        public DeleteItem(string tableName, AttributeCollection key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public AttributeCollection Key { get; set; }

        public string TableName { get; set; }

        public string? ConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
    }

    public class PutItem
    {
        public PutItem(string tableName, AttributeCollection item)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public AttributeCollection Item { get; set; }

        public string TableName { get; set; }

        public string? ConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection? ExpressionAttributeValues { get; set; }

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
    }

    public class UpdateItem
    {
        public UpdateItem(
            string tableName, 
            AttributeCollection key, 
            Change[] changes,
            Expression[]? conditions = null)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));

            var attrNames = new Dictionary<string, string>();
            var updateExpression = new UpdateExpression(changes, attrNames, ExpressionAttributeValues);

            UpdateExpression = updateExpression.ToString();

            if (conditions is { Length: > 0 })
            {
                var expression = new DynamoExpression(attrNames, ExpressionAttributeValues);

                expression.AddRange(conditions);

                ConditionExpression = expression.Text;
            }

            if (attrNames.Count > 0)
            {
                ExpressionAttributeNames = attrNames;
            }
        }

        public AttributeCollection Key { get; set; }

        public string TableName { get; set; }

        public string UpdateExpression { get; set; }

        public string? ConditionExpression { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public AttributeCollection ExpressionAttributeValues { get; set; } = new AttributeCollection();

        public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }        
    }
}
