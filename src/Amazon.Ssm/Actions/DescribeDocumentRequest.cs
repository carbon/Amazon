#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class DescribeDocumentRequest : ISsmRequest
{
    public string DocumentVersion { get; init; }

    [Required]
    public string Name { get; init; }
}
