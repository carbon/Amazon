using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Responses
{
    public sealed class ListTablesResult
    {
        public ListTablesResult(string[] tableNames)
        {
            TableNames = tableNames ?? throw new ArgumentNullException(nameof(tableNames));
        }

        public string? LastEvaluatedTableName { get; set; }
        public bool HasMoreTables => LastEvaluatedTableName != null;
        public string[] TableNames { get; set; }

        public static ListTablesResult FromJson(JsonObject json)
        {
            string[] tableNames;
            if (json.TryGetValue("TableNames", out var tableNamesNode))
            {
                tableNames = tableNamesNode.ToArrayOf<string>();
            }
            else
            {
                tableNames = new string[0];
            }

            var result = new ListTablesResult(tableNames);

            if (json.TryGetValue("LastEvaluatedTableName", out var lastEvaluatedNode))
            {
                result.LastEvaluatedTableName = lastEvaluatedNode.ToString();
            }

            return result;
        }
    }
}

/*
{
   "LastEvaluatedTableName": "string",
   "TableNames": [ "string" ]
}
*/