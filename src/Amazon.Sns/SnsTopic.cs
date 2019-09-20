using System;
using System.Threading.Tasks;

using Carbon.Messaging;

namespace Amazon.Sns
{
    public sealed class SnsTopic
    {
        private readonly SnsClient client;
        private readonly string arn;

        public SnsTopic(AwsRegion region, string accountId, string topicName, IAwsCredential credential)
        {
            if (accountId is null)
                throw new ArgumentNullException(nameof(accountId));

            if (topicName is null)
                throw new ArgumentNullException(nameof(topicName));
            
            this.client = new SnsClient(region, credential);
            this.arn = $"arn:aws:sns:{region}:{accountId}:{topicName}";
        }

        public Task<string> PublishAsync(string message)
        {
            // Max payload = 256KB (262,144 bytes)

            var request = new PublishRequest(arn, message);

            return client.PublishAsync(request);
        }

        public Task PublishAsync(IMessage<string> message)
        {
            return PublishAsync(message.Body);
        }
    }
}
