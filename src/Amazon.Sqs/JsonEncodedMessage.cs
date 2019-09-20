using Carbon.Json;
using Carbon.Messaging;

namespace Amazon.Sqs
{
    public sealed class JsonEncodedMessage<T> : IQueueMessage<T>
        where T : notnull, new()
    {
        private readonly SqsMessage model;

        public JsonEncodedMessage(SqsMessage model)
        {
            this.model = model;

            Body = JsonObject.Parse(model.Body).As<T>();
        }

        public static JsonEncodedMessage<T> Create(SqsMessage message)
        {
            return new JsonEncodedMessage<T>(message);
        }

        public string Id => model.MessageId;

        public MessageReceipt Receipt => new MessageReceipt(model.ReceiptHandle);

        public T Body { get; }
    }
}