#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class DescribeSSLPoliciesResponse : IElbResponse
    {
        [XmlElement]
        public DescribeSSLPoliciesResult DescribeSSLPoliciesResult { get; init; }
    }

    public sealed class DescribeSSLPoliciesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public SslPolicy[] SslPolicies { get; init; }
    }
}
