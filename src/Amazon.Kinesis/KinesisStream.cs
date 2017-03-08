using System;
using System.Collections.Generic;
using System.Threading;
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

            return retryPolicy.ExecuteAsync<IIterator>(async ()
                => await client.GetShardIteratorAsync(request).ConfigureAwait(false)
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
            #region Preconditions

            if (shard == null)
                throw new ArgumentNullException(nameof(shard));

            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            #endregion

            return new KinesisStreamSubscription(this, shard, observer);
        }
    }

    public class KinesisStreamSubscription : IDisposable
    {
        private readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();

        private readonly KinesisStream stream;
        private readonly IShard shard;
        private readonly IObserver<IRecord> observer;

        public KinesisStreamSubscription(KinesisStream stream, IShard shard, IObserver<IRecord> observer)
        {
            this.stream = stream;
            this.shard = shard;
            this.observer = observer;

            Task.Factory.StartNew(async () => await StartAsync().ConfigureAwait(false));
        }

        private async Task StartAsync()
        {
            var iterator = await stream.GetIteratorAsync(shard, IteratorPosition.End).ConfigureAwait(false);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await stream.GetAsync(iterator).ConfigureAwait(false);

                    foreach (var record in result)
                    {
                        observer.OnNext(record);
                    }

                    iterator = result.NextIterator;

                }
                catch { }

                await Task.Delay(250, cancellationToken.Token).ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            cancellationToken.Cancel();
        }
    }
}
