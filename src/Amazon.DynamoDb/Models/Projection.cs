using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class Projection
    {
        public Projection(string[] nonKeyAttributes, ProjectionType type)
        {
            NonKeyAttributes = nonKeyAttributes ?? new string[0];
            ProjectionType = type;
        }

        public string[] NonKeyAttributes { get; }
        public ProjectionType ProjectionType { get; }

        public JsonObject ToJson()
        {
            return new JsonObject {
                { "NonKeyAttributes", new XImmutableArray<string>(NonKeyAttributes) },
                { "ProjectionType", ProjectionType.ToQuickString() }
            };
        }
    }
}
