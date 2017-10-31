using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class CountResult
    {
        public CountResult() { }

        public CountResult(int count)
        {
            Count = count;
        }

        public int Count { get; set; }

        public static CountResult FromJson(JsonObject json)
        {
            return new CountResult((int)json["Count"]);
        }
    }
}

/*
{
    "Count":17
}
*/
