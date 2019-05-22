#nullable disable


namespace Amazon.Route53
{
    public class AliasTarget
    {
        public string DNSName { get; set; }

        public bool EvaluateTargetHealth { get; set; }

        public string HostedZoneId { get; set; }
    }
}
