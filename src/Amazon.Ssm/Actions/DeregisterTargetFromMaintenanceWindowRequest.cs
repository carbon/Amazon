namespace Amazon.Ssm
{
    public class DeregisterTargetFromMaintenanceWindowRequest
    {
        public string WindowId { get; set; }

        public string WindowTargetId { get; set; }
    }
}