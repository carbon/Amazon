using System.Net.Http;
using System.Text;
using Amazon.Security;
using Xunit;

namespace Amazon.Route53.Tests
{
    public class ChangeResourceRecordSetsRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var request = new ChangeResourceRecordSetsRequest(new[] {
                new ResourceRecordSetChange {
                    Action = ChangeAction.CREATE,
                    ResourceRecordSet = new ResourceRecordSet {
                        Type = ResourceRecordType.CNAME,
                        AliasTarget = new AliasTarget {
                            DNSName = "bananas"
                        },
                        GeoLocation = new GeoLocation("NA"),
                        HealthCheckId = "1",
                        Name = "name",
                        TTL = 600,
                        SetIdentifier = "set-id"
                    }
                }  
            });

            byte[] bytes = Route53Serializer<ChangeResourceRecordSetsRequest>.SerializeToUtf8Bytes(request);

            Assert.Equal(@"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<ChangeResourceRecordSetsRequest xmlns=""https://route53.amazonaws.com/doc/2013-04-01/"">
  <ChangeBatch>
    <Changes>
      <Change>
        <Action>CREATE</Action>
        <ResourceRecordSet>
          <AliasTarget>
            <DNSName>bananas</DNSName>
            <EvaluateTargetHealth>false</EvaluateTargetHealth>
          </AliasTarget>
          <GeoLocation>
            <ContinentCode>NA</ContinentCode>
          </GeoLocation>
          <HealthCheckId>1</HealthCheckId>
          <Name>name</Name>
          <SetIdentifier>set-id</SetIdentifier>
          <TTL>600</TTL>
          <Type>CNAME</Type>
        </ResourceRecordSet>
      </Change>
    </Changes>
  </ChangeBatch>
</ChangeResourceRecordSetsRequest>", Encoding.UTF8.GetString(bytes));
        }


        [Fact]
        public void Serialize2()
        {
            var request = new ChangeResourceRecordSetsRequest(new[] {
                new ResourceRecordSetChange {
                    Action = ChangeAction.CREATE,
                    ResourceRecordSet = new ResourceRecordSet {
                        Failover = Failover.SECONDARY,
                        TTL = 600,
                        Type = ResourceRecordType.NS
                    }
                }
            });

            byte[] bytes = Route53Serializer<ChangeResourceRecordSetsRequest>.SerializeToUtf8Bytes(request);

            Assert.Equal("9d0d340e8a3623f88f2d9731e37f6f66f43073fb841ffc32a63ddbc237012266", SignerV4.ComputeSHA256(new ByteArrayContent(bytes)));

            Assert.Equal(@"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<ChangeResourceRecordSetsRequest xmlns=""https://route53.amazonaws.com/doc/2013-04-01/"">
  <ChangeBatch>
    <Changes>
      <Change>
        <Action>CREATE</Action>
        <ResourceRecordSet>
          <Failover>SECONDARY</Failover>
          <TTL>600</TTL>
          <Type>NS</Type>
        </ResourceRecordSet>
      </Change>
    </Changes>
  </ChangeBatch>
</ChangeResourceRecordSetsRequest>", Encoding.UTF8.GetString(bytes));
        }


        [Fact]
        public void Serialize3()
        {
            var request = new ChangeResourceRecordSetsRequest(new[] {
                new ResourceRecordSetChange {
                    Action = ChangeAction.CREATE,
                    ResourceRecordSet = new ResourceRecordSet {
                        Failover = Failover.SECONDARY,
                        TTL = 600,
                        Type = ResourceRecordType.NS
                    }
                },
                new ResourceRecordSetChange(ChangeAction.DELETE, new ResourceRecordSet(ResourceRecordType.AAAA, "example.com.")),
                new ResourceRecordSetChange
                {
                    Action = ChangeAction.UPSERT,

                    ResourceRecordSet = new ResourceRecordSet
                    {
                        Failover = Failover.SECONDARY,
                        TTL = 30,
                        Weight = 255,
                        Type = ResourceRecordType.TXT,
                        ResourceRecords = new[]
                        {
                            new ResourceRecord("a"),
                            new ResourceRecord("b"),
                            new ResourceRecord("c")
                        }
                    }
                }
            });

            byte[] bytes = Route53Serializer<ChangeResourceRecordSetsRequest>.SerializeToUtf8Bytes(request);

            var text = Encoding.UTF8.GetString(bytes);

            Assert.Equal(@"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<ChangeResourceRecordSetsRequest xmlns=""https://route53.amazonaws.com/doc/2013-04-01/"">
  <ChangeBatch>
    <Changes>
      <Change>
        <Action>CREATE</Action>
        <ResourceRecordSet>
          <Failover>SECONDARY</Failover>
          <TTL>600</TTL>
          <Type>NS</Type>
        </ResourceRecordSet>
      </Change>
      <Change>
        <Action>DELETE</Action>
        <ResourceRecordSet>
          <Name>example.com.</Name>
          <ResourceRecords />
          <Type>AAAA</Type>
        </ResourceRecordSet>
      </Change>
      <Change>
        <Action>UPSERT</Action>
        <ResourceRecordSet>
          <Failover>SECONDARY</Failover>
          <ResourceRecords>
            <ResourceRecord>
              <Value>a</Value>
            </ResourceRecord>
            <ResourceRecord>
              <Value>b</Value>
            </ResourceRecord>
            <ResourceRecord>
              <Value>c</Value>
            </ResourceRecord>
          </ResourceRecords>
          <TTL>30</TTL>
          <Type>TXT</Type>
          <Weight>255</Weight>
        </ResourceRecordSet>
      </Change>
    </Changes>
  </ChangeBatch>
</ChangeResourceRecordSetsRequest>", Encoding.UTF8.GetString(bytes));
        }
    }
}
