namespace Amazon.Ec2.Models
{
    public sealed class InstanceType
    {
#nullable disable
        public InstanceType() { }
#nullable enable

        public InstanceType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}