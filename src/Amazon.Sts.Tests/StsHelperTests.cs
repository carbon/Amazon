namespace Amazon.Sts.Tests;

public class StsHelperTests
{
    [Fact]
    public void A()
    {
        var request = new GetCallerIdentityRequest();

        var dic = StsRequestHelper.ToParams(request);

        Assert.Single(dic);

        Assert.Equal("GetCallerIdentity", dic["Action"]);
    }

    [Fact]
    public void B()
    {
        var request = new GetFederationTokenRequest("name", durationSeconds: 30);

        var dic = StsRequestHelper.ToParams(request);

        Assert.Equal(3, dic.Count);

        Assert.Equal("30", dic["DurationSeconds"]);
    }
}