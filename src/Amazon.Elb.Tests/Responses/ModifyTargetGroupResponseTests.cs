namespace Amazon.Elb.Tests;

public class ModifyTargetGroupResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        // test data from https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_ModifyTargetGroup.html

        var response = ElbSerializer<ModifyTargetGroupResponse>.DeserializeXml(
            """
            <ModifyTargetGroupResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <ModifyTargetGroupResult> 
                <TargetGroups> 
                  <member> 
                    <TargetGroupArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-https-targets/2453ed029918f21f</TargetGroupArn> 
                    <HealthCheckTimeoutSeconds>5</HealthCheckTimeoutSeconds> 
                    <HealthCheckPort>443</HealthCheckPort> 
                    <Matcher> 
                      <HttpCode>200</HttpCode> 
                    </Matcher> 
                    <TargetGroupName>my-https-targets</TargetGroupName> 
                    <HealthCheckProtocol>HTTPS</HealthCheckProtocol> 
                    <Protocol>HTTPS</Protocol> 
                    <Port>443</Port> 
                    <HealthyThresholdCount>5</HealthyThresholdCount> 
                    <VpcId>vpc-3ac0fb5f</VpcId> 
                    <HealthCheckIntervalSeconds>30</HealthCheckIntervalSeconds> 
                    <LoadBalancerArns> 
                      <member>arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188</member> 
                    </LoadBalancerArns> 
                    <UnhealthyThresholdCount>2</UnhealthyThresholdCount> 
                  </member> 
                </TargetGroups> 
              </ModifyTargetGroupResult> 
              <ResponseMetadata> 
                <RequestId>8525b334-f466-11e5-aa04-33bf366f62e2</RequestId> 
              </ResponseMetadata> 
            </ModifyTargetGroupResponse>
            """);

        var tg_0 = response.ModifyTargetGroupResult.TargetGroups[0];

        Assert.Equal("arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-https-targets/2453ed029918f21f", tg_0.TargetGroupArn);
        Assert.Equal("my-https-targets", tg_0.TargetGroupName);

        Assert.Equal(5,     tg_0.HealthCheckTimeoutSeconds);
        Assert.Equal("443", tg_0.HealthCheckPort);

        Assert.Equal("200", tg_0.Matcher.HttpCode);

        Assert.Equal("HTTPS", tg_0.Protocol);
        Assert.Equal(443, tg_0.Port);
        Assert.Equal("vpc-3ac0fb5f", tg_0.VpcId);
        Assert.Equal(2, tg_0.UnhealthyThresholdCount);

        Assert.Equal(30, tg_0.HealthCheckIntervalSeconds);
    }
}