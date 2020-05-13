#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class UpdateDocumentRequest : ISsmRequest
    {
        [Required]
        public string Content { get; set; }

        public string DocumentVersion { get; set; }

        [Required]
        public string Name { get; set; }
    }
}