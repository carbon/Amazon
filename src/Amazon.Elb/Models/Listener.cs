#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class Listener
{
    [XmlElement]
    public string LoadBalancerArn { get; init; }

    [XmlElement]
    public Protocol Protocol { get; init; }

    [XmlElement]
    public int Port { get; init; }

    [XmlElement]
    public string ListenerArn { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public ListenerAction[] DefaultActions { get; init; }
}

/*
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
*/