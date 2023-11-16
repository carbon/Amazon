using System.Text.Json.Serialization;

namespace Amazon.CodeBuild.Serialization;

[JsonSerializable(typeof(StartBuildResponse))]
public partial class CodeBuildSerializerContext : JsonSerializerContext
{
}