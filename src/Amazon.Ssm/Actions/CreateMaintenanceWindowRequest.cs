using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class CreateMaintenanceWindowRequest
{
    public required bool AllowUnassociatedTargets { get; set; }

    [MaxLength(64)]
    public string? ClientToken { get; set; }

    public required int Cutoff { get; set; }

    public required int Duration { get; set; }

    public required string Name { get; set; }

    public required string Schedule { get; set; }
}
