using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<ProjectEnvironmentType>))]
public enum ProjectEnvironmentType
{
    ARM_CONTAINER = 1,
    LINUX_CONTAINER = 2,
    LINUX_GPU_CONTAINER = 3,
    WINDOWS_CONTAINER = 4,
    WINDOWS_SERVER_2019_CONTAINER = 5,
    WINDOWS_SERVER_2022_CONTAINER = 6,
    LINUX_LAMBDA_CONTAINER = 7,
    ARM_LAMBDA_CONTAINER = 8,
    LINUX_EC2 = 9,
    ARM_EC2 = 10,
    WINDOWS_EC2 = 11,
    MAC_ARM = 12
}