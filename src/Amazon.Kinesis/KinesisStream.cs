using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
    using Scheduling;

    public class KinesisStream : IStream
    {
        private readonly KinesisClient client;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
            initialDelay : TimeSpan.FromSeconds(1),
            maxDelay     : TimeSpan.FromSeconds(5),
            maxRetries   : 3);

        public KinesisStream(string name, KinesisClient client)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public string Name { get; }

        public Task PutAsync(byte[] data, string partitionKey)
        {
            // Serializer
            var record = new Record(Name, data) {
                PartitionKey = partitionKey
            };

            return retryPolicy.ExecuteAsync(async () => await client.PutRecordAsync(record).ConfigureAwait(false));
        }

        public Task<IReadOnlyList<IShard>> GetShardsAsync()
        {
            var request = new DescribeStreamRequest(Name);

            return retryPolicy.ExecuteAsync<IReadOnlyList<IShard>>(async () => {

                var result = await client.DescribeStreamAsync(request).ConfigureAwait(false);

                var shards = new List<IShard>(result.StreamDescription.Shards.Count);

                foreach (var shard in shards)
                {
                    shards.Add(shard);
                }

                return shards;
            });
        }

        public Task<IIterator> GetIteratorAsync(IShard shard, IteratorPosition position)
        {
            var request = new GetShardIteratorRequest(
                streamName             : Name,
                shardId                : shard.Id,
                type                   : ShardIteratorType.LATEST,
                startingSequenceNumber : position.Offset
            );

            return retryPolicy.ExecuteAsync<IIterator>(async () =>
                await client.GetShardIteratorAsync(request).ConfigureAwait(false)
            );
        }

        public Task<IRecordList> GetAsync(IIterator iterator, int take = 10000)
        {
            var request = new GetRecordsRequest(iterator.Value, take);

            return retryPolicy.ExecuteAsync<IRecordList>(async ()
                => await client.GetRecordsAsync(request).ConfigureAwait(false));
        }

        public IDisposable Subscribe(IShard shard, IObserver<IRecord> observer)
        {
            if (shard is null)
                throw new ArgumentNullException(nameof(shard));

            if (observer is null)
                throw new ArgumentNullException(nameof(observer));

            return new KinesisStreamSubscription(this, shard, observer);
        }
    }
}
