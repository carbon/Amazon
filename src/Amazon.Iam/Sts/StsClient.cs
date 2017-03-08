using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amazon.Sts
{
    using Helpers;
    using Models;

    public class StsClient : AwsClient
    {
        public const string Version = "2011-06-15";

        public StsClient(AwsCredential credential)
            : base(AwsService.Sts, AwsRegion.USEast1, credential)
        { }

        public async Task<AwsSession> GetSessionTokenAsync(TimeSpan duration)
        {
            #region Preconditions

            // 3600s (one hour) to 129600s (36 hours), with 43200s (12 hours) as the default. 
            // Sessions for AWS account owners are restricted to a maximum of 3600s (one hour).

            #endregion

            var request = new StsRequest("GetSessionToken") {
                { "Version", Version },
                { "DurationSeconds", (int)duration.TotalSeconds }
            };

            var requestUri = Endpoint + request.Parameters.ToQueryString();

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var responseText = await SendAsync(httpRequest);

            return GetSessionTokenResponse.Parse(responseText);
        }
    }
}