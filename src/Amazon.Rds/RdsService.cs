using System.Globalization;
using System.Net.Http;

using Amazon.Security;

namespace Amazon.Rds;

public sealed class RdsService(AwsRegion region, IAwsCredential credential)
{
    private readonly AwsRegion _region = region ?? throw new ArgumentNullException(nameof(region));
    private readonly IAwsCredential _credential = credential ?? throw new ArgumentNullException(nameof(credential));

    public async Task<AuthenticationToken> GetAuthenticationTokenAsync(GetAuthenticationTokenRequest request)
    {
        // Ensure the underlying credential is renewed
        if (_credential.ShouldRenew)
        {
            await _credential.RenewAsync().ConfigureAwait(false);
        }

        var date = DateTime.UtcNow;

        var scope = new CredentialScope(DateOnly.FromDateTime(date), _region, AwsService.RdsDb);

        var requestUri = new Uri($"https://{request.HostName}:{request.Port}?Action=connect&DBUser={request.UserName}");

        var url = new Uri(SignerV4.GetPresignedUrl(_credential, scope, date, request.Expires, HttpMethod.Get, requestUri));

        return new AuthenticationToken(
            value   : string.Create(CultureInfo.InvariantCulture, $"{url.Host}:{url.Port}/{url.Query}"),
            issued  : date,
            expires : date + request.Expires
        );
    }

    public AuthenticationToken GetAuthenticationToken(GetAuthenticationTokenRequest request)
    {
        return GetAuthenticationTokenAsync(request).GetAwaiter().GetResult();
    }
}

// http://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/UsingWithRDS.IAMDBAuth.html