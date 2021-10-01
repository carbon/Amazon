namespace Amazon.Elb.Tests
{
    public class SetSubnetResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = @"<SetSubnetsResponse xmlns=""http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/"">
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
</SetSubnetsResponse>";

            var r = ElbSerializer<SetSubnetsResponse>.DeserializeXml(text);

            Assert.Equal("subnet-8360a9e7", r.SetSubnetsResult.AvailabilityZones[0].SubnetId);
            Assert.Equal("us-west-2a", r.SetSubnetsResult.AvailabilityZones[0].ZoneName);
        }
    }
}