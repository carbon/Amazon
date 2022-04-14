using System;

namespace Amazon.DynamoDb;

internal class TableRequest
{
    internal TableRequest(string tableName!!)
    {
        TableName = tableName;
    }

    public string TableName { get; }
}
