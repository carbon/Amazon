using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Amazon.Metadata;

public sealed partial class InstanceMetadataService
{
    private const string host = "169.254.169.254";
    private const string baseMetadataUri = $"http://{host}/latest/meta-data";
    private static readonly TimeSpan _1hour = TimeSpan.FromHours(1);

    public static readonly InstanceMetadataService Instance = new();

    private InstanceMetadataService() { }

    private readonly HttpClient httpClient = new(new SocketsHttpHandler {
        ConnectTimeout = TimeSpan.FromSeconds(5),
        AllowAutoRedirect = false,
        UseCookies = false
    }) {
        Timeout = TimeSpan.FromSeconds(6)
    };

    // If Amazon EC2 is not preparing to stop or terminate the instance, 
    // or if you terminated the instance yourself, instance-action is 
    // not present and you receive an HTTP 404 error.

    public async Task<InstanceAction?> GetSpotInstanceActionOrDefaultAsync()
    {
        byte[]? value = await GetByteArrayOrDefaultAsync($"{baseMetadataUri}/spot/instance-action").ConfigureAwait(false);

        return value is { Length: > 0 }
            ? JsonSerializer.Deserialize<InstanceAction>(value)
            : null;
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
                    throw new Exception($"Invalid response getting /iam/security-credentials. StatusCode = {response.StatusCode}");
                }

                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                return JsonSerializer.Deserialize(responseBytes, IamJsonContext.Default.IamSecurityCredentials)!;
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }

        throw new Exception("error fetching security-credentials", lastException);
    }

    public async Task<InstanceIdentity> GetInstanceIdentityAsync()
    {
        using var response = await GetAsync($"http://{host}/latest/dynamic/instance-identity/document").ConfigureAwait(false);

        using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        return (await JsonSerializer.DeserializeAsync<InstanceIdentity>(responseStream).ConfigureAwait(false))!;
    }

    // us-east-1a
    public Task<string> GetAvailabilityZoneAsync()
    {
        return GetStringAsync($"{baseMetadataUri}/placement/availability-zone");
    }

    public async Task<string?> GetIamRoleNameAsync()
    {
        // TODO: Limit to first line...

        return await GetStringOrDefaultAsync($"{baseMetadataUri}/iam/security-credentials/").ConfigureAwait(false);
    }

    public Task<string> GetInstanceIdAsync()
    {
        return GetStringAsync($"{baseMetadataUri}/instance-id");
    }

    public async Task<IPAddress?> GetPublicIpv4Async()
    {
        string? result = await GetStringOrDefaultAsync($"{baseMetadataUri}/public-ipv4").ConfigureAwait(false);

        if (result is null || result.Length is 0) return null;

        return IPAddress.Parse(result);
    }

    public async Task<byte[]> GetUserDataAsync()
    {
        using var response = await GetAsync($"{baseMetadataUri}/user-data").ConfigureAwait(false);

        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }

    public Task<string?> GetUserDataStringAsync()
    {
        return GetStringOrDefaultAsync($"{baseMetadataUri}/user-data");
    }

    private MetadataToken? _token;

    private async Task<MetadataToken> GetTokenAsync(TimeSpan lifetime)
    {
        if (lifetime < TimeSpan.Zero)
        {
            throw new ArgumentException("Must be > 0", nameof(lifetime));
        }

        if (lifetime > TimeSpan.FromHours(6))
        {
            throw new ArgumentException("Must be less than 6 hours", nameof(lifetime));
        }

        int lifetimeInSeconds = (int)lifetime.TotalSeconds;

        var request = new HttpRequestMessage(HttpMethod.Put, $"http://{host}/latest/api/token") {
            Headers = {
                { "X-aws-ec2-metadata-token-ttl-seconds", lifetimeInSeconds.ToString(CultureInfo.InvariantCulture) }
            }
        };

        using HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new MetadataToken(responseText, expires: DateTime.UtcNow + lifetime);
    }

    private async Task<MetadataToken> GetTokenAsync()
    {
        var _5minutesFromNow = DateTime.UtcNow.AddMinutes(5);

        if (_token is not null && _token.Expires > _5minutesFromNow) // does not expire within 5 minutes
        {
            return _token;
        }

        _token = await GetTokenAsync(lifetime: _1hour).ConfigureAwait(false);

        return _token;
    }

    private async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        MetadataToken token = await GetTokenAsync().ConfigureAwait(false);

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri) {
            Headers = {
                { "X-aws-ec2-metadata-token", token.Value }
            }
        };

        return await httpClient.SendAsync(request).ConfigureAwait(false);
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

    private async Task<byte[]?> GetByteArrayOrDefaultAsync(string url)
    {
        using HttpResponseMessage response = await GetAsync(url).ConfigureAwait(false);

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
    }

    private async Task<string> GetStringAsync(string url)
    {
        using HttpResponseMessage response = await GetAsync(url).ConfigureAwait(false);

        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
}