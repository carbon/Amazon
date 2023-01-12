using System.Text.Json;

namespace Amazon.Sts;

public sealed class AssumeRoleRequest : IStsRequest
{
    public AssumeRoleRequest(
        string roleArn,
        string roleSessionName,
        TimeSpan? duration = null,
        JsonElement? policy = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleArn);
        ArgumentException.ThrowIfNullOrEmpty(roleSessionName);

        RoleArn = roleArn;
        RoleSessionName = roleSessionName;
        Policy = policy;

        if (duration.HasValue)
        {
            if (duration.Value < TimeSpan.FromMinutes(15))
            {
                throw new ArgumentOutOfRangeException(nameof(duration), "Must be at least 15 minutes");
            }

            if (duration.Value > TimeSpan.FromHours(12))
            {
                throw new ArgumentOutOfRangeException(nameof(duration), "May not excdeed 12 hours");
            }

            DurationSeconds = (int)duration.Value.TotalSeconds;
        }
    }

    string IStsRequest.Action => "AssumeRole";

    public int? DurationSeconds { get; }

    public string RoleArn { get; }

    public string RoleSessionName { get; }

    public JsonElement? Policy { get; }
}