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

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "ProjectionType", ProjectionType.ToQuickString() }
            };

            if (NonKeyAttributes != null && NonKeyAttributes.Length > 0)
                json.Add("NonKeyAttributes", new XImmutableArray<string>(NonKeyAttributes));

            return json;
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
