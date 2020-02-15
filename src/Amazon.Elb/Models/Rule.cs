#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class Rule
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Action[] Actions { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public RuleCondition[] Conditions { get; set; }

        [XmlElement]
        public bool IsDefault { get; set; }

        [XmlElement]
        public string Priority { get; set; }

        [XmlElement]
        public string RuleArn { get; set; }
    }
}

/*
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

*/