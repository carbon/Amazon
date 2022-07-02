#nullable disable

namespace Amazon.Translate;

public sealed class AppliedTerminology
{
    public string Name { get; init; }

    public Term[] Terms { get; init; }
}