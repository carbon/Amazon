using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class RemoveTagsFromResourceRequest : ISsmRequest
    {
        [Required]
        public string ResourceId { get; set; }

        [Required]
        public string ResourceType { get; set; }

        [Required]
        public string[] TagKeys { get; set; }
    }
}