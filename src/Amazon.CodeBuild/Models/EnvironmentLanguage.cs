#nullable disable

namespace Amazon.CodeBuild;

public sealed class EnvironmentLanguage
{
    public EnvironmentImage[] Images { get; init; }

    public Language Language { get; init; }
}
