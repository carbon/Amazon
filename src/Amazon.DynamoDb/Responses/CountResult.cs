using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class CountResult
    {
        public int Count { get; set; }

        public static CountResult FromJson(JsonObject json)
        {
            return new CountResult {
                Count = (int)json["Count"]
            };
        }
    }
}

/*
{
    "Count":17
}
*/
