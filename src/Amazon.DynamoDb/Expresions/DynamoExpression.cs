using System;
using System.Text;

using Carbon.Data.Expressions;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    using static ExpressionKind;

    public sealed class DynamoExpression
    {
        private readonly StringBuilder sb = new StringBuilder();

        private int expressionCount = 0;

        public DynamoExpression()
            : this(new JsonObject(), new AttributeCollection())
        { }

        public DynamoExpression(JsonObject attributeNames, AttributeCollection attributeValues)
        {
            AttributeNames = attributeNames;
            AttributeValues = attributeValues;
        }

        public JsonObject AttributeNames { get; }

        public AttributeCollection AttributeValues { get; }

        public bool HasAttributeNames => AttributeNames.Keys.Count > 0;

        public bool HasAttributeValues => AttributeValues.Count > 0;

        public string Text => sb.ToString();

        public void AddRange(Expression[] expressions)
        {
            foreach (Expression expression in expressions)
            {
                Add(expression);
            }
        }

        public void Add(Expression expression)
        {
            if (expressionCount > 0)
            {
                sb.Append(" and ");
            }

            WritePrimary(expression);

            expressionCount++;
        }

        #region Writers

        private void WritePrimary(Expression expression)
        {
            switch (expression)
            {
                case BinaryExpression binary   : WriteBinaryExpression(binary);    break;
                case BetweenExpression between : WriteBetweenExpression(between);  break;
                case FunctionExpression func   : WriteFunctionExpression(func);    break;
                default: throw new Exception("Unexpected primary expression: " + expression.Kind);
            }
        }

        private void WriteBetweenExpression(BetweenExpression e)
        {
            WriteName(e.Expression.Name);

            sb.Append(" between ");

            Write(e.Start);
            sb.Append(" and ");
            Write(e.End);
        }

        public void WriteBinaryExpression(BinaryExpression e)
        {
            Write(e.Left);

            sb.Append(' ');
            sb.Append(GetName(e.Kind));
            sb.Append(' ');

            Write(e.Right);
        }

        private void Write(Expression e)
        {
            switch (e)
            {   
                case Symbol symbol             : WriteName(symbol.Name);            break;
                case BinaryExpression binary   : WriteBinaryExpression(binary);     break;
                case Constant constant         : WriteValue(constant);              break;
                case FunctionExpression func   : WriteFunctionExpression(func);     break;
                case BetweenExpression between : WriteBetweenExpression(between);   break;
                default:
                    throw new Exception("Unexpected expression:" + e.Kind);
            }
        }

        // https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.OperatorsAndFunctions.html
        // attribute_exists(path)
        // attribute_not_exists(path)
        // attribute_type(path, type)
        // begins_with(path, substr)
        // contains(path, operand)
        // size(path)

        private void WriteFunctionExpression(FunctionExpression funcExp)
        {
            switch (funcExp.Name)
            {
                case "isNotNull"  : sb.Append("attribute_exists");     break;
                case "exists"     : sb.Append("attribute_exists");     break;
                case "notExists"  : sb.Append("attribute_not_exists"); break;
                case "isNull"     : sb.Append("attribute_not_exists"); break;
                case "startsWith" : sb.Append("begins_with"); break;
                default           : sb.Append(funcExp.Name); break;
            }

            sb.Append('(');

            var i = 0;

            foreach (Expression e in funcExp.Args)
            {
                if (i != 0) sb.Append(", ");

                Write(e);

                i++;
            }

            sb.Append(')');
        }

        private void WriteName(string name)
        {
            sb.WriteName(name, AttributeNames);
        }

        private void WriteValue(Constant constant)
        {
            var variableName = ":v" + AttributeValues.Count.ToString();

            var convertor = DbValueConverterFactory.Get(constant.Value.GetType());

            AttributeValues[variableName] = convertor.FromObject(constant.Value);

            sb.Append(variableName);
        }

        #endregion

        public static DynamoExpression Conjunction(params Expression[] expressions)
        {
            var result = new DynamoExpression();

            result.AddRange(expressions);

            return result;
        }

        public static DynamoExpression FromExpression(Expression expression)
        {
            var result = new DynamoExpression();

            result.Add(expression);

            return result;
        }

        #region Helpers

        private static string GetName(ExpressionKind kind)
        {
            switch (kind)
            {
                case And      : return "and";
                case AndAlso  : return "and";
                case Or       : return "or";
                case OrElse   : return "or";
                case Not      : return "not";
                case Equal    : return "=";
                case NotEqual : return "<>";
                case Gt       : return ">";
                case Gte      : return ">=";
                case Lt       : return "<";
                case Lte      : return "<=";
            }

            throw new Exception("Unexpected expression:" + kind);
        }

        #endregion
    }

    // attribute_not_exists(#timestamp) or #timestamp = :timestamp
    // Price <= :p
    // (#P between :lo and :hi) and (#PC in (:cat1, :cat2))

    // expr.ExpressionAttributeNames["#timestamp"] = "last-updated";
    // expr.ExpressionAttributeValues[":timestamp"] = lastUpdated;

    // If you define an expression attribute value, 
    // you must use it consistently throughout the entire expression. 
    // Also, you cannot omit the : symbol.


    /*
    Functions: attribute_exists | attribute_not_exists | attribute_type | contains | begins_with | size
    These function names are case-sensitive.
    Comparison operators: = | <> | < | > | <= | >= | BETWEEN | IN
    Logical operators: AND | OR | NOT
    */
}

// Used in filters, queries, scans, and conditional updates
