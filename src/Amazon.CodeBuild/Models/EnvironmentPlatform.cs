namespace Amazon.CodeBuild
{
    public class EnvironmentPlatform
    {
        /// <summary>
        /// The list of programming languages that are available for the specified platform.
        /// </summary>
        public EnvironmentLanguage[] Languages { get; set; }

        // DEBIAN | AMAZON_LINUX | UBUNTU
        public string Platform { get; set; }
    }
}
