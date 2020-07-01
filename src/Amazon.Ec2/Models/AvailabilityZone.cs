#nullable disable

namespace Amazon.Ec2
{
    public sealed class AvailabilityZone
    {
        public string RegionName { get; set; }
        
        public string ZoneName { get; set; }

        public string ZoneState { get; set; }
    }
}