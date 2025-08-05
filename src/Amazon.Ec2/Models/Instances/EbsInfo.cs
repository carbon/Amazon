using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class EbsInfo
{
    [XmlElement("ebsOptimizedSupport")]
    public string? EbsOptimizedSupport { get; init; }

    // unsupported | supported
    [XmlElement("encryptionSupport")]
    public string? EncryptionSupport { get; init; }

    // unsupported | supported
    [XmlElement("nvmeSupport")]
    public string? NvmeSupport { get; init; }
}