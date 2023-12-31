using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetCallerIdentityResult
{
    /// <summary>
    /// The AWS ARN associated with the calling entity.
    /// </summary>
    [XmlElement]
    public required string Arn { get; init; }

    /// <summary>
    /// The AWS account ID number of the account that owns or contains the calling entity.
    /// </summary>
    [XmlElement]
    public required string Account { get; init; }

    /// <summary>
    /// The unique identifier of the calling entity. 
    /// </summary>
    [XmlElement]
    public required string UserId { get; init; }
}