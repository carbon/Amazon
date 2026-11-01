using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Amazon.Sqs.Tests;

public static class JSO
{
    public static readonly JsonSerializerOptions Indented = new() { 
        WriteIndented = true, 
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
    };

    public static string ToIndentedJsonString<T>(this T instance, JsonTypeInfo<T> jsonTypeInfo)
    {
        var utf8 = JsonSerializer.SerializeToUtf8Bytes(instance, jsonTypeInfo);

        var el= JsonElement.Parse(utf8);

        return JsonSerializer.Serialize(el, Indented);
    }

    public static string ToIndentedJsonString<T>(this T instance)
    {
        return JsonSerializer.Serialize(instance, Indented);
    }
}