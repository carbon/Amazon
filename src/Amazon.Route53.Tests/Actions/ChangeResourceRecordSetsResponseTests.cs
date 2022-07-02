namespace Amazon.Route53.Tests;

public class ChangeResourceRecordSetsResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = Route53Serializer<ChangeResourceRecordSetsResponse>.DeserializeXml(
            $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <ChangeResourceRecordSetsResponse xmlns="{Route53Client.Namespace}">
               <ChangeInfo>
                  <Comment>string</Comment>
                  <Id>string</Id>
                  <Status>string</Status>
               </ChangeInfo>
            </ChangeResourceRecordSetsResponse>
            """);

        Assert.Equal("string", result.ChangeInfo.Status);
        Assert.Equal("string", result.ChangeInfo.Id);
    }
}