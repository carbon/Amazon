namespace Amazon.DynamoDb
{
	public interface IConsumedResources
	{
		ConsumedCapacity ConsumedCapacity { get; }
	}
}