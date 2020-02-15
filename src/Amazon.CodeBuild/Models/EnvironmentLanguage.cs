#nullable disable

namespace Amazon.CodeBuild
{
    public class EnvironmentLanguage
    {
        public EnvironmentImage[] Images { get; set; }

        // JAVA | PYTHON | NODE_JS | RUBY | GOLANG | DOCKER | ANDROID | BASE
        public string Language { get; set; }
    }
}
