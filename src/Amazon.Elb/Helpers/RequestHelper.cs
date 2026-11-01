using System.Collections;
using System.Globalization;

namespace Amazon.Elb;

public static class RequestHelper
{
    private const int MaxDepth = 8;

    public static List<KeyValuePair<string, string>> ToParams(object instance)
    {
        var parameters = new List<KeyValuePair<string, string>>(4);

        AddMembers(parameters, null, instance, 0);

        return parameters;
    }

    private static void AddMembers(List<KeyValuePair<string, string>> parameters, string? prefix, object instance, int depth)
    {
        if (depth > MaxDepth)
        {
            throw new Exception($"exceeded max depth of {MaxDepth}");
        }

        var model = InstanceModel.Get(instance.GetType());

        foreach (var member in model.Members)
        {
            var value = member.GetValue(instance);

            if (value is null) continue;

            string key = prefix is null ? member.Name : $"{prefix}.{member.Name}";

            AddValue(parameters, key, value, depth);
        }
    }

    private static void AddValue(List<KeyValuePair<string, string>> parameters, string key, object value, int depth)
    {
        if (value is IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string elementKey = string.Create(CultureInfo.InvariantCulture, $"{key}.member.{i + 1}");

                AddValue(parameters, elementKey, list[i]!, depth);
            }
        }
        else if (Type.GetTypeCode(value.GetType()) is TypeCode.Object)
        {
            AddMembers(parameters, key, value, depth + 1);
        }
        else
        {
            parameters.Add(new(key, value.ToString()!));
        }
    }
}
