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
        ArgumentNullException.ThrowIfNull(roleArn);
        ArgumentNullException.ThrowIfNull(roleSessionName);

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

    public string RoleArn { get; init; }

    public string RoleSessionName { get; init; }

    public JsonElement? Policy { get; init; }
}