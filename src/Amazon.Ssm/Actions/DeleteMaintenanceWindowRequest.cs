using System;

namespace Amazon.Ssm;

public sealed class DeleteMaintenanceWindowRequest : ISsmRequest
{
    public DeleteMaintenanceWindowRequest(string windowId)
    {
        ArgumentNullException.ThrowIfNull(windowId);

        WindowId = windowId;
    }

    public string WindowId { get; }
}
