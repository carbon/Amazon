namespace Amazon.Ses;

public sealed class DeleteIdentityRequest(string identity)
{
    public string Identity { get; } = identity ?? throw new ArgumentNullException(identity);
}
