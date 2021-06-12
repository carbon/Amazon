using Xunit;

namespace Amazon.Ec2.Tests
{
    public class DescribeSecurityGroupsTest
    {
        [Fact]
        public void CanParse()
        {
            var text =
@"<DescribeSecurityGroupsResponse xmlns=""http://ec2.amazonaws.com/doc/2016-11-15/"">
    <requestId>1d62eae0-acdd-481d-88c9-example</requestId>
    <securityGroupInfo>
        <item>
            <ownerId>123456789012</ownerId>
            <groupId>sg-9bf6ceff</groupId>
            <groupName>SSHAccess</groupName>
            <groupDescription>Security group for SSH access</groupDescription>
            <vpcId>vpc-31896b55</vpcId>
            <ipPermissions>
                <item>
                    <ipProtocol>tcp</ipProtocol>
                    <fromPort>22</fromPort>
                    <toPort>22</toPort>
                    <groups/>
                    <ipRanges>
                        <item>
                            <cidrIp>0.0.0.0/0</cidrIp>
                        </item>
                    </ipRanges>
                    <ipv6Ranges>
                        <item>
                            <cidrIpv6>::/0</cidrIpv6>
                        </item>
                    </ipv6Ranges>
                    <prefixListIds/>
                </item>
            </ipPermissions>
            <ipPermissionsEgress>
                <item>
                    <ipProtocol>-1</ipProtocol>
                    <groups/>
                    <ipRanges>
                        <item>
                            <cidrIp>0.0.0.0/0</cidrIp>
                        </item>
                    </ipRanges>
                    <ipv6Ranges>
                        <item>
                            <cidrIpv6>::/0</cidrIpv6>
                        </item>
                    </ipv6Ranges>
                    <prefixListIds/>
                </item>
            </ipPermissionsEgress>
        </item>
    </securityGroupInfo>
</DescribeSecurityGroupsResponse>";

            var response = Ec2Serializer<DescribeSecurityGroupsResponse>.Deserialize(text);


            Assert.Single(response.SecurityGroups);

            var group = response.SecurityGroups[0];

            Assert.Equal(123456789012L, group.OwnerId);
            Assert.Equal("sg-9bf6ceff", group.GroupId);
            Assert.Equal("SSHAccess", group.GroupName);
            Assert.Equal("Security group for SSH access", group.GroupDescription);

            Assert.Single(group.IpPermissions); 
            Assert.Single(group.IpPermissionsEgress);

            Assert.Equal("tcp", group.IpPermissions[0].IpProtocol);
            Assert.Equal(22,    group.IpPermissions[0].FromPort);
            Assert.Equal("22",  group.IpPermissions[0].ToPort);


        }
    }
}
