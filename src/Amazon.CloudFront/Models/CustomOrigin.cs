#nullable disable

using System.Xml.Serialization;

namespace Amazon.CloudFront;

public sealed class CustomOrigin
{
    [XmlElement("DNSName")]
    public string DnsName { get; set; }

    [XmlElement("HTTPPort")]
    public int HttpPort { get; set; } = 80;

    [XmlElement("HTTPSPort")]
    public int HttpsPort { get; set; } = 443;

    [XmlElement("OriginProtocolPolicy")]
    public string OriginProtocolPolicy { get; set; } = "match-viewer";
}