namespace Amazon.Route53.Tests;

public class ChangeResourceRecordSetsResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = Route53Serializer<ChangeResourceRecordSetsResponse>.DeserializeXml(
            """
            <?xml version="1.0" encoding="UTF-8"?>
            <ChangeResourceRecordSetsResponse xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
               <ChangeInfo>
                  <Comment>string</Comment>
                  <Id>string</Id>
                  <Status>string</Status>
               </ChangeInfo>
            </ChangeResourceRecordSetsResponse>
            """u8.ToArray());

        Assert.Equal("string", result.ChangeInfo.Status);
        Assert.Equal("string", result.ChangeInfo.Id);
    }
}