#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class CreateMaintenanceWindowRequest
{
    [Required]
    public bool AllowUnassociatedTargets { get; set; }

    public string ClientToken { get; set; }

    [Required]
    public int Cutoff { get; set; }

    [Required]
    public int Duration { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Schedule { get; set; }
}
