using Xunit;

namespace Amazon.Ec2.Tests
{
    public class DescribeSubnetsTests
    {
        [Fact]
        public void Deserialize()
        {
            var text =
@"<DescribeSubnetsResponse xmlns=""http://ec2.amazonaws.com/doc/2016-11-15/"">
  <requestId>7a62c49f-347e-4fc4-9331-6e8eEXAMPLE</requestId>
  <subnetSet>
    <item>
      <subnetId>subnet-9d4a7b6c</subnetId>
      <state>available</state>
      <vpcId>vpc-1a2b3c4d</vpcId>
      <cidrBlock>10.0.1.0/24</cidrBlock>
      <ipv6CidrBlockAssociationSet>
        <item>
          <ipv6CidrBlock>2001:db8:1234:1a00::/64</ipv6CidrBlock>
          <associationId>subnet-cidr-assoc-abababab</associationId>
          <ipv6CidrBlockState>
             <state>ASSOCIATED</state>
          </ipv6CidrBlockState>
         </item>
      </ipv6CidrBlockAssociationSet>
      <availableIpAddressCount>251</availableIpAddressCount>
      <availabilityZone>us-east-1a</availabilityZone>
      <defaultForAz>false</defaultForAz>
      <mapPublicIpOnLaunch>false</mapPublicIpOnLaunch>
      <tagSet/>
      <assignIpv6AddressOnCreation>false</assignIpv6AddressOnCreation>
    </item>
    <item>
      <subnetId>subnet-6e7f829e</subnetId>
      <state>available</state>
      <vpcId>vpc-1a2b3c4d></vpcId>
      <cidrBlock>10.0.0.0/24</cidrBlock>
      <ipv6CidrBlockAssociationSet/> 
      <availableIpAddressCount>251</availableIpAddressCount>
      <availabilityZone>us-east-1a</availabilityZone>
      <defaultForAz>false</defaultForAz>
      <mapPublicIpOnLaunch>false</mapPublicIpOnLaunch>
      <assignIpv6AddressOnCreation>false</assignIpv6AddressOnCreation>
    </item>
  </subnetSet>
</DescribeSubnetsResponse>";

            var response = Ec2Serializer<DescribeSubnetsResponse>.Deserialize(text);

            Assert.Equal(2, response.Subnets.Length);

            var subnet = response.Subnets[0];

            Assert.Equal("subnet-9d4a7b6c", subnet.SubnetId);
            Assert.Equal("vpc-1a2b3c4d",    subnet.VpcId);
            Assert.Equal("us-east-1a",      subnet.AvailabilityZone);

            Assert.Equal(251, subnet.AvailableIpAddressCount);
        }
    }
}
