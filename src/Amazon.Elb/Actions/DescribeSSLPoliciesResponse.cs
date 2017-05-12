using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class DescribeSSLPoliciesResponse : IElbResponse
    {
        [XmlElement]
        public DescribeSSLPoliciesResult DescribeSSLPoliciesResult { get; set; }
    }

    public class DescribeSSLPoliciesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public SslPolicy[] SslPolicies { get; set; }
    }
}
