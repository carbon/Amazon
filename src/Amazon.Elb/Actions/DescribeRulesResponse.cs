using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class DescribeRulesResponse : IElbResponse
    {
        [XmlElement]
        public DescribeRulesResult DescribeRulesResult { get; set; }
    }

    public class DescribeRulesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Rule[] Rules { get; set; }
    }
}

/*
<DescribeRulesResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
  <DescribeRulesResult> 
    <Rules> 
      <member> 
        <IsDefault>false</IsDefault> 
        <Conditions> 
          <member> 
            <Field>path-pattern</Field> 
            <Values> 
              <member>/img/*</member> 
            </Values> 
          </member> 
        </Conditions> 
        <Priority>10</Priority> 
        <Actions> 
          <member> 
            <Type>forward</Type> 
            <TargetGroupArn>arn:aws:elasticloadbalancing:ua-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067</TargetGroupArn> 
          </member> 
        </Actions> 
        <RuleArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:listener-rule/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2/9683b2d02a6cabee</RuleArn> 
      </member> 
    </Rules> 
  </DescribeRulesResult> 
  <ResponseMetadata> 
    <RequestId>74926cf3-f3a3-11e5-b543-9f2c3fbb9bee</RequestId> 
  </ResponseMetadata>
</DescribeRulesResponse>
*/