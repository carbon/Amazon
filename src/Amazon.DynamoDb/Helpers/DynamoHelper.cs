using System.Text;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal static class DynamoExtensions
    {
        public static void WriteName(this StringBuilder sb, string name, JsonObject map)
        {
            if (DynamoKeyword.IsReserved(name))
            {
                if (!map.ContainsKey("#" + name))
                {
                    map.Add("#" + name, name);
                }

                sb.Append("#" + name);
            }
            else
            {
                sb.Append(name);
            }
        }

        public static void WriteValue(this StringBuilder sb, object value, AttributeCollection attributes)
        {
            var varName = ":v" + attributes.Count;

            var convertor = DbValueConverterFactory.Get(value.GetType());

            attributes[varName] = convertor.FromObject(value, null);

            sb.Append(varName);
        }
    }
}