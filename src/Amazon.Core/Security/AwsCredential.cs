#pragma warning disable IDE0057 // Use range operator

using System;
using System.Threading.Tasks;

namespace Amazon;

public sealed class AwsCredential : IAwsCredential
{
    public AwsCredential(string accessKeyId, string secretAccessKey)
    {
        ArgumentNullException.ThrowIfNull(accessKeyId);
        ArgumentNullException.ThrowIfNull(secretAccessKey);

        AccessKeyId = accessKeyId;
        SecretAccessKey = secretAccessKey;
    }

    // 16 - 32 characters
    public string AccessKeyId { get; }

    public string SecretAccessKey { get; }

    public string? SecurityToken => null;

    // {id}:{secret}

    public static AwsCredential Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        int colonIndex = text.IndexOf(':');

        if (colonIndex == -1)
        {
            throw new ArgumentException("accessKeyId & secretAccessKey must be seperated by ':'");
        }

        return new AwsCredential(
            accessKeyId: text.Substring(0, colonIndex),
            secretAccessKey: text.Substring(colonIndex + 1)
        );
    }

    public bool ShouldRenew => false;

    public Task<bool> RenewAsync() => Task.FromResult(false);
}
