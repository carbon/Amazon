#nullable disable

namespace Amazon.CodeBuild;

public sealed class EnvironmentLanguage
{
    public EnvironmentImage[] Images { get; init; }

    // JAVA | PYTHON | NODE_JS | RUBY | GOLANG | DOCKER | ANDROID | BASE
    public string Language { get; init; }
}