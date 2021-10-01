#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class CreateLoadBalancerResponse : IElbResponse
{
    [XmlElement]
    public CreateLoadBalancerResult CreateLoadBalancerResult { get; init; }
}

public sealed class CreateLoadBalancerResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public LoadBalancer[] LoadBalancers { get; init; }
}

/*
<CreateLoadBalancerResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
  <CreateLoadBalancerResult>
    <LoadBalancers> 
      <member> 
        <LoadBalancerArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-internal-load-balancer/5b49b8d4303115c2</LoadBalancerArn> 
        <Scheme>internal</Scheme> 
        <LoadBalancerName>my-internal-load-balancer</LoadBalancerName> 
        <VpcId>vpc-3ac0fb5f</VpcId> 
        <CanonicalHostedZoneId>Z2P70J7EXAMPLE</CanonicalHostedZoneId> 
        <CreatedTime>2016-03-25T21:29:48.850Z</CreatedTime> 
        <AvailabilityZones> 
          <member> 
            <SubnetId>subnet-8360a9e7</SubnetId> 
            <ZoneName>us-west-2a</ZoneName> 
          </member> 
          <member> 
            <SubnetId>subnet-b7d581c0</SubnetId> 
            <ZoneName>us-west-2b</ZoneName> 
          </member> 
        </AvailabilityZones> 
        <SecurityGroups> 
          <member>sg-5943793c</member> 
        </SecurityGroups> 
        <DNSName>internal-my-internal-load-balancer-1529930873.us-west-2.elb.amazonaws.com</DNSName> 
        <State> 
          <Code>provisioning</Code> 
        </State> 
        <Type>application</Type> 
      </member> 
    </LoadBalancers> 
  </CreateLoadBalancerResult> 
  <ResponseMetadata> 
    <RequestId>b37b9c3e-f2d0-11e5-a53c-67205c0d10fd</RequestId> 
  </ResponseMetadata>
</CreateLoadBalancerResponse>
*/