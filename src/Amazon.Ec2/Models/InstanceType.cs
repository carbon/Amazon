namespace Amazon.Ec2.Models
{
    public sealed class InstanceType
    {
        public InstanceType() { }

        public InstanceType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}