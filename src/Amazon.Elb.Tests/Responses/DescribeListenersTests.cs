using System.Text;

namespace Amazon.Elb.Tests;

public class DescribeListenersTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = ElbSerializer<DescribeListenersResponse>.DeserializeXml(
            Encoding.UTF8.GetBytes($"""
            <DescribeListenersResponse xmlns="{ElbClient.Namespace}">
              <DescribeListenersResult> 
                <Listeners> 
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
                </Listeners> 
              </DescribeListenersResult> 
              <ResponseMetadata> 
                <RequestId>18e470d3-f39c-11e5-a53c-67205c0d10fd</RequestId> 
              </ResponseMetadata>
            </DescribeListenersResponse>
            """));

        var listeners = response.DescribeListenersResult.Listeners;

        Assert.Single(listeners);

        var arnPrefix = "arn:aws:elasticloadbalancing:us-west-2:123456789012";

        Assert.Equal($"{arnPrefix}:loadbalancer/app/my-load-balancer/50dc6c495c0c9188", listeners[0].LoadBalancerArn);
        Assert.Equal($"{arnPrefix}:listener/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2", listeners[0].ListenerArn);

        Assert.Equal(Protocol.HTTP, listeners[0].Protocol);

        Assert.Single(listeners[0].DefaultActions);
        Assert.Equal("forward", listeners[0].DefaultActions[0].Type);
        Assert.Equal($"{arnPrefix}:targetgroup/my-targets/73e2d6bc24d8a067", listeners[0].DefaultActions[0].TargetGroupArn);
    }
}