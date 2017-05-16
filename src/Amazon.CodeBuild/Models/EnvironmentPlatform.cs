namespace Amazon.CodeBuild
{
    public class EnvironmentPlatform
    {
        public EnvironmentLanguage[] Languages { get; set; }

        // DEBIAN | AMAZON_LINUX | UBUNTU
        public string Platform { get; set; }
    }
}
