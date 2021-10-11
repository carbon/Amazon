#nullable disable

namespace Amazon.Ssm;

public sealed class GetMaintenanceWindowResponse
{
    public bool AllowUnassociatedTargets { get; init; }

    public long CreatedDate { get; init; }

    public int Cutoff { get; init; }

    public int Duration { get; init; }

    public bool Enabled { get; init; }

    public long ModifiedDate { get; init; }

    public string Name { get; init; }

    public string Schedule { get; init; }

    public string WindowId { get; init; }
}