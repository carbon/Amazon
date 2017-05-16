using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class UpdateDocumentDefaultVersionRequest : ISsmRequest
    {
        [Required]
        public string DocumentVersion { get; set; }

        [Required]
        public string Name { get; set; }
    }
}