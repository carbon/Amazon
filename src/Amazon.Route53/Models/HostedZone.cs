#nullable disable


namespace Amazon.Route53
{
    public class HostedZone
    {
        public string CallerReference { get; set; }

        public HostedZoneConfig Config { get; set; }

        public string Id { get; set; }

        public LinkedService LinkedService { get; set; }

        public string Name { get; set; }

        public long ResourceRecordSetCount { get; set; }
    }
}
