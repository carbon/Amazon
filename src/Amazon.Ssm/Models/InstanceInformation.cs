namespace Amazon.Ssm
{
    public class InstanceInformation
    {
        public string ActivationId { get; set; }

        public string AgentVersion { get; set; }
        
        public string AssociationStatus { get; set; }

        public string ComputerName { get; set; }

        public string InstanceId { get; set; }

        public string IamRole { get; set; }

        public bool IsLatestVersion { get; set; }

        public string LastAssociationExecutionDate { get; set; }

        public string Name { get; set; }

        public PingStatus PingStatus { get; set; }

        public string PlatformName { get; set; }

        public PlatformType PlatformType { get; set; }

        public string PlatformVersion { get; set; }

        public string RegistrationDate { get; set; }

        // ManagedInstance | Document | EC2Instance
        public string ResourceType { get; set; }
    }

    public enum PlatformType
    {
        Windows,
        Linux
    }

    public enum PingStatus
    {
        Online,
        ConnectionLost,
        Inactive
    }
}
