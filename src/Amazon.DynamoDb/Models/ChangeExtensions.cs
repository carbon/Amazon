using System;
using System.Collections.Generic;

using Carbon.Data;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public static class ChangeExtensions
    {
        public static KeyValuePair<string, JsonObject> ToProperty(this in Change change)
        {
            var o = new JsonObject();

            if (change.Operation == DataOperation.Replace)
            {
                // Delete Null Values
                if (change.Value is null)
                {
                    o.Add("Action", "DELETE");
                }

                // Default action is Replace
            }
            else
            {
                o.Add("Action", DataOpToAction(change.Operation));
            }

            if (change.Value != null)
            {
                o.Add("Value", new DbValue(change.Value).ToJson());
            }

            return new KeyValuePair<string, JsonObject>(change.Name, o);
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