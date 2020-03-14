using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.CloudFront
{
    [XmlRoot(Namespace = "http://cloudfront.amazonaws.com/doc/2010-11-01/")]
    public sealed class DistributionConfig
    {
        public DistributionConfig()
        {
            this.Enabled = "true";
        }

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
        public string Comment { get; set; }

        public string Enabled { get; set; }

        public XDocument ToXml()
        {
            var serializer = new XmlSerializer(typeof(DistributionConfig));

            var namespaces = new XmlSerializerNamespaces();

            //  Add lib namespace with empty prefix 
            namespaces.Add("", "http://cloudfront.amazonaws.com/doc/2010-11-01/");

            var doc = new XDocument();

            using (var xw = doc.CreateWriter())
            {
                serializer.Serialize(xw, this, namespaces);
            }

            return doc;
        }
    }
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
