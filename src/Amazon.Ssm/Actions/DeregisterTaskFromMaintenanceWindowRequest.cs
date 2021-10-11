using System;

namespace Amazon.Ssm;

public sealed class DeregisterTaskFromMaintenanceWindowRequest
{
    public DeregisterTaskFromMaintenanceWindowRequest(string windowId, string windowTaskId)
    {
        ArgumentNullException.ThrowIfNull(windowId);
        ArgumentNullException.ThrowIfNull(windowTaskId);

        WindowId = windowId;
        WindowTaskId = windowTaskId;
    }

    public string WindowId { get; }

    public string WindowTaskId { get; }
}
