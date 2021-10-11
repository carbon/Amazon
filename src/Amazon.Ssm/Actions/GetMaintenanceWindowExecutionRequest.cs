#nullable disable

namespace Amazon.Ssm;

public sealed class GetMaintenanceWindowExecutionRequest : ISsmRequest
{
    public string WindowExecutionId { get; set; }
}
