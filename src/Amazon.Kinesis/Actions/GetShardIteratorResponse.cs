#nullable disable

using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
	public sealed class GetShardIteratorResponse : KinesisResponse, IIterator
	{
		public string ShardIterator { get; set; }

        #region IIterator

        string IIterator.Value => ShardIterator;

		#endregion
	}
}

/*
{
    "ShardIterator": "string"
}
*/