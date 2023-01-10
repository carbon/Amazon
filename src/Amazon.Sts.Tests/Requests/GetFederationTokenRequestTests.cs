namespace Amazon.Sts.Requests.Tests;

public class GetFederationTokenRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new GetFederationTokenRequest("name", durationSeconds: 30);

        var dic = StsRequestHelper.ToParams(request);

        Assert.Equal(3, dic.Count);

        Assert.Equal(new("Action", "GetFederationToken"), dic[0]);
        Assert.Equal(new("DurationSeconds", "30"), dic[1]);
        Assert.Equal(new("Name", "name"), dic[2]);
    }
}