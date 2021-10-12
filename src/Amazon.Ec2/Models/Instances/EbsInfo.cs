#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class EbsInfo
{
    [XmlElement("ebsOptimizedSupport")]
    public string EbsOptimizedSupport { get; init; }

    [XmlElement("encryptionSupport")]
    public string EncryptionSupport { get; init; }
}
