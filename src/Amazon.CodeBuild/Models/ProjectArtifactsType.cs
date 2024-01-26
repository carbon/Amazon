using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectArtifactsType>))]
public enum ProjectArtifactsType
{
    CODEPIPELINE = 1,
    S3 = 2,
    NO_ARTIFACTS = 3
}