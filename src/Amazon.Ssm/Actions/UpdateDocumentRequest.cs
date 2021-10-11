#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class UpdateDocumentRequest : ISsmRequest
{
    [Required]
    public string Content { get; set; }

#nullable enable

    public string? DocumentVersion { get; set; }

#nullable disable

    [Required]
    public string Name { get; set; }
}
