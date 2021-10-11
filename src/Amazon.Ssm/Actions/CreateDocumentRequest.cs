#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class CreateDocumentRequest : ISsmRequest
{
    public string Content { get; set; }

    // Command | Policy | Automation
    public string DocumentType { get; set; }

    [Required]
    public string Name { get; set; }
}