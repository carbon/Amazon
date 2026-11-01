namespace Amazon.Rds.Tests;

public class ModelTests
{
    [Fact]
    public void Request_DefaultsToFifteenMinutes()
    {
        var request = new GetAuthenticationTokenRequest("db.example.com", 5432, "jane_doe");

        Assert.Equal("db.example.com", request.HostName);
        Assert.Equal(5432, request.Port);
        Assert.Equal("jane_doe", request.UserName);
        Assert.Equal(TimeSpan.FromMinutes(15), request.Expires);
    }

    [Fact]
    public void Request_KeepsExplicitExpiry()
    {
        var request = new GetAuthenticationTokenRequest("db.example.com", 5432, "jane_doe", TimeSpan.FromMinutes(1));

        Assert.Equal(TimeSpan.FromMinutes(1), request.Expires);
    }

    [Fact]
    public void Request_RejectsMissingHostNameAndUserName()
    {
        Assert.Throws<ArgumentNullException>(() => new GetAuthenticationTokenRequest(null!, 5432, "jane_doe"));
        Assert.Throws<ArgumentException>(() => new GetAuthenticationTokenRequest("", 5432, "jane_doe"));
        Assert.Throws<ArgumentNullException>(() => new GetAuthenticationTokenRequest("db.example.com", 5432, null!));
        Assert.Throws<ArgumentException>(() => new GetAuthenticationTokenRequest("db.example.com", 5432, ""));
    }

    [Fact]
    public void AuthenticationToken_ExposesValues()
    {
        var issued = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var expires = issued.AddMinutes(15);

        var token = new AuthenticationToken("host:3306/?Action=connect", issued, expires);

        Assert.Equal("host:3306/?Action=connect", token.Value);
        Assert.Equal(issued, token.Issued);
        Assert.Equal(expires, token.Expires);
    }

    [Fact]
    public void AuthenticationToken_RejectsMissingValue()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthenticationToken(null!, DateTime.UtcNow, DateTime.UtcNow));
        Assert.Throws<ArgumentException>(() => new AuthenticationToken("", DateTime.UtcNow, DateTime.UtcNow));
    }

    [Fact]
    public void UserArn_MatchesDocumentedFormat()
    {
        // arn:aws:rds-db:region:account-id:dbuser:DbiResourceId/db-user-name
        var arn = new UserArn(new AwsRegion("us-east-2"), "123456789012", "db-12ABC34DEFG5HIJ6KLMNOP78QR", "jane_doe");

        Assert.Equal("arn:aws:rds-db:us-east-2:123456789012:dbuser:db-12ABC34DEFG5HIJ6KLMNOP78QR/jane_doe", arn.ToString());
    }
}
