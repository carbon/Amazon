using System.Text;

using Amazon.Security;

namespace Amazon.Route53.Tests;

public class ChangeResourceRecordSetsRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new ChangeResourceRecordSetsRequest([
            new ResourceRecordSetChange {
                Action = ChangeAction.CREATE,
                ResourceRecordSet = new ResourceRecordSet {
                    Type = ResourceRecordType.CNAME,
                    AliasTarget = new AliasTarget {
                        DNSName = "bananas",
                        HostedZoneId = "zone-id",
                    },
                    GeoLocation = new GeoLocation {
                        ContinentCode = ContinentCode.NA
                    },
                    HealthCheckId = "1",
                    Name = "name",
                    TTL = 600,
                    SetIdentifier = "set-id"
                }
            }
        ]);

        byte[] bytes = Route53Serializer<ChangeResourceRecordSetsRequest>.SerializeToUtf8Bytes(request);

        Assert.Equal(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <ChangeResourceRecordSetsRequest xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
              <ChangeBatch>
                <Changes>
                  <Change>
                    <Action>CREATE</Action>
                    <ResourceRecordSet>
                      <AliasTarget>
                        <DNSName>bananas</DNSName>
                        <EvaluateTargetHealth>false</EvaluateTargetHealth>
                        <HostedZoneId>zone-id</HostedZoneId>
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
            </ChangeResourceRecordSetsRequest>
            """.ReplaceLineEndings("\n"), Encoding.UTF8.GetString(bytes));
    }

    [Fact]
    public void CanSerialize2()
    {
        var request = new ChangeResourceRecordSetsRequest([
            new ResourceRecordSetChange {
                Action = ChangeAction.CREATE,
                ResourceRecordSet = new ResourceRecordSet {
                    Name = "test",
                    Failover = Failover.SECONDARY,
                    TTL = 600,
                    Type = ResourceRecordType.NS
                }
            }
        ]);

        byte[] bytes = Route53Serializer<ChangeResourceRecordSetsRequest>.SerializeToUtf8Bytes(request);

        Assert.Equal(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <ChangeResourceRecordSetsRequest xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
              <ChangeBatch>
                <Changes>
                  <Change>
                    <Action>CREATE</Action>
                    <ResourceRecordSet>
                      <Failover>SECONDARY</Failover>
                      <Name>test</Name>
                      <TTL>600</TTL>
                      <Type>NS</Type>
                    </ResourceRecordSet>
                  </Change>
                </Changes>
              </ChangeBatch>
            </ChangeResourceRecordSetsRequest>
            """.ReplaceLineEndings("\n"), Encoding.UTF8.GetString(bytes));

        var signature = SignerV4.ComputeSHA256(new ByteArrayContent(bytes));

        Assert.Equal("07af9af48e804e4f87e95a8134fd5ec28deead45311af7187466732c14c0be89", signature);
    }

    [Fact]
    public void Serialize3()
    {
        var request = new ChangeResourceRecordSetsRequest([
            new ResourceRecordSetChange {
                Action = ChangeAction.CREATE,
                ResourceRecordSet = new ResourceRecordSet {
                    Name = "test",
                    Failover = Failover.SECONDARY,
                    TTL = 600,
                    Type = ResourceRecordType.NS
                }
            },
            new ResourceRecordSetChange(ChangeAction.DELETE, new ResourceRecordSet(ResourceRecordType.AAAA, "example.com.")),
            new ResourceRecordSetChange {
                Action = ChangeAction.UPSERT,
                ResourceRecordSet = new ResourceRecordSet {
                    Name = "test",
                    Failover = Failover.SECONDARY,
                    TTL = 30,
                    Weight = 255,
                    Type = ResourceRecordType.TXT,
                    ResourceRecords = [
                        new ResourceRecord("a"),
                        new ResourceRecord("b"),
                        new ResourceRecord("c")
                    ]
                }
            }
        ]);

        byte[] bytes = Route53Serializer<ChangeResourceRecordSetsRequest>.SerializeToUtf8Bytes(request);

        Assert.Equal(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <ChangeResourceRecordSetsRequest xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
              <ChangeBatch>
                <Changes>
                  <Change>
                    <Action>CREATE</Action>
                    <ResourceRecordSet>
                      <Failover>SECONDARY</Failover>
                      <Name>test</Name>
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
                      <Name>test</Name>
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
            </ChangeResourceRecordSetsRequest>
            """.ReplaceLineEndings("\n"), Encoding.UTF8.GetString(bytes));
    }
}