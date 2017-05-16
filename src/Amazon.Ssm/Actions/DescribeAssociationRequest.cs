namespace Amazon.Ssm
{
    public class DescribeAssociationRequest
    {
        public string AssociationId { get; set; }

        public string InstanceId { get; set; }

        public string Name { get; set; }
    }
}