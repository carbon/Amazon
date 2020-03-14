﻿using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.Metadata
{
    internal sealed class InstanceMetadataService
    {
        public static readonly InstanceMetadataService Instance = new InstanceMetadataService();

        private const string baseMetadataUri = "http://169.254.169.254/latest/meta-data";

        private readonly HttpClient httpClient = new HttpClient {
            Timeout = TimeSpan.FromSeconds(5)
        };

        private MetadataToken? token;

        public async Task<MetadataToken> GetTokenAsync(TimeSpan lifetime)
        {
            if (lifetime < TimeSpan.Zero || lifetime > TimeSpan.FromHours(6))
            {
                throw new ArgumentException("Must be > 0 & less than 6 hours", nameof(lifetime));
            }

            var request = new HttpRequestMessage(HttpMethod.Put, "http://169.254.169.254/latest/api/token") {
                Headers = { { "X-aws-ec2-metadata-token-ttl-seconds", ((int)lifetime.TotalSeconds).ToString(CultureInfo.InvariantCulture) } }
            };

            using HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new MetadataToken(responseText, DateTime.UtcNow + lifetime);
        }

        // If Amazon EC2 is not preparing to stop or terminate the instance, 
        // or if you terminated the instance yourself, instance-action is 
        // not present and you receive an HTTP 404 error.

        public async Task<InstanceAction?> GetSpotInstanceActionOrDefaultAsync()
        {
            string? value = await GetStringOrDefaultAsync(baseMetadataUri + "/spot/instance-action").ConfigureAwait(false);

            return value != null 
                ? JsonSerializer.Deserialize<InstanceAction>(value)
                : null;
        }
      
        public async Task<IamSecurityCredentials> GetIamSecurityCredentials(string roleName)
        {
            string requestUri = baseMetadataUri + "/iam/security-credentials/" + roleName;

            Exception? lastException = null;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using HttpResponseMessage response = await GetAsync(requestUri).ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Invalid response getting /iam/security-credentials. " + response.StatusCode);
                    }
                    
                    using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    return await JsonSerializer.DeserializeAsync<IamSecurityCredentials>(responseStream).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    token = null;

                    lastException = ex;
                }
            }

            throw new Exception("error getting security credentials", lastException);
        }

        private static readonly JsonSerializerOptions camelCasePolicy = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<InstanceIdentity> GetInstanceIdentityAsync()
        {
            using var response = await GetAsync("http://169.254.169.254/latest/dynamic/instance-identity/document").ConfigureAwait(false);

            using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<InstanceIdentity>(responseStream, camelCasePolicy).ConfigureAwait(false);
        }

        // us-east-1a
        public Task<string> GetAvailabilityZoneAsync()
        {
            return GetStringAsync(baseMetadataUri + "/placement/availability-zone");
        }

        public async Task<string?> GetIamRoleNameAsync()
        {
            // TODO: Limit to first line...

            return await GetStringOrDefaultAsync(baseMetadataUri + "/iam/security-credentials/").ConfigureAwait(false);
        }

        public Task<string> GetInstanceIdAsync()
        {
            return GetStringAsync(baseMetadataUri + "/instance-id");
        }

        public async Task<IPAddress?> GetPublicIpAsync()
        {
            string? result = await GetStringOrDefaultAsync(baseMetadataUri + "/public-ipv4").ConfigureAwait(false);

            if (result is null || result.Length == 0) return null;

            return IPAddress.Parse(result);
        }

        public async Task<byte[]> GetUserDataAsync()
        {
            using var response = await GetAsync(baseMetadataUri + "/user-data").ConfigureAwait(false);

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        public Task<string?> GetUserDataStringAsync()
        {
            return GetStringOrDefaultAsync(baseMetadataUri + "/user-data");
        }

        public async ValueTask<MetadataToken> GetTokenAsync()
        {
            // does not expire within 5 minutes
            if (token != null && token.Expires > DateTime.UtcNow.AddMinutes(5))
            {
                return token;
            }

            token = await GetTokenAsync(TimeSpan.FromHours(1)).ConfigureAwait(false);

            return token;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        
            MetadataToken token = await GetTokenAsync().ConfigureAwait(false);

            request.Headers.Add("X-aws-ec2-metadata-token", token.Value);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            return response;
        }

        public async Task<string?> GetStringOrDefaultAsync(string url)
        {
            using var response = await GetAsync(url).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<string> GetStringAsync(string url)
        {
            using var response = await GetAsync(url).ConfigureAwait(false);

    
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}