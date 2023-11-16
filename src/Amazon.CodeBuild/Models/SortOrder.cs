using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<SortOrder>))]
public enum SortOrder
{
    ASCENDING = 1,
    DESCENDING = 2
}