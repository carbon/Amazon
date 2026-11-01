namespace Amazon.Sts.Tests;

public class StsHelperTests
{
    [Fact]
    public void ToParams_StartsWithAction()
    {
        var request = new GetCallerIdentityRequest();

        var dic = StsRequestHelper.ToParams(request);

        Assert.Single(dic);

        Assert.Equal(new("Action", "GetCallerIdentity"), dic[0]);
    }
}
