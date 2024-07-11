namespace Amazon.Route53.Tests;

public class ListResourceRecordSetResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = Route53Serializer<ListResourceRecordSetsResponse>.DeserializeXml(
            """
            <ListResourceRecordSetsResponse xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
              <ResourceRecordSets>
                <ResourceRecordSet>
                  <Name>example.com.</Name>
                  <Type>SOA</Type>
                  <TTL>900</TTL>
                  <ResourceRecords>
                    <ResourceRecord>
                      <Value>ns-2048.awsdns-64.net. hostmaster.awsdns.com. 1 7200 900 1209600 86400</Value>
                    </ResourceRecord>
                  </ResourceRecords>
                </ResourceRecordSet>
              </ResourceRecordSets>
              <IsTruncated>true</IsTruncated>
              <MaxItems>1</MaxItems>
              <NextRecordName>example.com.</NextRecordName>
              <NextRecordType>NS</NextRecordType>
            </ListResourceRecordSetsResponse>
            """u8.ToArray());

        var recordSet = response.ResourceRecordSets[0];

        Assert.Equal("example.com.", recordSet.Name);
        Assert.Equal(ResourceRecordType.SOA, recordSet.Type);
        Assert.Equal(900, recordSet.TTL);

        Assert.NotNull(recordSet.ResourceRecords);
        Assert.Single(recordSet.ResourceRecords);

        Assert.Equal("ns-2048.awsdns-64.net. hostmaster.awsdns.com. 1 7200 900 1209600 86400", recordSet.ResourceRecords[0].Value);

        Assert.True(response.IsTruncated);
        Assert.Equal(1, response.MaxItems);
        Assert.Equal("example.com.", response.NextRecordName);
        Assert.Equal("NS", response.NextRecordType);
    }
}
