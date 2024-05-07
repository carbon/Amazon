#nullable disable

using System.Xml.Serialization;

namespace Amazon.CloudFront;

[XmlRoot(Namespace = CloudFrontClient.Namespace)]
public sealed class DistributionConfig
{
    [XmlElement]
    public CustomOrigin CustomOrigin { get; set; }

    /// <summary>
    /// The caller reference is a unique value that you provide and CloudFront uses to prevent replays of your request. 
    /// You must provide a new caller reference value and other new information in the request for CloudFront to create a new distribution. 
    /// You could use a time stamp for the caller reference (for example: 20091130090000).
    /// </summary>
    [XmlElement]
    public string CallerReference { get; set; }

    /// <summary>
    /// You can optionally associate one or more CNAME aliases with a distribution so that you can use a domain name of your choice in links to your objects instead of the domain name CloudFront assigns. 
    /// For more information, see Using CNAMEs.
    /// </summary>
    [XmlElement(ElementName = "CNAME")]
    public string CName { get; set; }

    // [StringLength(128)]
    [XmlElement]
    public string Comment { get; set; }

    [XmlElement]
    public string Enabled { get; set; } = "true";
}


/*
<?xml version="1.0" encoding="UTF-8"?>
<DistributionConfig xmlns="http://cloudfront.amazonaws.com/doc/2010-11-01/">
	<CustomOrigin>
		<DNSName>www.example.com</DNSName>
		<HTTPPort>80</HTTPPort>
		<HTTPSPort>443</HTTPSPort>
		<OriginProtocolPolicy>match-viewer</OriginProtocolPolicy>
	</CustomOrigin>
	<CallerReference>20091130090000</CallerReference>
	<CNAME>beagles.com</CNAME>
	<Comment>My comments</Comment>
	<Enabled>true</Enabled>
	<Logging>
		<Bucket>mylogs.s3.amazonaws.com</Bucket>
		<Prefix>myprefix/</Prefix>
	</Logging>
</DistributionConfig>
*/
