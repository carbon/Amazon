#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb
{
    public sealed class UpdateTableResult
    {
        public TableDescription TableDescription { get; set; }
    }
}