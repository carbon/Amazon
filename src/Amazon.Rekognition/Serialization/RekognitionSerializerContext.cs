using System.Text.Json.Serialization;

namespace Amazon.Rekognition.Serialization;

[JsonSerializable(typeof(DetectFacesResult))]
[JsonSerializable(typeof(DetectLabelsResult))]
public partial class RekognitionSerializerContext : JsonSerializerContext
{
}