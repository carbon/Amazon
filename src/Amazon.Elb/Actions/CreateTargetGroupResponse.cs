#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class CreateTargetGroupResponse : IElbResponse
    {
        public CreateTargetGroupResult CreateTargetGroupResult { get; set; }
    }

    public class CreateTargetGroupResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetGroup[] TargetGroups { get; set; }
    }
}

/*
<CreateTargetGroupResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
  <CreateTargetGroupResult> 
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
        <UnhealthyThresholdCount>2</UnhealthyThresholdCount> 
      </member> 
    </TargetGroups> 
  </CreateTargetGroupResult> 
  <ResponseMetadata> 
    <RequestId>b83fe90e-f2d5-11e5-b95d-3b2c1831fc26</RequestId> 
  </ResponseMetadata>
</CreateTargetGroupResponse>
*/