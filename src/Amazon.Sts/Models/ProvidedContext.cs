namespace Amazon.Sts;

public sealed class ProvidedContext
{
    public string? ContextAssertion { get; init; }

    public string? ProviderArn { get; init; }
}