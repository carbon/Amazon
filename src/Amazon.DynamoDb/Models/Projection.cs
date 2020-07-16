#nullable disable

using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class Projection
    {
        public Projection() { }
        public Projection(string[] nonKeyAttributes, ProjectionType type)
        {
            NonKeyAttributes = nonKeyAttributes ?? new string[0];
            ProjectionType = type;
        }

        public string[] NonKeyAttributes { get; set; }
        public ProjectionType ProjectionType { get; set; }
    }
}
