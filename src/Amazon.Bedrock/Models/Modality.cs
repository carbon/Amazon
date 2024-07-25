using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

[JsonConverter(typeof(JsonStringEnumConverter<Modality>))]
public enum Modality
{
    Text,
    Image,
    Embedding
}