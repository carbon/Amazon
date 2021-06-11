#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class ProjectCache
    {
        // NO_CACHE | S3
        public string Type { get; init; }

        public string Location { get; init; }
    }
}