#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class EnvironmentImage
    {
        public string Description { get; init; }

        /// <summary>
        /// The name of the docker image
        /// </summary>
        public string Name { get; init; }
    }
}