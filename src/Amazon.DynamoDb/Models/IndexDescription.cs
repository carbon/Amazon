using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
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
