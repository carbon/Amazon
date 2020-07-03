#nullable disable

using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ConsumedCapacity
    {
        public string TableName { get; set; }

        public float CapacityUnits { get; set; }

        internal static ConsumedCapacity FromJsonElement(JsonElement el)
        {
            var result = new ConsumedCapacity();

            foreach (var property in el.EnumerateObject())
            {
                if (property.NameEquals("TableName"))
                {
                    result.TableName = property.Value.GetString();
                }
                else if (property.NameEquals("CapacityUnits"))
                {
                    result.CapacityUnits = property.Value.GetSingle();
                }
            }

            return result;
        }
    }
}

/*
{
   "TableName": "Thread",
   "CapacityUnits": 1
}
*/
