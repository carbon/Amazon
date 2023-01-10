namespace Amazon.Sts.Tests;

public class AssumeRoleRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new AssumeRoleRequest("arn", "session", TimeSpan.FromMinutes(15));

        var dic = StsRequestHelper.ToParams(request);

        Assert.Equal(4, dic.Count);

        Assert.Equal(new("Action",          "AssumeRole"), dic[0]);
        Assert.Equal(new("DurationSeconds", "900"),        dic[1]);
        Assert.Equal(new("RoleArn",         "arn"),        dic[2]);
        Assert.Equal(new("RoleSessionName", "session"),    dic[3]);
    }
}