#nullable disable

namespace Amazon.Ssm;

public sealed class DeregisterTargetFromMaintenanceWindowRequest
{
    public string WindowId { get; set; }

    public string WindowTargetId { get; set; }
}