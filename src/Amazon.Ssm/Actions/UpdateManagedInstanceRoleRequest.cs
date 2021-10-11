#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class UpdateManagedInstanceRoleRequest : ISsmRequest
{
    [Required]
    public string IamRole { get; set; }

    [Required]
    public string InstanceId { get; set; }
}
