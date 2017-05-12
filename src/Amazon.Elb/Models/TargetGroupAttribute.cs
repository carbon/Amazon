﻿using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class TargetGroupAttribute
    {
        [XmlElement]
        public string Key { get; set; }

        [XmlElement]
        public string Value { get; set; }
    }
}

/*
<DescribeTargetGroupAttributesResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
  <DescribeTargetGroupAttributesResult> 
    <Attributes> 
      <member> 
        <Value>300</Value> 
        <Key>deregistration_delay.timeout_seconds</Key> 
      </member> 
    </Attributes> 
  </DescribeTargetGroupAttributesResult> 
  <ResponseMetadata> 
    <RequestId>54618294-f3a8-11e5-bb98-57195a6eb84a</RequestId> 
  </ResponseMetadata> 
</DescribeTargetGroupAttributesResponse>
*/