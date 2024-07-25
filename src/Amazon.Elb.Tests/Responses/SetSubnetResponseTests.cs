namespace Amazon.Elb.Tests;

public class SetSubnetResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = ElbSerializer<SetSubnetsResponse>.DeserializeXml(
            """
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
            """u8.ToArray());

        var result = response.SetSubnetsResult;

        Assert.Equal("subnet-8360a9e7", result.AvailabilityZones[0].SubnetId);
        Assert.Equal("us-west-2a", result.AvailabilityZones[0].ZoneName);
    }
}
