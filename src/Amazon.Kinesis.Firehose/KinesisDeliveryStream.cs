namespace Amazon.Kinesis.Firehose;

public class DeliveryStream
{
    private readonly KinesisFirehoseClient _client;

    public DeliveryStream(string name, KinesisFirehoseClient client)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(client);

        Name = name;
        _client = client;
    }

    public string Name { get; }

    public Task PutAsync(byte[] data)
    {
        return _client.PutRecordAsync(new PutRecordRequest(Name, new Record(data)));
    }
}