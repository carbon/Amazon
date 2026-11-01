using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class OutputConfig
{
    [JsonPropertyName("textFormat")]
    public required TextFormat TextFormat { get; init; }
}

public sealed class TextFormat
{
    [JsonPropertyName("type")]
    public required OutputFormatType Type { get; init; }

    [JsonPropertyName("structure")]
    public required OutputFormatStructure Structure { get; init; }

    public static implicit operator TextFormat(JsonSchemaDefinition jsonSchema)
    {
        ArgumentNullException.ThrowIfNull(jsonSchema);

        return new TextFormat {
            Type = OutputFormatType.JsonSchema,
            Structure = new OutputFormatStructure { JsonSchema = jsonSchema }
        };
    }
}

public sealed class OutputFormatStructure
{
    [JsonPropertyName("jsonSchema")]
    public required JsonSchemaDefinition JsonSchema { get; init; }

    public static implicit operator OutputFormatStructure(JsonSchemaDefinition jsonSchema)
    {
        ArgumentNullException.ThrowIfNull(jsonSchema);

        return new OutputFormatStructure {
            JsonSchema = jsonSchema
        };
    }
}

public sealed class JsonSchemaDefinition
{
    [JsonPropertyName("schema")]
    public required string Schema { get; init; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }
}
