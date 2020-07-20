using System.Collections.Generic;
using System.Text;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal static class DynamoExtensions
    {
        public static void WriteName(this StringBuilder sb, string name, Dictionary<string, string> map)
        {
            if (DynamoKeyword.IsReserved(name))
            {
                string key = "#" + name;

                if (!map.ContainsKey(key))
                {
                    map.Add(key, name);
                }

                sb.Append(key);
            }
            else
            {
                sb.Append(name);
            }
        }

        public static void WriteName(this StringBuilder sb, string name, JsonObject map)
        {
            if (DynamoKeyword.IsReserved(name))
            {
                string key = "#" + name;

                if (!map.ContainsKey(key))
                {
                    map.Add(key, name);
                }

                sb.Append(key);
            }
            else
            {
                sb.Append(name);
            }
        }

        public static void WriteValue(this StringBuilder sb, object value, AttributeCollection attributes)
        {
            string varName = ":v" + attributes.Count.ToString();

            var convertor = DbValueConverterFactory.Get(value.GetType());

            attributes[varName] = convertor.FromObject(value, null!);

            sb.Append(varName);
        }
    }
}