#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class ListTagsForResourceRequest : ISsmRequest
{
    [Required]
    public string ResourceId { get; set; }

    // ManagedInstance | MaintenanceWindow | Parameter
    [Required]
    public string ResourceType { get; set; }
}