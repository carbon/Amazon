#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    [XmlRoot(Namespace = Route53Client.Namespace)]
    public class ChangeResourceRecordSetsRequest
    {
        public ChangeResourceRecordSetsRequest() { }

        public ChangeResourceRecordSetsRequest(params ResourceRecordSetChange[] changes)
        {
            ChangeBatch = new ChangeBatch(changes);
        }

        [XmlElement]
        public ChangeBatch ChangeBatch { get; set; }
    }

    public class ChangeBatch
    {
        public ChangeBatch() { }

        public ChangeBatch(params ResourceRecordSetChange[] changes)
        {
            Changes = changes;
        }

        [XmlArrayItem("Change")]
        public ResourceRecordSetChange[] Changes { get; set; }
    }

    public class ResourceRecordSetChange
    {
        public ResourceRecordSetChange() { }

        public ResourceRecordSetChange(ChangeAction action, ResourceRecordSet resourceRecordSet)
        {
            Action = action;
            ResourceRecordSet = resourceRecordSet;
        }

        public ResourceRecordSetChange(ChangeAction action, ResourceRecordType type, string name, string value, int ttl)
        {
            Action = action;
            ResourceRecordSet = new ResourceRecordSet(type, name, new ResourceRecord(value)) { TTL = ttl };
        }

        public ChangeAction Action { get; set; }

        public ResourceRecordSet ResourceRecordSet { get; set; }

    }

    public enum ChangeAction : byte
    {
        CREATE = 1,
        DELETE = 2,
        UPSERT = 3
    }

    public enum Failover : byte
    {
        None = 0,
        PRIMARY = 1,
        SECONDARY = 2
    }

   

}


/*
<ChangeResourceRecordSetsRequest xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
   <ChangeBatch>
      <Changes>
         <Change>
            <Action>string</Action>
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
         </Change>
      </Changes>
      <Comment>string</Comment>
   </ChangeBatch>
</ChangeResourceRecordSetsRequest>
*/