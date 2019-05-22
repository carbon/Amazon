#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
    public sealed class KinesisStreamSubscription : IDisposable
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
