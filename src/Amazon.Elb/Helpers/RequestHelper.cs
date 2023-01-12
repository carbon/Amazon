using System.Collections;
using System.Globalization;

namespace Amazon.Elb;

public static class RequestHelper
{
    public static List<KeyValuePair<string, string>> ToParams(object instance)
    {
        var parameters = new List<KeyValuePair<string, string>>(4);

        var model = InstanceModel.Get(instance.GetType());

        foreach (var member in model.Members)
        {
            var value = member.GetValue(instance);

            if (value is null) continue;

            if (value is IList list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string prefix = string.Create(CultureInfo.InvariantCulture, $"{member.Name}.member.{i + 1}");

                    var element = list[i]!;

                    if (Type.GetTypeCode(element.GetType()) is TypeCode.Object)
                    {
                        AddParameters(parameters, prefix, element);
                    }
                    else
                    {
                        parameters.Add(new(prefix, element.ToString()!));
                    }
                }
            }
            else if (Type.GetTypeCode(value.GetType()) == TypeCode.Object)
            {
                AddParameters(parameters, member.Name, value);
            }
            else
            {
                parameters.Add(new(member.Name, value.ToString()!));
            }
        }

        return parameters;
    }

    private static void AddParameters(List<KeyValuePair<string, string>> parameters, string prefix, object instance)
    {
        if (parameters.Count > 100)
        {
            throw new Exception("exceeded max of 100 parameters");
        }

        var model = InstanceModel.Get(instance.GetType());

        foreach (var m in model.Members)
        {
            var value = m.GetValue(instance);

            if (value is null) continue;

            string key = $"{prefix}.{m.Name}";

            if (Type.GetTypeCode(value.GetType()) is TypeCode.Object)
            {
                AddParameters(parameters, key, value);
            }
            else
            {
                parameters.Add(new (key, value.ToString()!));
            }
        }
    }
}
