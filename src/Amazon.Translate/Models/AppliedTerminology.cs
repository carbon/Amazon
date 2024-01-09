namespace Amazon.Translate;

public sealed class AppliedTerminology
{
    public required string Name { get; init; }

    public required Term[] Terms { get; init; }
}

public sealed class TranslationSettings
{
}