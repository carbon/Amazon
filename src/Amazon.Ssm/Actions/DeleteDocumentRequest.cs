namespace Amazon.Ssm;

public sealed class DeleteDocumentRequest : ISsmRequest
{
    public DeleteDocumentRequest(string name!!)
    {
        Name = name;
    }

    public string Name { get; }
}