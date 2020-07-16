#nullable disable
using Amazon.DynamoDb.Extensions;
using System;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class ListTablesResult
    {
        public string LastEvaluatedTableName { get; set; }
        public bool HasMoreTables => LastEvaluatedTableName != null;
        public string[] TableNames { get; set; }

        public static ListTablesResult FromJsonElement(JsonElement el)
        {
            string[] tableNames = null;
            string lastEvaluatedTableName = null;

            foreach (var property in el.EnumerateObject())
            {
                if (property.NameEquals("TableNames"))
                {
                    tableNames = property.Value.GetStringArray();
                }
                else if (property.NameEquals("LastEvaluatedTableName"))
                {
                    lastEvaluatedTableName = property.Value.GetString();
                }
            }

            return new ListTablesResult()
            {
                TableNames = tableNames,
                LastEvaluatedTableName = lastEvaluatedTableName,
            };
        }
    }
}

/*
{
   "LastEvaluatedTableName": "string",
   "TableNames": [ "string" ]
}
*/