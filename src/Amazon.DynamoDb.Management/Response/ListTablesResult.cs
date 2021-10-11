using System.Text.Json;

namespace Amazon.DynamoDb;

public sealed class ListTablesResult
{
#nullable disable

    public string[] TableNames { get; init; }

#nullable enable

    public string? LastEvaluatedTableName { get; init; }

    public bool HasMoreTables => LastEvaluatedTableName != null;

    public static ListTablesResult FromJsonElement(in JsonElement el)
    {
        return JsonSerializer.Deserialize<ListTablesResult>(el)!;
    }
}

/*
{
   "LastEvaluatedTableName": "string",
   "TableNames": [ "string" ]
}
*/