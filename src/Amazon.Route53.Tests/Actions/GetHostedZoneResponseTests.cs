using Xunit;

namespace Amazon.Route53.Tests
{
    public class GetHostedZoneResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<GetHostedZoneResponse xmlns=""https://route53.amazonaws.com/doc/2013-04-01/"">
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
</GetHostedZoneResponse>";

            var result = Route53Serializer<GetHostedZoneResponse>.DeserializeXml(text);

            Assert.Equal("/hostedzone/Z1PA6795UKMFR9", result.HostedZone.Id);
            Assert.Equal("example.com.", result.HostedZone.Name);
            Assert.Equal("2017-03-01T11:22:14Z", result.HostedZone.CallerReference);
            Assert.Equal(17, result.HostedZone.ResourceRecordSetCount);
            Assert.Equal("This is my first hosted zone.", result.HostedZone.Config.Comment);
            Assert.False(result.HostedZone.Config.PrivateZone);

            Assert.Equal(4, result.DelegationSet.NameServers.Length);
            Assert.Equal("ns-2048.awsdns-64.com", result.DelegationSet.NameServers[0]);
            Assert.Equal("ns-2049.awsdns-65.net", result.DelegationSet.NameServers[1]);

        }
    }
}
