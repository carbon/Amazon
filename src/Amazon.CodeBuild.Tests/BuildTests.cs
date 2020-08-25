using System;
using System.Text.Json;

using Xunit;

namespace Amazon.CodeBuild.Tests
{
    public class BuildTests
    {
        private static readonly JsonSerializerOptions jso = new () { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        [Fact]
        public void ParseBuild()
        {
            var text = @"{ ""build"":{ ""arn"":""arn"",""artifacts"":{ ""location"":""ommited""},""buildComplete"":false,""buildStatus"":""IN_PROGRESS"",""currentPhase"":""SUBMITTED"",""environment"":{ ""computeType"":""BUILD_GENERAL1_LARGE"",""environmentVariables"":[],""image"":""microsoft/dotnet:1.1.1-sdk"",""type"":""LINUX_CONTAINER""},""id"":""id"",""initiator"":""initiator"",""projectName"":""projectName"",""source"":{""auth"":{""type"":""OAUTH""},""location"":""location"",""type"":""GITHUB""},""startTime"":1.494895355089E9,""timeoutInMinutes"":60}}";

            var build = JsonSerializer.Deserialize<BuildResponse>(text, jso).Build;

            Assert.Equal("arn", build.Arn);
            Assert.Equal(1494895355089, ((DateTimeOffset)build.StartTime).ToUnixTimeMilliseconds()); // 1.494895355089E9
            Assert.Equal("ommited", build.Artifacts.Location);

            Assert.False(build.BuildComplete);
            Assert.Equal("IN_PROGRESS", build.BuildStatus);
            Assert.Equal("SUBMITTED", build.CurrentPhase);

            Assert.Equal(new DateTime(2017, 05, 16, 00, 42, 35, 89, DateTimeKind.Utc), build.StartTime);
            Assert.Null(build.EndTime);

            Assert.Equal("BUILD_GENERAL1_LARGE", build.Environment.ComputeType);

            Assert.Equal(60, build.TimeoutInMinutes);
        }

        public class BuildResponse
        {
            public Build Build { get; set; }
        }

        [Fact]
        public void ParseBuildsResponse()
        {
            var text = @"{
  ""builds"": [
    {
      ""arn"": ""x"",
      ""artifacts"": {
        ""location"": ""x""
      },
      ""buildComplete"": true,
      ""buildStatus"": ""SUCCEEDED"",
      ""currentPhase"": ""COMPLETED"",
      ""endTime"": 1.495244229972E9,
      ""environment"": {
        ""computeType"": ""BUILD_GENERAL1_LARGE"",
        ""environmentVariables"": [
          {
            ""name"": ""BUILD_ID"",
            ""value"": ""17""
          }
        ],
        ""image"": ""microsoft/dotnet:1.1.1-sdk"",
        ""type"": ""LINUX_CONTAINER""
      },
      ""id"": ""a"",
      ""initiator"": ""a"",
    
      ""phases"": [
        {
          ""durationInSeconds"": 0,
          ""endTime"": 1.495244106372E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""SUBMITTED"",
          ""startTime"": 1.495244105957E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 29,
          ""endTime"": 1.495244135403E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""PROVISIONING"",
          ""startTime"": 1.495244106372E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 23,
          ""endTime"": 1.49524415843E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""DOWNLOAD_SOURCE"",
          ""startTime"": 1.495244135403E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 0,
          ""endTime"": 1.495244158484E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""INSTALL"",
          ""startTime"": 1.49524415843E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 19,
          ""endTime"": 1.495244177951E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""PRE_BUILD"",
          ""startTime"": 1.495244158484E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 31,
          ""endTime"": 1.495244209206E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""BUILD"",
          ""startTime"": 1.495244177951E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 9,
          ""endTime"": 1.495244218245E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""POST_BUILD"",
          ""startTime"": 1.495244209206E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 7,
          ""endTime"": 1.495244225473E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""UPLOAD_ARTIFACTS"",
          ""startTime"": 1.495244218245E9
        },
        {
          ""contexts"": [ ],
          ""durationInSeconds"": 4,
          ""endTime"": 1.495244229972E9,
          ""phaseStatus"": ""SUCCEEDED"",
          ""phaseType"": ""FINALIZING"",
          ""startTime"": 1.495244225473E9
        },
        {
          ""phaseType"": ""COMPLETED"",
          ""startTime"": 1.495244229972E9
        }
      ],
      ""projectName"": ""x"",
      ""source"": {
        ""auth"": {
          ""type"": ""x""
        },
        ""location"": ""x"",
        ""type"": ""x""
      },
      ""sourceVersion"": ""69cfb4c2fbf644cef26f45847282780d19c96dba"",
      ""startTime"": 1.495244105957E9,
      ""timeoutInMinutes"": 60
    }
  ],
  ""buildsNotFound"": [ ]
}";
            var response = JsonSerializer.Deserialize<BatchGetBuildsResponse>(text, jso);

            Assert.Single(response.Builds);

            var build = response.Builds[0];

            var lastPhase = build.Phases[^1];

            Assert.Equal(10, build.Phases.Length);

            Assert.Equal("COMPLETED", lastPhase.PhaseType);
        }
    }
}
