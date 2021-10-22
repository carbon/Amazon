using System.Threading.Tasks;

namespace Amazon.Elb;

public sealed class TargetGroupService
{
    private readonly ElbClient _client;

    public TargetGroupService(ElbClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    public async Task AddInstancesAsync(string targetGroupArn, params string[] instanceIds)
    {
        ArgumentNullException.ThrowIfNull(instanceIds);

        if (instanceIds.Length is 0)
            throw new ArgumentException("May not be empty", nameof(instanceIds));

        var targets = new TargetDescription[instanceIds.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = new TargetDescription(instanceIds[i]);
        }

        var request = new RegisterTargetsRequest(targetGroupArn, targets);

        await _client.RegisterTargetsAsync(request).ConfigureAwait(false);
    }

    public async Task RemoveInstancesAsync(string targetGroupArn, params string[] instanceIds)
    {
        ArgumentNullException.ThrowIfNull(instanceIds);

        if (instanceIds.Length is 0)
            throw new ArgumentException("May not be empty", nameof(instanceIds));

        var targets = new TargetDescription[instanceIds.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = new TargetDescription(instanceIds[i]);
        }

        var request = new DeregisterTargetsRequest(targetGroupArn, targets);

        await _client.DeregisterTargetsAsync(request).ConfigureAwait(false);
    }
}