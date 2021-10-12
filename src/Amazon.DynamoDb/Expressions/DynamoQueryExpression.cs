using Carbon.Data.Expressions;

namespace Amazon.DynamoDb
{
    public sealed class DynamoQueryExpression
    {
        public readonly string[] keyNames;
        
        // TODO: Expand BinaryExpression

        public DynamoQueryExpression(string[] keyNames, Expression[] expressions)
        {
            this.keyNames = keyNames;

            KeyExpression = new DynamoExpression(AttributeNames, AttributeValues);

            foreach (Expression expression in expressions)
            {
                if (expression is BinaryExpression be)
                {
                    if (IsKey(be.Left.ToString()!))
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
                    if (IsKey(between.Expression.Name))
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

        internal bool HasAttributeValues => AttributeValues.Count > 0;

        private void AddFilterExpression(Expression expression)
        {
            FilterExpression ??= new DynamoExpression(AttributeNames, AttributeValues);
            
            FilterExpression.Add(expression);
        }

        public Dictionary<string, string> AttributeNames { get; } = new ();

        public AttributeCollection AttributeValues { get; } = new AttributeCollection();

        public DynamoExpression KeyExpression { get; }

        public DynamoExpression? FilterExpression { get; set; }

        private bool IsKey(string name)
        {
            foreach (string key in keyNames)
            {
                if (name.Equals(key, StringComparison.Ordinal)) return true;
            }

            return false;
        }
    }
}