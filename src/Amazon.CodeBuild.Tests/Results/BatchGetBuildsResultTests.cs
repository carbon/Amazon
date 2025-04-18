﻿using System.Text.Json;

using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild.Results.Tests;

public class BatchGetBuildsResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        BatchGetBuildsResult? result = JsonSerializer.Deserialize(
            """
            {
              "builds": [
                {
                  "arn": "arn",
                  "artifacts": {
                    "location": "x"
                  },
                  "buildComplete": true,
                  "buildStatus": "SUCCEEDED",
                  "currentPhase": "COMPLETED",
                  "endTime": 1.495244229972E9,
                  "environment": {
                    "computeType": "BUILD_GENERAL1_LARGE",
                    "environmentVariables": [
                      {
                        "name": "BUILD_ID",
                        "value": "17"
                      }
                    ],
                    "image": "microsoft/dotnet:1.1.1-sdk",
                    "type": "LINUX_CONTAINER"
                  },
                  "id": "a",
                  "initiator": "a",
                  "phases": [
                    {
                      "durationInSeconds": 0,
                      "endTime": 1.495244106372E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "SUBMITTED",
                      "startTime": 1.495244105957E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 29,
                      "endTime": 1.495244135403E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "PROVISIONING",
                      "startTime": 1.495244106372E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 23,
                      "endTime": 1.49524415843E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "DOWNLOAD_SOURCE",
                      "startTime": 1.495244135403E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 0,
                      "endTime": 1.495244158484E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "INSTALL",
                      "startTime": 1.49524415843E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 19,
                      "endTime": 1.495244177951E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "PRE_BUILD",
                      "startTime": 1.495244158484E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 31,
                      "endTime": 1.495244209206E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "BUILD",
                      "startTime": 1.495244177951E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 9,
                      "endTime": 1.495244218245E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "POST_BUILD",
                      "startTime": 1.495244209206E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 7,
                      "endTime": 1.495244225473E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "UPLOAD_ARTIFACTS",
                      "startTime": 1.495244218245E9
                    },
                    {
                      "contexts": [ ],
                      "durationInSeconds": 4,
                      "endTime": 1.495244229972E9,
                      "phaseStatus": "SUCCEEDED",
                      "phaseType": "FINALIZING",
                      "startTime": 1.495244225473E9
                    },
                    {
                      "phaseType": "COMPLETED",
                      "startTime": 1.495244229972E9
                    }
                  ],
                  "projectName": "project-name",
                  "source": {
                    "auth": {
                      "type": "x"
                    },
                    "location": "x",
                    "type": "x"
                  },
                  "sourceVersion": "69cfb4c2fbf644cef26f45847282780d19c96dba",
                  "startTime": 1.495244105957E9,
                  "timeoutInMinutes": 60
                }
              ],
              "buildsNotFound": [ ]
            }
            """u8, CodeBuildSerializerContext.Default.BatchGetBuildsResult);

        Assert.NotNull(result);
        Assert.Single(result.Builds);

        var build = result.Builds[0];

        Assert.Equal("arn", build.Arn);

        // Environment
        Assert.Equal("microsoft/dotnet:1.1.1-sdk", build.Environment.Image);
        Assert.NotNull(build.Environment.EnvironmentVariables);
        Assert.Single(build.Environment.EnvironmentVariables);
        Assert.Equal(ProjectEnvironmentType.LINUX_CONTAINER, build.Environment.Type);

        Assert.Equal("project-name", build.ProjectName);

        // Phases -
        Assert.Equal(10, build.Phases.Length);

        Assert.Equal(PhaseStatus.SUCCEEDED, build.Phases[0].PhaseStatus);
        Assert.Equal(PhaseType.SUBMITTED, build.Phases[0].PhaseType);

        Assert.Equal(PhaseType.COMPLETED, build.Phases[^1].PhaseType);
    }
}