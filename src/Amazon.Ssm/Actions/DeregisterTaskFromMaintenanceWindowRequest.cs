namespace Amazon.Ssm
{
    public class DeregisterTaskFromMaintenanceWindowRequest
    {
        public string WindowId { get; set; }

        public string WindowTaskId { get; set; }
    }
}