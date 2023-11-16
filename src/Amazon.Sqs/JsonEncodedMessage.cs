using System.Text.Json;
using System.Text.Json.Serialization;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class JsonEncodedMessage<T> : IQueueMessage<T>
    where T : notnull
{
    private static readonly JsonSerializerOptions s_jso = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly SqsMessage _model;

    internal JsonEncodedMessage(SqsMessage model)
    {
        _model = model;

        Body = JsonSerializer.Deserialize<T>(model.Body, s_jso)!;
    }

    public static JsonEncodedMessage<T> Create(SqsMessage message)
    {
        return new JsonEncodedMessage<T>(message);
    }

    public string Id => _model.MessageId;

    public MessageReceipt Receipt => new(_model.ReceiptHandle);

    public T Body { get; }
}
