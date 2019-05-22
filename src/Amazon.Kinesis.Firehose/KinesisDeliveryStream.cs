#nullable enable

using System;
using System.Threading.Tasks;

namespace Amazon.Kinesis.Firehose
{
    public class DeliveryStream
    {
        private readonly KinesisFirehoseClient client;

        public DeliveryStream(string name, KinesisFirehoseClient client)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public string Name { get; }

        public Task PutAsync(byte[] data)
        {
            return client.PutRecordAsync(new PutRecordRequest(Name, new Record(data)));
        }
    }
}