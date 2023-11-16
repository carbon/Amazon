using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectArtifactsPackagingType>))]
public enum ProjectArtifactsPackagingType
{
    NONE = 1,
    ZIP = 2
}
