using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Sts;

internal static class StsRequestHelper
{
    private static readonly JsonSerializerOptions jso = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public static Dictionary<string, string> ToParams<T>(T instance)
       where T : IStsRequest
    {
        using var doc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(instance, jso));

        var parameters = new Dictionary<string, string>(2);

        foreach (var member in doc.RootElement.EnumerateObject())
        {
            parameters.Add(member.Name, member.Value.ToString()!);
        }

        return parameters;
    }
}
