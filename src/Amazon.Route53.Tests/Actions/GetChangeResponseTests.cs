namespace Amazon.Route53.Tests;

public class GetChangeResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = Route53Serializer<GetChangeResponse>.DeserializeXml(
            """
            <GetChangeResponse xmlns="https://route53.amazonaws.com/doc/2013-04-01/">
                <ChangeInfo>
                    <Id>1</Id>
                    <Status>INSYNC</Status>
                    <SubmittedAt>2017-03-10T01:36:41.958Z</SubmittedAt>
                </ChangeInfo>
            </GetChangeResponse>
            """u8.ToArray());

        Assert.Equal("1", result.ChangeInfo.Id);
        Assert.Equal("INSYNC", result.ChangeInfo.Status);
        Assert.Equal(2017, result.ChangeInfo.SubmittedAt.Year);
    }
}