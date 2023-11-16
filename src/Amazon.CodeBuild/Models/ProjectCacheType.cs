using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectCacheType>))]
public enum ProjectCacheType
{
    /// <summary>
    /// The build project does not use any cache.
    /// </summary>
    NO_CACHE,

    /// <summary>
    /// The build project reads and writes from and to S3.
    /// </summary>
    S3,

    /// <summary>
    /// The build project stores a cache locally on a build host that is only available to that build host.
    /// </summary>
    LOCAL
}