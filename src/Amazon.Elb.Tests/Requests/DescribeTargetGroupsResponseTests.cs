namespace Amazon.Elb.Tests;

public class DescribeTargetGroupsResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = ElbSerializer<DescribeTargetGroupsResponse>.DeserializeXml(
            """
            <DescribeTargetGroupsResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <DescribeTargetGroupsResult> 
                <TargetGroups> 
                  <member> 
                    <TargetGroupArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067</TargetGroupArn> 
                    <HealthCheckTimeoutSeconds>5</HealthCheckTimeoutSeconds> 
                    <HealthCheckPort>traffic-port</HealthCheckPort> 
                    <Matcher> 
                      <HttpCode>200</HttpCode> 
                    </Matcher> 
                    <TargetGroupName>my-targets</TargetGroupName> 
                    <HealthCheckProtocol>HTTP</HealthCheckProtocol> 
                    <HealthCheckPath>/</HealthCheckPath> 
                    <Protocol>HTTP</Protocol> 
                    <Port>80</Port> 
                    <VpcId>vpc-3ac0fb5f</VpcId> 
                    <HealthyThresholdCount>5</HealthyThresholdCount> 
                    <HealthCheckIntervalSeconds>30</HealthCheckIntervalSeconds> 
                    <LoadBalancerArns> 
                      <member>arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188</member> 
                    </LoadBalancerArns> 
                    <UnhealthyThresholdCount>2</UnhealthyThresholdCount> 
                  </member> 
                </TargetGroups> 
              </DescribeTargetGroupsResult> 
              <ResponseMetadata> 
                <RequestId>70092c0e-f3a9-11e5-ae48-cff02092876b</RequestId> 
              </ResponseMetadata> 
            </DescribeTargetGroupsResponse>
            """u8.ToArray());

        Assert.Single(response.DescribeTargetGroupsResult.TargetGroups);

        Assert.Equal("200", response.DescribeTargetGroupsResult.TargetGroups[0].Matcher.HttpCode);
    }
}