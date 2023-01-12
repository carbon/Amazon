namespace Amazon.Ssm;

public sealed class DeleteDocumentRequest : ISsmRequest
{
    public DeleteDocumentRequest(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }

    public string Name { get; }
}