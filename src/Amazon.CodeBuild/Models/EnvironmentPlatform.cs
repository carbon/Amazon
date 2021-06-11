#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class EnvironmentPlatform
    {
        /// <summary>
        /// The list of programming languages that are available for the specified platform.
        /// </summary>
        public EnvironmentLanguage[] Languages { get; init; }

        // DEBIAN | AMAZON_LINUX | UBUNTU
        public string Platform { get; init; }
    }
}
