#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class CreateRuleResponse : IElbResponse
    {
        [XmlElement]
        public CreateRuleResult CreateRuleResult { get; set; }
    }

    public sealed class CreateRuleResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Rule[] Rules { get; set; }
    }

}

/*
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
*/