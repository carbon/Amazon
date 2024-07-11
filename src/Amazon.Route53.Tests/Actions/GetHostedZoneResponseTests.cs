namespace Amazon.Route53.Tests;

public class GetHostedZoneResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = Route53Serializer<GetHostedZoneResponse>.DeserializeXml(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <GetHostedZoneResponse xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
                <HostedZone>
                    <Id>/hostedzone/Z1PA6795UKMFR9</Id>
                    <Name>example.com.</Name>
                    <CallerReference>2017-03-01T11:22:14Z</CallerReference>
                    <Config>
                        <Comment>This is my first hosted zone.</Comment>
                        <PrivateZone>false</PrivateZone>
                    </Config>
                    <ResourceRecordSetCount>17</ResourceRecordSetCount>
                </HostedZone>
                <DelegationSet>
                    <NameServers>
                        <NameServer>ns-2048.awsdns-64.com</NameServer>
                        <NameServer>ns-2049.awsdns-65.net</NameServer>
                        <NameServer>ns-2050.awsdns-66.org</NameServer>
                        <NameServer>ns-2051.awsdns-67.co.uk</NameServer>
                    </NameServers>
                </DelegationSet>
            </GetHostedZoneResponse>
            """u8.ToArray());

        var hostedZone = response.HostedZone;

        Assert.Equal("/hostedzone/Z1PA6795UKMFR9",    hostedZone.Id);
        Assert.Equal("example.com.",                  hostedZone.Name);
        Assert.Equal("2017-03-01T11:22:14Z",          hostedZone.CallerReference);
        Assert.Equal(17,                              hostedZone.ResourceRecordSetCount);

        Assert.NotNull(hostedZone.Config);
        Assert.Equal("This is my first hosted zone.", hostedZone.Config.Comment);
        Assert.False(hostedZone.Config.PrivateZone);

        Assert.Equal(4, response.DelegationSet.NameServers.Length);
        Assert.Equal("ns-2048.awsdns-64.com", response.DelegationSet.NameServers[0]);
        Assert.Equal("ns-2049.awsdns-65.net", response.DelegationSet.NameServers[1]);
    }
}