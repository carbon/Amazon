using System;

namespace Amazon.DynamoDb
{
    internal class TableRequest
    {
        internal TableRequest(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string TableName { get; }
    }
}