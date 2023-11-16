using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectSourceType>))]
public enum ProjectSourceType
{
    BITBUCKET,
    CODECOMMIT,
    CODEPIPELINE,
    GITHUB,
    GITHUB_ENTERPRISE,
    NO_SOURCE,
    S3
}