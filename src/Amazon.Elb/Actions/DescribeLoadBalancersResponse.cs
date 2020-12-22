﻿#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class DescribeLoadBalancersResponse : IElbResponse
    {
        [XmlElement]
        public DescribeLoadBalancersResult DescribeLoadBalancersResult { get; set; }
    }

    public sealed class DescribeLoadBalancersResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public LoadBalancer[] LoadBalancers { get; set; }
    }
}