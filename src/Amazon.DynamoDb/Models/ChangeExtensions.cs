using System;
using System.Collections.Generic;

using Carbon.Data;

namespace Amazon.DynamoDb
{
    public static class ChangeExtensions
    {
        public static KeyValuePair<string, B> ToProperty(this in Change change)
        {
            var o = new B();

            if (change.Operation == DataOperation.Replace)
            {
                // Delete Null Values
                if (change.Value is null)
                {
                    o.Action = "DELETE";
                }

                // Default action is Replace
            }
            else
            {
                o.Action = DataOpToAction(change.Operation);
            }

            if (change.Value != null)
            {
                o.Value = new DbValue(change.Value);
            }

            return new KeyValuePair<string, B>(change.Name, o);
        }

        public class B
        {
            public string? Action { get; set; }

            public DbValue? Value { get; set; }
        }

        internal static string DataOpToAction(DataOperation op) => op switch
        {
            DataOperation.Add     => "ADD",
            DataOperation.Replace => "PUT",
            DataOperation.Remove  => "DELETE",
            _                     => throw new Exception("Invalid operation: " + op)
        };       
    }
}