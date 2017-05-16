namespace Amazon.Ssm
{
    public class GetMaintenanceWindowExecutionRequest : ISsmRequest
    {
        public string WindowExecutionId { get; set; }
    }
}