using System.Collections;
using System.Globalization;

namespace Amazon.Ec2;

internal static class Ec2RequestHelper
{
    public static Dictionary<string, string> ToParams(string actionName, object instance)
    {
        var parameters = new Dictionary<string, string> {
            { "Action", actionName }
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
            else if (Type.GetTypeCode(value.GetType()) == TypeCode.Object)
            {
                AddObject(parameters, member.Name, value);
            }
            else
            {
                parameters.Add(member.Name, value.ToString()!);
            }
        }

        return parameters;
    }

    private static void AddArray(Dictionary<string, string> parameters, string prefix, IList array)
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
                parameters.Add(key, element.ToString()!);
            }
        }
    }

    private static void AddObject(Dictionary<string, string> parameters, string prefix, object instance)
    {
        if (parameters.Count > 100)
        {
            throw new ArgumentException("Exceeded 100 arg limit", nameof(parameters));
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
                parameters.Add(key, value.ToString()!);
            }
        }
    }
}
