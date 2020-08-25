using System.Text.Json;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    public sealed class JsonEncodedMessage<T> : IQueueMessage<T>
        where T : notnull
    {
        private static readonly JsonSerializerOptions jso = new () {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly SqsMessage model;

        internal JsonEncodedMessage(SqsMessage model)
        {
            this.model = model;

            Body = JsonSerializer.Deserialize<T>(model.Body, jso);
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