#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    public sealed class HealthCheckConfig
    {
        public string IPAddress { get; init; }

        public int Port { get; init; }

        public string Http { get; init; }

        public string ResourcePath { get; init; }

        public string FullyQualifiedDomainName { get; init; }

        public int RequestInterval { get; init; }

        public int FailureThreshold { get; init; }

        public bool MeasureLatency { get; init; }

        public bool EnableSNI { get; init; }

        [XmlArray]
        [XmlArrayItem("Region")]
        public string[] Regions { get; init; }

        public bool Inverted { get; init; }
    }
}