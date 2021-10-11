using System;

namespace Amazon.Ssm;

public sealed class DeleteDocumentRequest : ISsmRequest
{
    public DeleteDocumentRequest(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    public string Name { get; }
}
