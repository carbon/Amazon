using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Carbon.Json;

namespace Amazon.Elb
{
    public static class RequestHelper
    {
        

        

        public static Dictionary<string, string> ToParams(object instance)
        {
            var parameters = new Dictionary<string, string>();

            foreach (var member in JsonObject.FromObject(instance))
            {
                if (member.Value is XNull) continue;

                if (member.Value is JsonArray array)
                {
                    for (var i = 0; i < array.Count; i++)
                    {
                        var prefix = member.Key + ".member." + (i + 1);

                        var element = array[i];

                        if (element is JsonObject obj)
                        {
                            AddParameters(parameters, prefix, obj);
                        }
                        else
                        {
                            parameters.Add(prefix, element.ToString());
                        }
                    }
                }
                else if (member.Value is JsonObject obj)
                {
                    AddParameters(parameters, member.Key, obj);
                }
                else
                {
                    parameters.Add(member.Key, member.Value.ToString());
                }
            }

            return parameters;
        }

        private static void AddParameters(Dictionary<string, string> parameters, string prefix, JsonObject instance)
        {
            if (parameters.Count > 100) throw new System.Exception("excedeeded max of 100 parameters");

            foreach (var m in instance)
            {
                if (m.Value is XNull) continue;

                var key = prefix + "." + m.Key;

                if (m.Value is JsonObject obj)
                {
                    AddParameters(parameters, key, obj);
                }
                else
                {
                    parameters.Add(key, m.Value.ToString());
                }
            }
        }
    }
}
