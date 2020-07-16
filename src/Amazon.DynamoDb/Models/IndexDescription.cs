using Amazon.DynamoDb.Extensions;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class IndexDescription
    {
        public string? IndexName { get; set; }
        public long IndexSizeBytes { get; set; }
        public long ItemCount { get; set; }
        public KeySchemaElement[]? KeySchema { get; set; }
        public Projection? Projection { get; set; }
    }
}
