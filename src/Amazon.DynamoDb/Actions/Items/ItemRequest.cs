using Carbon.Json;

namespace Amazon.DynamoDb
{
    public abstract class ItemRequest
    {
        //  "DeleteRequest"		| Key
        //  "PutRequest"		| Item

        public abstract JsonObject ToJson();
    }
}
