#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class RemoveTagsFromResourceRequest : ISsmRequest
{
    [Required]
    public string ResourceId { get; init; }

    [Required]
    public string ResourceType { get; init; }

    [Required]
    public string[] TagKeys { get; init; }
}