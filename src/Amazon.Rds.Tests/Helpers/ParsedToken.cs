using System.Security.Cryptography;
using System.Text;

namespace Amazon.Rds.Tests;

// Parses a token and re-derives its SigV4 signature without using SignerV4, so that the tests
// don't simply compare the signer with itself. Encoding follows the SigV4 rule that everything
// except A-Z a-z 0-9 - _ . ~ is percent-encoded, and '+' is a literal plus rather than a space.
internal sealed class ParsedToken
{
    private ParsedToken(string host, int port, Dictionary<string, string> parameters)
    {
        Host = host;
        Port = port;
        Parameters = parameters;
    }

    public string Host { get; }

    public int Port { get; }

    public Dictionary<string, string> Parameters { get; }

    public string this[string name] => Parameters[name];

    public static ParsedToken Parse(string token)
    {
        int slash = token.IndexOf('/');

        Assert.True(slash > 0, $"Expected host:port/?query, was: {token}");
        Assert.Equal('?', token[slash + 1]);

        string authority = token[..slash];
        int colon = authority.LastIndexOf(':');

        Assert.True(colon > 0, $"Expected a port in: {authority}");

        var parameters = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (string pair in token[(slash + 2)..].Split('&'))
        {
            int equals = pair.IndexOf('=');

            Assert.True(equals > 0, $"Expected name=value, was: {pair}");

            parameters[Uri.UnescapeDataString(pair[..equals])] = Uri.UnescapeDataString(pair[(equals + 1)..]);
        }

        return new ParsedToken(authority[..colon], int.Parse(authority[(colon + 1)..]), parameters);
    }

    public string ComputeSignature(string secretAccessKey, string signedHost)
    {
        var canonicalQuery = string.Join('&', Parameters
            .Where(static p => p.Key is not "X-Amz-Signature")
            .Select(static p => (Key: Uri.EscapeDataString(p.Key), Value: Uri.EscapeDataString(p.Value)))
            .OrderBy(static p => p.Key, StringComparer.Ordinal)
            .Select(static p => $"{p.Key}={p.Value}"));

        string emptyPayloadHash = Hex(SHA256.HashData([]));

        string canonicalRequest = $"GET\n/\n{canonicalQuery}\nhost:{signedHost}\n\nhost\n{emptyPayloadHash}";

        string[] scope = this["X-Amz-Credential"].Split('/');
        string date = scope[1], region = scope[2], service = scope[3], terminator = scope[4];

        string stringToSign = string.Join('\n',
            "AWS4-HMAC-SHA256",
            this["X-Amz-Date"],
            $"{date}/{region}/{service}/{terminator}",
            Hex(SHA256.HashData(Encoding.UTF8.GetBytes(canonicalRequest))));

        byte[] key = Encoding.UTF8.GetBytes("AWS4" + secretAccessKey);

        foreach (string part in new[] { date, region, service, terminator })
        {
            key = HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(part));
        }

        return Hex(HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(stringToSign)));
    }

    private static string Hex(byte[] bytes) => Convert.ToHexStringLower(bytes);
}

internal sealed class FakeCredential(string accessKeyId, string secretAccessKey, string? securityToken = null) : IAwsCredential
{
    public string AccessKeyId { get; private set; } = accessKeyId;

    public string SecretAccessKey { get; private set; } = secretAccessKey;

    public string? SecurityToken { get; private set; } = securityToken;

    public bool ShouldRenew { get; set; }

    public int RenewCount { get; private set; }

    public Exception? RenewFailure { get; set; }

    public (string AccessKeyId, string SecretAccessKey, string SecurityToken)? RenewTo { get; set; }

    public async Task<bool> RenewAsync()
    {
        RenewCount++;

        // Force a real suspension so the service's async path is exercised
        await Task.Yield();

        if (RenewFailure is not null)
        {
            throw RenewFailure;
        }

        if (RenewTo is not { } next)
        {
            return false;
        }

        (AccessKeyId, SecretAccessKey, SecurityToken) = next;
        ShouldRenew = false;

        return true;
    }
}
