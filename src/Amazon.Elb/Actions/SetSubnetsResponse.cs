#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class SetSubnetsResponse : IElbResponse
    {
        [XmlElement]
        public SetSubnetsResult SetSubnetsResult { get; set; }
    }

    public sealed class SetSubnetsResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public AvailabilityZone[] AvailabilityZones { get; set; }
    }
}

/*
<SetSubnetsResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
  <SetSubnetsResult> 
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
  </SetSubnetsResult> 
  <ResponseMetadata> 
    <RequestId>c1a80803-f3ab-11e5-b673-8d4a8a9e6f48</RequestId> 
  </ResponseMetadata> 
</SetSubnetsResponse>
*/