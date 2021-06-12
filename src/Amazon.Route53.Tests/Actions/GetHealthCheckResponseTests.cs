using Xunit;

namespace Amazon.Route53.Tests
{

    public class GetHealthCheckResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = @"<GetHealthCheckResponse xmlns=""https://route53.amazonaws.com/doc/2013-04-01/"">
   <HealthCheck>
      <Id>abcdef11-2222-3333-4444-555555fedcba</Id>
      <CallerReference>example.com 192.0.2.17</CallerReference>
      <HealthCheckConfig>
         <IPAddress>192.0.2.17</IPAddress>
         <Port>80</Port>
         <Type>HTTP</Type>
         <ResourcePath>/docs/route-53-health-check.html</ResourcePath>
         <FullyQualifiedDomainName>example.com</FullyQualifiedDomainName>
         <RequestInterval>30</RequestInterval>
         <FailureThreshold>3</FailureThreshold>
         <MeasureLatency>true</MeasureLatency>
         <EnableSNI>true</EnableSNI>
         <Regions>
            <Region>ap-southeast-1</Region>
            <Region>ap-southeast-2</Region>
            <Region>ap-northeast-1</Region>
         </Regions>
         <Inverted>false</Inverted>
      </HealthCheckConfig>
      <HealthCheckVersion>2</HealthCheckVersion>
   </HealthCheck>
</GetHealthCheckResponse>";



            var result = Route53Serializer<GetHealthCheckResponse>.DeserializeXml(text);

            var hcc = result.HealthCheck.HealthCheckConfig;

            Assert.Equal("abcdef11-2222-3333-4444-555555fedcba", result.HealthCheck.Id);
            Assert.Equal(2, result.HealthCheck.HealthCheckVersion);

            Assert.Equal("192.0.2.17",                       hcc.IPAddress);
            Assert.Equal(80,                                 hcc.Port);
            Assert.Equal("/docs/route-53-health-check.html", hcc.ResourcePath);
            Assert.Equal(30,                                 hcc.RequestInterval);
            Assert.Equal(3,                                  hcc.FailureThreshold);

            Assert.Equal("ap-southeast-1", hcc.Regions[0]);
            Assert.Equal("ap-southeast-2", hcc.Regions[1]);

        }
    }
}
