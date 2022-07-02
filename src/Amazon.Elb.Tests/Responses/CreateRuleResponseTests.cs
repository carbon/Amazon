namespace Amazon.Elb.Tests;

public class CreateRuleResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        // test data from https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_CreateRule.html

        var response = ElbSerializer<CreateRuleResponse>.DeserializeXml(
            """
            <CreateRuleResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
                <CreateRuleResult> 
                    <Rules> 
                        <member> 
                        <IsDefault>false</IsDefault> 
                        <Conditions> 
                            <member> 
                            <Field>path-pattern</Field> 
                            <Values> 
                                <member>/img/*</member> 
                                <member>/vid/*</member> 
                            </Values> 
                            </member> 
                        </Conditions> 
                        <Priority>10</Priority> 
                        <Actions> 
                            <member> 
                            <Type>forward</Type> 
                            <TargetGroupArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067</TargetGroupArn> 
                            </member> 
                        </Actions> 
                        <RuleArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:listener-rule/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2/9683b2d02a6cabee</RuleArn> 
                        </member> 
                    </Rules> 
                </CreateRuleResult> 
                <ResponseMetadata> 
                    <RequestId>c5478c83-f397-11e5-bb98-57195a6eb84a</RequestId> 
                </ResponseMetadata>
            </CreateRuleResponse>
            """);

        Assert.Single(response.CreateRuleResult.Rules);

        var rule = response.CreateRuleResult.Rules[0];

        Assert.Single(rule.Conditions);
        Assert.Equal("path-pattern", rule.Conditions[0].Field);
        Assert.Equal("/img/*", rule.Conditions[0].Values[0]);
        Assert.Equal("/vid/*", rule.Conditions[0].Values[1]);

        Assert.Equal(10, rule.Priority);
        Assert.Equal("forward", rule.Actions[0].Type);
        Assert.Equal("arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067", rule.Actions[0].TargetGroupArn);
    }
}