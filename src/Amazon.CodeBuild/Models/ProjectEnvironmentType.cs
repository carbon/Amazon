using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectEnvironmentType>))]
public enum ProjectEnvironmentType
{
    ARM_CONTAINER = 1,
    LINUX_CONTAINER = 2,
    LINUX_GPU_CONTAINER = 3,
    WINDOWS_CONTAINER = 4,
    WINDOWS_SERVER_2019_CONTAINER = 5
}