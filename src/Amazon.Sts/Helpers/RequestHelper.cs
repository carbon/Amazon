using System.Text.Json;

namespace Amazon.Sts;

internal static class StsRequestHelper
{
    public static List<KeyValuePair<string, string>> ToParams<T>(T instance)
       where T : IStsRequest
    {
        using var doc = JsonSerializer.SerializeToDocument(instance);

        var parameters = new List<KeyValuePair<string, string>>(4);

        parameters.Add(new("Action", instance.Action));

        foreach (var member in doc.RootElement.EnumerateObject())
        {
            if (member.Value.ValueKind is JsonValueKind.Null) continue;

            var value = member.Value.ToString()!;

            parameters.Add(new (member.Name, value));
        }

        return parameters;
    }
}