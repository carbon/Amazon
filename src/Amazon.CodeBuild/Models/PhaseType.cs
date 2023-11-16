using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<PhaseType>))]
public enum PhaseType
{
    SUBMITTED,
    QUEUED,
    PROVISIONING,
    DOWNLOAD_SOURCE,
    INSTALL,
    PRE_BUILD,
    BUILD,
    POST_BUILD,
    UPLOAD_ARTIFACTS,
    FINALIZING,
    COMPLETED
}