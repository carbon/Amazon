using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class DeleteDocumentRequest : ISsmRequest
    {
        public DeleteDocumentRequest(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
    }
}