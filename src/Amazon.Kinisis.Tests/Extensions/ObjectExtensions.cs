using System.Text.Json;

namespace Amazon;

internal static class ObjectExtensions
{  
    public static readonly JsonSerializerOptions s_jso_indented = new() {
        WriteIndented = true
    };

    public static string ToIndentedJsonString<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, s_jso_indented);
    }   
}