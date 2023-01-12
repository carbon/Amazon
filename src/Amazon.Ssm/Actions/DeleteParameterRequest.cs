using System.Diagnostics.CodeAnalysis;

namespace Amazon.Ssm;

public sealed class DeleteParameterRequest : ISsmRequest
{
    public DeleteParameterRequest() { }

    [SetsRequiredMembers]
    public DeleteParameterRequest(string name)
    {
        Name = name;
    }

    public required string Name { get; set; }
}
