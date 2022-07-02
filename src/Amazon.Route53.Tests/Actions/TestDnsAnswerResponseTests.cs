namespace Amazon.Route53.Tests;

public class TestDnsAnswerResponseTests
{
    [Fact]
    public void Deserialize()
    {
        var result = Route53Serializer<TestDnsAnswerResponse>.DeserializeXml("""
            <TestDnsAnswerResponse xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
                <Nameserver>ns-2048.awsdns-64.com</Nameserver>
                <RecordName>www.example.com</RecordName>
                <RecordType>A</RecordType>
                <RecordData>
                    <RecordDataEntry>198.51.100.222</RecordDataEntry>
                </RecordData>
                <ResponseCode>NOERROR</ResponseCode>
                <Protocol>UDP</Protocol>
            </TestDnsAnswerResponse>
            """);

        Assert.Equal("A", result.RecordType);
        Assert.Equal("UDP", result.Protocol);
    }
}