using System;
using System.Net.Http;

using Amazon.Security;

namespace Amazon.Rds
{
    public class RdsService
    {
        private readonly AwsRegion region;
        private readonly IAwsCredential credential;

        public RdsService(AwsRegion region, IAwsCredential credential)
        {
            this.region     = region     ?? throw new ArgumentNullException(nameof(region));
            this.credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        public AuthenticationToken GenerateAuthenticationToken(GetAuthenticationTokenRequest request)
        {
            var date = DateTime.UtcNow;

            var scope = new CredentialScope(date, AwsRegion.USEast1, AwsService.RdsDb);

            // TODO: Avoid this allocation by extending presign...

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get, 
                $"https://{request.HostName}:{request.Port}?Action=connect&DBUser={request.UserName}"
            );
            
            // Ensure the underlying credential are current
            if (credential.ShouldRenew)
            {
                credential.RenewAsync().Wait();
            }

            SignerV4.Default.Presign(credential, scope, date, TimeSpan.FromMinutes(15), httpRequest);

            var url = httpRequest.RequestUri;

            return new AuthenticationToken(
                value   : url.Host + ":" + url.Port.ToString() + "/" + url.Query,
                issued  : date,
                expires : date.AddMinutes(15)
            );
        }
    }
}

// http://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/UsingWithRDS.IAMDBAuth.html