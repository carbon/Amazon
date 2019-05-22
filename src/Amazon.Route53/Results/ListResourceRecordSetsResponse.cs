#nullable disable


using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class ListResourceRecordSetsResponse
    {
        public bool IsTruncated { get; set; }

        public int MaxItems { get; set; }

        public string NextRecordIdentifier { get; set; }

        public string NextRecordName { get; set; }

        [XmlElement]
        public string NextRecordType { get; set; }

        [XmlArrayItem("ResourceRecordSet")]
        public ResourceRecordSet[] ResourceRecordSets { get; set; }
    }


}

/*
<ListResourceRecordSetsResponse>
   <IsTruncated>boolean</IsTruncated>
   <MaxItems>string</MaxItems>
   <NextRecordIdentifier>string</NextRecordIdentifier>
   <NextRecordName>string</NextRecordName>
   <NextRecordType>string</NextRecordType>
   <ResourceRecordSets>
      <ResourceRecordSet>
         <AliasTarget>
            <DNSName>string</DNSName>
            <EvaluateTargetHealth>boolean</EvaluateTargetHealth>
            <HostedZoneId>string</HostedZoneId>
         </AliasTarget>
         <Failover>string</Failover>
         <GeoLocation>
            <ContinentCode>string</ContinentCode>
            <CountryCode>string</CountryCode>
            <SubdivisionCode>string</SubdivisionCode>
         </GeoLocation>
         <HealthCheckId>string</HealthCheckId>
         <MultiValueAnswer>boolean</MultiValueAnswer>
         <Name>string</Name>
         <Region>string</Region>
         <ResourceRecords>
            <ResourceRecord>
               <Value>string</Value>
            </ResourceRecord>
         </ResourceRecords>
         <SetIdentifier>string</SetIdentifier>
         <TrafficPolicyInstanceId>string</TrafficPolicyInstanceId>
         <TTL>long</TTL>
         <Type>string</Type>
         <Weight>long</Weight>
      </ResourceRecordSet>
   </ResourceRecordSets>
</ListResourceRecordSetsResponse>
*/