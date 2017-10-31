using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
	public class GetShardIteratorResponse : IIterator
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