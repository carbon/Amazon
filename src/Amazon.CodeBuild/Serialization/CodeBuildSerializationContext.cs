using System.Text.Json.Serialization;

namespace Amazon.CodeBuild.Serialization;

[JsonSerializable(typeof(BatchGetBuildsResult))]
[JsonSerializable(typeof(BatchGetProjectsResult))]
[JsonSerializable(typeof(CreateProjectRequest))]
[JsonSerializable(typeof(CreateProjectResult))]
[JsonSerializable(typeof(ListBuildsResult))]
[JsonSerializable(typeof(ListBuildsForProjectResult))]
[JsonSerializable(typeof(ListCuratedEnvironmentImagesResult))]
[JsonSerializable(typeof(ListProjectsResult))]
[JsonSerializable(typeof(StartBuildResult))]
[JsonSerializable(typeof(StopBuildResult))]
[JsonSerializable(typeof(UpdateProjectResult))]
public partial class CodeBuildSerializerContext : JsonSerializerContext
{
}