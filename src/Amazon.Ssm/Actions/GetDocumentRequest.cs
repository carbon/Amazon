using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class GetDocumentRequest : ISsmRequest
{
    public string? DocumentVersion { get; init; }

#nullable disable

    [Required]
    public string Name { get; init; }
}
