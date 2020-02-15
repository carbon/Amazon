using System;
using System.Threading.Tasks;

namespace Amazon.Elb
{
    public sealed class TargetGroupService
    {
        private readonly ElbClient client;

        public TargetGroupService(ElbClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddInstancesAsync(string targetGroupArn, params string[] instanceIds)
        {
            if (instanceIds is null)
                throw new ArgumentNullException(nameof(instanceIds));

            if (instanceIds.Length == 0)
                throw new ArgumentException("May not be empty", nameof(instanceIds));

            var targets = new TargetDescription[instanceIds.Length];

            for (var i = 0; i < targets.Length; i++)
            {
                targets[i] = new TargetDescription(instanceIds[i]);
            }

            var request = new RegisterTargetsRequest(targetGroupArn, targets);

            await client.RegisterTargetsAsync(request).ConfigureAwait(false);
        }

        public async Task RemoveInstancesAsync(string targetGroupArn, params string[] instanceIds)
        {
            if (instanceIds is null)
                throw new ArgumentNullException(nameof(instanceIds));

            if (instanceIds.Length == 0)
                throw new ArgumentException("May not be empty", nameof(instanceIds));

            var targets = new TargetDescription[instanceIds.Length];

            for (var i = 0; i < targets.Length; i++)
            {
                targets[i] = new TargetDescription(instanceIds[i]);
            }

            var request = new DeregisterTargetsRequest(targetGroupArn, targets);

            await client.DeregisterTargetsAsync(request).ConfigureAwait(false);
        }
    }
}