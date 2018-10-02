using Carbon.Data;
using Carbon.Json;
using System;

using System.Collections.Generic;

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

                // Action is Replace By Default
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

        internal static string DataOpToAction(DataOperation op)
        {
            switch (op)
            {
                case DataOperation.Add      : return "ADD";
                case DataOperation.Replace  : return "PUT";
                case DataOperation.Remove   : return "DELETE";

                default: throw new Exception("Invalid DataOperation: " + op.ToString());
            }
        }
    }
}
