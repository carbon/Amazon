using System.Collections.Generic;
using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class Listener
    {
        [XmlElement]
        public string LoadBalancerArn { get; set; }

        [XmlElement]
        public string Protocol { get; set; }

        [XmlElement]
        public int Port { get; set; }

        [XmlElement]
        public string ListenerArn { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public ListenerAction[] DefaultActions { get; set; }
    }
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