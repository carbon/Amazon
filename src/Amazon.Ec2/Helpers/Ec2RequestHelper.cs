using System;
using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.Ec2
{
    internal static class Ec2RequestHelper
    {
        public static Dictionary<string, string> ToParams(string actionName, object instance)
        {
            #region Preconditions

            if (actionName == null)
                throw new ArgumentNullException(nameof(actionName));

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            #endregion

            var parameters = new Dictionary<string, string> {
                { "Action", actionName }
            };

            foreach (var member in JsonObject.FromObject(instance))
            {
                if (member.Value is XNull) continue;

                if (member.Value is JsonArray arr)
                {
                    AddArray(parameters, member.Key, arr);
                }
                else if (member.Value is JsonObject obj)
                {
                    AddObject(parameters, member.Key, obj);
                }
                else
                {
                    parameters.Add(member.Key, member.Value.ToString());
                }
            }

            return parameters;
        }

        private static void AddArray(Dictionary<string, string> parameters, string prefix, JsonArray array)
        {
            for (var i = 0; i < array.Count; i++)
            {
                var key = prefix + "." + (i + 1);

                var element = array[i];

                if (element is JsonObject obj)
                {
                    AddObject(parameters, key, obj);
                }
                else
                {
                    parameters.Add(key, element.ToString());
                }
            }
        }

        private static void AddObject(Dictionary<string, string> parameters, string prefix, JsonObject instance)
        {
            if (parameters.Count > 100) throw new System.Exception("excedeeded max of 100 parameters");

            foreach (var m in instance)
            {
                if (m.Value is XNull) continue;

                var key = prefix + "." + m.Key;

                if (m.Value is JsonObject obj)
                {
                    AddObject(parameters, key, obj);
                }
                else if (m.Value is JsonArray arr)
                {
                    AddArray(parameters, key, arr);
                }
                else
                {
                    parameters.Add(key, m.Value.ToString());
                }
            }
        }
    }
}
