#nullable disable

using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
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

        public JsonObject ToJson()
        {
            return new JsonObject {
                { "NonKeyAttributes", new XImmutableArray<string>(NonKeyAttributes) },
                { "ProjectionType", ProjectionType.ToQuickString() }
            };
        }

        public static Projection FromJsonElement(JsonElement element)
        {
            var projection = new Projection();

            foreach (var prop in element.EnumerateObject())
            {
                if (prop.NameEquals("NonKeyAttributes")) projection.NonKeyAttributes = prop.Value.GetStringArray();
                else if (prop.NameEquals("ProjectionType")) projection.ProjectionType = prop.Value.GetEnum<ProjectionType>();
            }

            return projection;
        }
    }
}
