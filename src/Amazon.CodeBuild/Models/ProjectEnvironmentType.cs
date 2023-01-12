﻿using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectEnvironmentType
{
    WINDOWS_CONTAINER,
    LINUX_CONTAINER,
    LINUX_GPU_CONTAINER,
    ARM_CONTAINER,
    WINDOWS_SERVER_2019_CONTAINER
}