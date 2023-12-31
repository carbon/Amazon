using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleResult
{
    [XmlElement]
    public required Credentials Credentials { get; init; }

    [XmlElement]
    public required AssumedRoleUser AssumedRoleUser { get; init; }

    /// <summary>
    /// The source identity specified by the principal that is calling the AssumeRole operation.
    /// </summary>
    [XmlElement]
    public string? SourceIdentity { get; init; }

    [XmlElement]
    public int PackedPolicySize { get; init; }
}