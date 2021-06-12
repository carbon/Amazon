namespace Amazon.Kinesis.Firehose
{
    public sealed class ElasticsearchDestinationConfiguration
    {
        public BufferingHints? BuferingHints { get; init; }

        public CloudWatchLoggingOptions? CloudWatchLoggingOptions { get; init; }

        public string? ClusterEndpoint { get; init; }

        public string? DomainARN { get; init; }

#nullable disable

        public string IndexName { get; init; }

        public string IndexRotationPeriod { get; init; }
    }
}
