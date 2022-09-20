namespace Amazon.Security.Tests;

public class CredentialScopeTests
{
    [Fact]
    public void Scopes()
    {
        Assert.Equal("20120215/us-east-1/iam/aws4_request", new CredentialScope(
            date    : new DateOnly(2012, 02, 15),
            region  : AwsRegion.USEast1,
            service : AwsService.Iam
        ).ToString());
            
        Assert.Equal("20120215/us-west-2/ec2/aws4_request", new CredentialScope(
            date    : new DateOnly(2012, 02, 15),
            region  : AwsRegion.Get("us-west-2"),
            service : AwsService.Ec2
        ).ToString());
    }
}