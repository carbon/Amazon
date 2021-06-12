#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class DescribeListenersResponse : IElbResponse
    {
       [XmlElement]
       public DescribeListenersResult DescribeListenersResult { get; init; }
    }

    public sealed class DescribeListenersResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Listener[] Listeners { get; init; }
    }
}


/*
<DescribeListenersResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
  <DescribeListenersResult> 
    <Listeners> 
      <member> 
        <LoadBalancerArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188</LoadBalancerArn> 
        <Protocol>HTTP</Protocol> 
        <Port>80</Port> 
        <ListenerArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:listener/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2</ListenerArn> 
        <DefaultActions> 
          <member> 
            <Type>forward</Type> 
            <TargetGroupArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067</TargetGroupArn> 
          </member> 
        </DefaultActions> 
      </member> 
    </Listeners> 
  </DescribeListenersResult> 
  <ResponseMetadata> 
    <RequestId>18e470d3-f39c-11e5-a53c-67205c0d10fd</RequestId> 
  </ResponseMetadata>
</DescribeListenersResponse>
*/