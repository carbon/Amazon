#nullable disable

namespace Amazon.Route53;

public sealed class HealthCheck
{
    public string Id { get; init; }

    public string CallerReference { get; init; }

    public HealthCheckConfig HealthCheckConfig { get; init; }

    public int HealthCheckVersion { get; init; }
}

/*
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
    <HealthCheckVersion>2<HealthCheckVersion>
</HealthCheck>
*/