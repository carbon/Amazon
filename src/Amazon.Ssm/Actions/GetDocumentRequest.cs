#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class GetDocumentRequest : ISsmRequest
    {
        public string DocumentVersion { get; set; }

        [Required]
        public string Name { get; set; }
    }
}