using System.Collections;
using System.Globalization;

using Amazon.Ec2.Serialization;

namespace Amazon.Ec2;

internal static class Ec2RequestSerializer
{
    public static List<KeyValuePair<string, string>> ToParams(string actionName, object instance)
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", actionName)
        };

        var model = InstanceModel.Get(instance.GetType());

        foreach (var member in model.Members)
        {
            var value = member.GetValue(instance);

            if (value is null) continue;

            if (value is IList list)
            {
                AddArray(parameters, member.Name, list);
            }
            else if (Type.GetTypeCode(value.GetType()) is TypeCode.Object)
            {
                AddObject(parameters, member.Name, value);
            }
            else
            {
                parameters.Add(new(member.Name, value.ToString()!));
            }
        }

        return parameters;
    }

    private static void AddArray(List<KeyValuePair<string, string>> parameters, string prefix, IList array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            string key = string.Create(CultureInfo.InvariantCulture, $"{prefix}.{i + 1}");

            object element = array[i]!;

            if (Type.GetTypeCode(element.GetType()) == TypeCode.Object)
            {
                AddObject(parameters, key, element);
            }
            else
            {
                parameters.Add(new(key, element.ToString()!));
            }
        }
    }

    private static void AddObject(List<KeyValuePair<string, string>> parameters, string prefix, object instance)
    {
        if (parameters.Count >= 100)
        {
            throw new ArgumentException("Exceeded parameter limit", nameof(parameters));
        }

        var model = InstanceModel.Get(instance.GetType());

        foreach (var member in model.Members)
        {
            object value = member.GetValue(instance);

            if (value is null) continue;

            var key = $"{prefix}.{member.Name}";

            if (value is IList list)
            {
                AddArray(parameters, key, list);
            }
            else if (Type.GetTypeCode(value.GetType()) == TypeCode.Object)
            {
                AddObject(parameters, key, value);
            }
            else
            {
                parameters.Add(new (key, value.ToString()!));
            }
        }
    }
}