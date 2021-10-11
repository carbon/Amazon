using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Scheduling;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class KinesisStream : IStream
{
    private readonly KinesisClient _client;

    private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromSeconds(1),
        maxDelay     : TimeSpan.FromSeconds(5),
        maxRetries   : 3
    );

    public KinesisStream(string name, KinesisClient client)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(client);

        Name = name;
        _client = client;
    }

    public string Name { get; }

    public Task PutAsync(byte[] data, string partitionKey)
    {
        var record = new Record(Name, data) {
            PartitionKey = partitionKey
        };

        return retryPolicy.ExecuteAsync(async () => await _client.PutRecordAsync(record).ConfigureAwait(false));
    }

    public Task<IReadOnlyList<IShard>> GetShardsAsync()
    {
        var request = new DescribeStreamRequest(Name);

        return retryPolicy.ExecuteAsync<IReadOnlyList<IShard>>(async () => {

            var result = await _client.DescribeStreamAsync(request).ConfigureAwait(false);

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
            await _client.GetShardIteratorAsync(request).ConfigureAwait(false)
        );
    }

    public Task<IRecordList> GetAsync(IIterator iterator, int take = 10000)
    {
        var request = new GetRecordsRequest(iterator.Value, take);

        return retryPolicy.ExecuteAsync<IRecordList>(async () => 
            await _client.GetRecordsAsync(request).ConfigureAwait(false)
        );
    }

    public IDisposable Subscribe(IShard shard, IObserver<IRecord> observer)
    {
        ArgumentNullException.ThrowIfNull(shard);
        ArgumentNullException.ThrowIfNull(observer);

        return new KinesisStreamSubscription(this, shard, observer);
    }
}
