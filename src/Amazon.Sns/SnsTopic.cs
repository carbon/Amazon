using System;
using System.Threading.Tasks;

using Carbon.Messaging;

namespace Amazon.Sns
{
    public class SnsTopic
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
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            
            // Max payload = 256KB (262,144 bytes)

            var request = new PublishRequest {
                TopicArn = arn,
                Message = message
            };

            return client.PublishAsync(request);
        }

        public Task PublishAsync(IMessage<string> message) => PublishAsync(message.Body);
    }
}
