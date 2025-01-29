using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

[JsonConverter(typeof(JsonStringEnumConverter<Modality>))]
public enum Modality
{
    Text      = 1,
    Image     = 2,
    Video     = 3,
    Embedding = 4
}