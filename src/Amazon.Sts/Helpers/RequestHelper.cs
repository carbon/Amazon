using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.Sts
{
    public static class StsRequestHelper
    {
        public static Dictionary<string, string> ToParams(IStsRequest instance)
        {
            var parameters = new Dictionary<string, string>();

            foreach (var member in JsonObject.FromObject(instance))
            {
                if (member.Value is XNull) continue;

                /*
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
                */
                /*
                else if (member.Value is JsonObject obj)
                {
                    AddParameters(parameters, member.Key, obj);
                }
                */
                else
                {
                    parameters.Add(member.Key, member.Value.ToString());
                }
            }

            return parameters;
        }
    }
}
