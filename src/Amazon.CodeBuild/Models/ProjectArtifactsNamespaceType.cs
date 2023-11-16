using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectArtifactsNamespaceType>))]
public enum ProjectArtifactsNamespaceType
{
    NONE = 1,
    BUILD_ID = 2
}