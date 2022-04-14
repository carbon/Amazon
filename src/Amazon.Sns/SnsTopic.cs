using System.Threading.Tasks;

using Carbon.Messaging;

namespace Amazon.Sns;

public sealed class SnsTopic
{
    private readonly string _arn;
    private readonly SnsClient _client;

    public SnsTopic(AwsRegion region!!, string accountId!!, string topicName!!, IAwsCredential credential)
    {
        _arn    = $"arn:aws:sns:{region}:{accountId}:{topicName}";
        _client = new SnsClient(region, credential);
    }

    public Task<string> PublishAsync(string message)
    {
        // Max payload = 256KB (262,144 bytes)

        var request = new PublishRequest(_arn, message);

        return _client.PublishAsync(request);
    }

    public Task PublishAsync(IMessage<string> message)
    {
        return PublishAsync(message.Body);
    }
}