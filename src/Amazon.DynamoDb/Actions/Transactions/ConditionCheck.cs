using System;
using System.Collections.Generic;

using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Transactions
{
    public sealed class ConditionCheck
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
}