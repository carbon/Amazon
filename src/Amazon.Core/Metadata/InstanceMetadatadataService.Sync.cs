using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Amazon.Metadata;

public sealed partial class InstanceMetadataService
{
    public string? GetIamRoleName()
    {
        return GetStringOrDefault(baseMetadataUri + "/iam/security-credentials/");
    }

    public string GetAvailabilityZone()
    {
        return GetStringOrDefault(baseMetadataUri + "/placement/availability-zone")!;
    }

    internal IamSecurityCredentials GetIamSecurityCredentials(string roleName)
    {
        string requestUri = baseMetadataUri + "/iam/security-credentials/" + roleName;

        Exception? lastException = null;

        for (int i = 0; i < 3; i++)
        {
            try
            {
                using HttpResponseMessage response = Get(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Invalid response getting /iam/security-credentials. {response.StatusCode}");
                }

                using var responseStream = response.Content.ReadAsStream();

                var result = JsonSerializer.Deserialize<IamSecurityCredentials>(GetString(responseStream));

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


    private MetadataToken GetToken()
    {
        // does not expire within 5 minutes
        if (token is not null && token.Expires > DateTime.UtcNow.AddMinutes(5))
        {
            return token;
        }

        token = GetToken(TimeSpan.FromHours(1));

        return token;
    }

    private HttpResponseMessage Get(string requestUri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        MetadataToken token = GetToken();

        request.Headers.Add("X-aws-ec2-metadata-token", token.Value);

        return httpClient.Send(request);

    }

    private MetadataToken GetToken(TimeSpan lifetime)
    {
        if (lifetime < TimeSpan.Zero || lifetime > TimeSpan.FromHours(6))
        {
            throw new ArgumentException("Must be > 0 & less than 6 hours", nameof(lifetime));
        }

        int lifetimeInSeconds = (int)lifetime.TotalSeconds;

        var request = new HttpRequestMessage(HttpMethod.Put, "http://169.254.169.254/latest/api/token")
        {
            Headers = {
                { "X-aws-ec2-metadata-token-ttl-seconds", lifetimeInSeconds.ToString(CultureInfo.InvariantCulture) }
            }
        };

        string responseText = SendAndReadString(request);

        return new MetadataToken(responseText, DateTime.UtcNow + lifetime);
    }

    private string? GetStringOrDefault(string url)
    {
        using HttpResponseMessage response = Get(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        using var responseStream = response.Content.ReadAsStream();

        string responseText = GetString(responseStream);

        return responseText;
    }

    private string SendAndReadString(HttpRequestMessage request)
    {
        using HttpResponseMessage response = httpClient.Send(request, HttpCompletionOption.ResponseContentRead);

        using var responseStream = response.Content.ReadAsStream();

        string responseText = GetString(responseStream);

        return responseText;
    }

    private static string GetString(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: false);

        return reader.ReadToEnd();
    }
}
