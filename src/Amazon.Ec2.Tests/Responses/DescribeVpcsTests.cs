namespace Amazon.Ec2.Tests
{
    public class DescribeVpcsResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            var text =

@"<DescribeVpcsResponse xmlns=""http://ec2.amazonaws.com/doc/2016-11-15/"">
  <requestId>7a62c49f-347e-4fc4-9331-6e8eEXAMPLE</requestId>
  <vpcSet>
    <item>
      <vpcId>vpc-1a2b3c4d</vpcId>
      <state>available</state>
      <cidrBlock>10.0.0.0/23</cidrBlock>
      <ipv6CidrBlockAssociationSet>
        <item>
          <ipv6CidrBlock>2001:db8:1234:1a00::/56</ipv6CidrBlock>
          <associationId>vpc-cidr-assoc-abababab</associationId>
          <ipv6CidrBlockState>
             <state>ASSOCIATED</state>
          </ipv6CidrBlockState>
        </item>
      </ipv6CidrBlockAssociationSet>    
      <dhcpOptionsId>dopt-7a8b9c2d</dhcpOptionsId> 
      <instanceTenancy>default</instanceTenancy>
      <isDefault>false</isDefault>
      <tagSet/>
    </item>
  </vpcSet>
</DescribeVpcsResponse>";

            var response = Ec2Serializer<DescribeVpcsResponse>.Deserialize(text);

            Assert.Single(response.Vpcs);

            var vpc = response.Vpcs[0];

            Assert.Equal("vpc-1a2b3c4d", vpc.VpcId);
            Assert.Equal("available", vpc.State);
            Assert.Equal("10.0.0.0/23", vpc.CidrBlock);
            Assert.Equal("default", vpc.InstanceTenancy);
            Assert.Equal("dopt-7a8b9c2d", vpc.DhcpOptionsId);
            Assert.False(vpc.IsDefault);

            Assert.Equal("2001:db8:1234:1a00::/56", vpc.Ipv6CidrBlockAssociations[0].Ipv6CidrBlock);
            Assert.Equal("vpc-cidr-assoc-abababab", vpc.Ipv6CidrBlockAssociations[0].AssociationId);

        }
    }
}
