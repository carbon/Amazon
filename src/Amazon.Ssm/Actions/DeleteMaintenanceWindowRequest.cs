using System;

namespace Amazon.Ssm
{
    public sealed class DeleteMaintenanceWindowRequest : ISsmRequest
    {
        public DeleteMaintenanceWindowRequest(string windowId)
        {
            this.WindowId = windowId ?? throw new ArgumentNullException(nameof(windowId));
        }

        public string WindowId { get; }
    }
}