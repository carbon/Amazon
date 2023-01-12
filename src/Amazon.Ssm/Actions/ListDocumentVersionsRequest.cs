namespace Amazon.Ssm;

public sealed class ListDocumentVersionsRequest : ISsmRequest
{
    public ListDocumentVersionsRequest() { }

    public ListDocumentVersionsRequest(string name)
    {
        Name = name;
    }

    public int? MaxResults { get; init; }

    public required string Name { get; init; }

    public string? NextToken { get; init; }
}
