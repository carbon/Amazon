#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class UpdateDocumentDefaultVersionRequest : ISsmRequest
{
    [Required]
    public string DocumentVersion { get; set; }

    [Required]
    public string Name { get; set; }
}
