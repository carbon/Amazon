using System;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Security;

namespace Amazon.Rds
{
    public sealed class RdsService
    {
        private readonly AwsRegion region;
        private readonly IAwsCredential credential;

        public RdsService(AwsRegion region, IAwsCredential credential)
        {
            this.region     = region     ?? throw new ArgumentNullException(nameof(region));
            this.credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        public async Task<AuthenticationToken> GetAuthenticationTokenAsync(GetAuthenticationTokenRequest request)
        {
            // Ensure the underlying credential is renewed
            if (credential.ShouldRenew)
            {
                await credential.RenewAsync().ConfigureAwait(false);
            }

            var date = DateTime.UtcNow;

            var scope = new CredentialScope(date, region, AwsService.RdsDb);


            Uri requestUri = new Uri($"https://{request.HostName}:{request.Port}?Action=connect&DBUser={request.UserName}"); 

            Uri url = new Uri(SignerV4.GetPresignedUrl(credential, scope, date, request.Expires, HttpMethod.Get, requestUri));

            return new AuthenticationToken(
                value   : url.Host + ":" + url.Port.ToString() + "/" + url.Query,
                issued  : date,
                expires : date + request.Expires
            );
        }

        public AuthenticationToken GetAuthenticationToken(GetAuthenticationTokenRequest request)
        {
            return GetAuthenticationTokenAsync(request).GetAwaiter().GetResult();
        }
    }
}

// http://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/UsingWithRDS.IAMDBAuth.html