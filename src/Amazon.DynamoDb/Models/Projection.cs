#nullable disable

using System;

namespace Amazon.DynamoDb
{
    public sealed class Projection
    {
        public Projection() { }

        public Projection(string[] nonKeyAttributes, ProjectionType type)
        {
            NonKeyAttributes = nonKeyAttributes ?? Array.Empty<string>();
            ProjectionType = type;
        }

        public string[] NonKeyAttributes { get; set; }

        public ProjectionType ProjectionType { get; set; }
    }
}