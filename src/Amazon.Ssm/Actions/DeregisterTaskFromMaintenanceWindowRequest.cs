using System;

namespace Amazon.Ssm
{
    public sealed class DeregisterTaskFromMaintenanceWindowRequest
    {
        public DeregisterTaskFromMaintenanceWindowRequest(string windowId, string windowTaskId)
        {
            WindowId = windowId ?? throw new ArgumentNullException(nameof(windowId));
            WindowTaskId = windowTaskId ?? throw new ArgumentNullException(nameof(windowTaskId));
        }

        public string WindowId { get; }

        public string WindowTaskId { get;  }
    }
}