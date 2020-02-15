#nullable disable

namespace Amazon.CodeBuild
{
    public class EnvironmentImage
    {
        public string Description { get; set; }

        /// <summary>
        /// The name of the docker image
        /// </summary>
        public string Name { get; set; }
    }
}
