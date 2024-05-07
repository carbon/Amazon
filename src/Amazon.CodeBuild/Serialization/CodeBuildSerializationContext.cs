using System.Text.Json.Serialization;

namespace Amazon.CodeBuild.Serialization;

[JsonSerializable(typeof(BatchGetBuildsRequest))]
[JsonSerializable(typeof(BatchGetBuildsResult))]
[JsonSerializable(typeof(BatchGetProjectsRequest))]
[JsonSerializable(typeof(BatchGetProjectsResult))]
[JsonSerializable(typeof(CreateProjectRequest))]
[JsonSerializable(typeof(CreateProjectResult))]
[JsonSerializable(typeof(DeleteProjectRequest))]
[JsonSerializable(typeof(ListBuildsRequest))]
[JsonSerializable(typeof(ListBuildsResult))]
[JsonSerializable(typeof(ListBuildsForProjectRequest))]
[JsonSerializable(typeof(ListBuildsForProjectResult))]
[JsonSerializable(typeof(ListCuratedEnvironmentImagesRequest))]
[JsonSerializable(typeof(ListCuratedEnvironmentImagesResult))]
[JsonSerializable(typeof(ListProjectsRequest))]
[JsonSerializable(typeof(ListProjectsResult))]
[JsonSerializable(typeof(StartBuildRequest))]
[JsonSerializable(typeof(StartBuildResult))]
[JsonSerializable(typeof(StopBuildRequest))]
[JsonSerializable(typeof(StopBuildResult))]
[JsonSerializable(typeof(UpdateProjectRequest))]
[JsonSerializable(typeof(UpdateProjectResult))]
public partial class CodeBuildSerializerContext : JsonSerializerContext
{
}