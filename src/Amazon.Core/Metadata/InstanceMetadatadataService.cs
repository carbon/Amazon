using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Amazon.Metadata;

public sealed partial class InstanceMetadataService
{
    public static readonly InstanceMetadataService Instance = new();

    private InstanceMetadataService() { }

    private const string baseMetadataUri = "http://169.254.169.254/latest/meta-data";

    private readonly HttpClient httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    // If Amazon EC2 is not preparing to stop or terminate the instance, 
    // or if you terminated the instance yourself, instance-action is 
    // not present and you receive an HTTP 404 error.

    public async Task<InstanceAction?> GetSpotInstanceActionOrDefaultAsync()
    {
        string? value = await GetStringOrDefaultAsync(baseMetadataUri + "/spot/instance-action").ConfigureAwait(false);

        if (value is null) return null;

        return JsonSerializer.Deserialize<InstanceAction>(value);
    }

    internal async Task<IamSecurityCredentials> GetIamSecurityCredentialsAsync(string roleName)
    {
        string requestUri = $"{baseMetadataUri}/iam/security-credentials/{roleName}";

        Exception? lastException = null;

        for (int i = 0; i < 3; i++)
        {
            try
            {
                using HttpResponseMessage response = await GetAsync(requestUri).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Invalid response getting /iam/security-credentials. {response.StatusCode}");
                }

                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                var result = await JsonSerializer.DeserializeAsync(responseStream, IamJsonContext.Default.IamSecurityCredentials).ConfigureAwait(false);

                return result!;
            }
            catch (Exception ex)
            {
                token = null;

                lastException = ex;
            }
        }

        throw new Exception("error getting security credentials", lastException);
    }

    public async Task<InstanceIdentity> GetInstanceIdentityAsync()
    {
        using var response = await GetAsync("http://169.254.169.254/latest/dynamic/instance-identity/document").ConfigureAwait(false);

        using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        return (await JsonSerializer.DeserializeAsync<InstanceIdentity>(responseStream).ConfigureAwait(false))!;
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

    public async Task<IPAddress?> GetPublicIpv4Async()
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

    private MetadataToken? token;

    private async Task<MetadataToken> GetTokenAsync(TimeSpan lifetime)
    {
        if (lifetime < TimeSpan.Zero || lifetime > TimeSpan.FromHours(6))
        {
            throw new ArgumentException("Must be > 0 & less than 6 hours", nameof(lifetime));
        }

        int lifetimeInSeconds = (int)lifetime.TotalSeconds;

        var request = new HttpRequestMessage(HttpMethod.Put, "http://169.254.169.254/latest/api/token") {
            Headers = {
                { "X-aws-ec2-metadata-token-ttl-seconds", lifetimeInSeconds.ToString(CultureInfo.InvariantCulture) }
            }
        };

        using HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new MetadataToken(responseText, DateTime.UtcNow + lifetime);
    }

    private async Task<MetadataToken> GetTokenAsync()
    {
        // does not expire within 5 minutes
        if (token is not null && token.Expires > DateTime.UtcNow.AddMinutes(5))
        {
            return token;
        }

        token = await GetTokenAsync(TimeSpan.FromHours(1)).ConfigureAwait(false);

        return token;
    }

    private async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        MetadataToken token = await GetTokenAsync().ConfigureAwait(false);

        request.Headers.Add("X-aws-ec2-metadata-token", token.Value);

        HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

        return response;
    }

    private async Task<string?> GetStringOrDefaultAsync(string url)
    {
        using HttpResponseMessage response = await GetAsync(url).ConfigureAwait(false);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    private async Task<string> GetStringAsync(string url)
    {
        using HttpResponseMessage response = await GetAsync(url).ConfigureAwait(false);

        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
}