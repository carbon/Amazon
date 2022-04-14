namespace Amazon.Ssm;

public sealed class DeleteMaintenanceWindowRequest : ISsmRequest
{
    public DeleteMaintenanceWindowRequest(string windowId!!)
    {
        WindowId = windowId;
    }

    public string WindowId { get; }
}