using System.Globalization;
using System.Text;

namespace Amazon.DynamoDb;

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

    public static void WriteValue(this StringBuilder sb, object value, AttributeCollection attributes)
    {
        string varName = string.Create(CultureInfo.InvariantCulture, $":v{attributes.Count}");

        var convertor = DbValueConverterFactory.Get(value.GetType());

        attributes[varName] = convertor.FromObject(value, null!);

        sb.Append(varName);
    }
}
