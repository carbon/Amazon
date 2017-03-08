using System;

using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class DynamoQueryExpression
    {
        public readonly string[] keyNames;
        
        // TODO: Expand BinaryExpression

        public DynamoQueryExpression(string[] keyNames, Expression[] expressions)
        {
            this.keyNames = keyNames;

            KeyExpression = new DynamoExpression(AttributeNames, AttributeValues);

            foreach (var expression in expressions)
            {
                if (expression is BinaryExpression be)
                {

                    if (IsKey(be.Left.ToString()))
                    {
                        KeyExpression.Add(be);
                    }
                    else
                    {
                        AddFilterExpression(be);
                    }
                }
                else if (expression is BetweenExpression between)
                {
                    if (IsKey(between.Property.ToString()))
                    {
                        KeyExpression.Add(between);
                    }
                    else
                    {
                        AddFilterExpression(between);
                    }
                }
                else
                {
                    throw new Exception("Unexpected expression type:" + expression);
                }
            }
        }

        public bool HasAttributeNames => AttributeNames.Keys.Count > 0;

        public bool HasAttributeValues => AttributeValues.Count > 0;

        private void AddFilterExpression(Expression expression)
        {
            if (FilterExpression == null)
            {
                FilterExpression = new DynamoExpression(AttributeNames, AttributeValues);
            }

            FilterExpression.Add(expression);
        }

        public JsonObject AttributeNames { get; } = new JsonObject();

        public AttributeCollection AttributeValues { get; } = new AttributeCollection();

        public DynamoExpression KeyExpression { get; }

        public DynamoExpression FilterExpression { get; set; }

        private bool IsKey(string name)
        {
            foreach (var key in keyNames)
            {
                if (name == key) return true;
            }

            return false;
        }
    }

    // attribute_not_exists(#timestamp) or #timestamp = :timestamp
    // Price <= :p
    // (#P between :lo and :hi) and (#PC in (:cat1, :cat2))

    // expr.ExpressionAttributeNames["#timestamp"] = "last-updated";
    // expr.ExpressionAttributeValues[":timestamp"] = lastUpdated;


    // If you define an expression attribute value, you must use it consistently throughout the entire expression. 
    // Also, you cannot omit the : symbol.s
}
