using System.Threading;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class KinesisStreamSubscription : IDisposable
{
    private readonly CancellationTokenSource cts = new();

    private readonly KinesisStream _stream;
    private readonly IShard _shard;
    private readonly IObserver<IRecord> _observer;

    public KinesisStreamSubscription(KinesisStream stream, IShard shard, IObserver<IRecord> observer)
    {
        _stream = stream;
        _shard = shard;
        _observer = observer;

        Task.Factory.StartNew(StartAsync, TaskCreationOptions.LongRunning);
    }

    private async Task StartAsync()
    {
        var iterator = await _stream.GetIteratorAsync(_shard, IteratorPosition.End).ConfigureAwait(false);

        while (!cts.IsCancellationRequested)
        {
            try
            {
                var result = await _stream.GetAsync(iterator).ConfigureAwait(false);

                foreach (var record in result)
                {
                    _observer.OnNext(record);
                }

                iterator = result.NextIterator;

            }
            catch { }

            await Task.Delay(250, cts.Token).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        cts.Cancel();
    }
}
