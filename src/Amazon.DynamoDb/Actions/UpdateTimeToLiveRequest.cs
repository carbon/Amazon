using System;

namespace Amazon.DynamoDb
{
    public sealed class UpdateTimeToLiveRequest
    {
        public UpdateTimeToLiveRequest(string tableName, string attributeName, bool enabled)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            TimeToLiveSpecification = new TimeToLiveSpecification(attributeName, enabled);
        }

        public string TableName { get; }

        public TimeToLiveSpecification TimeToLiveSpecification { get; }
    }
}