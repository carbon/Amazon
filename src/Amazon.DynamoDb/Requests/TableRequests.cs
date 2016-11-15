using System;
using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public abstract class ItemRequest
    {
        //  "DeleteRequest"		| Key
        //  "PutRequest"		| Item

        public abstract JsonObject ToJson();
    }

    public class PutRequest : ItemRequest
    {
        public PutRequest(AttributeCollection item)
        {
            Item = item;
        }

        public AttributeCollection Item { get; }

        public override JsonObject ToJson()
        {
            return new JsonObject {
                { "Item", Item.ToJson() }
            };
        }
    }

    public class DeleteRequest : ItemRequest
    {
        public DeleteRequest(AttributeCollection key)
        {
            Key = key;
        }

        public AttributeCollection Key { get; }

        public override JsonObject ToJson()
        {
            return new JsonObject {
                { "Key", Key.ToJson() }
            };
        }
    }

    public class TableRequests
    {
        public TableRequests(string tableName, List<ItemRequest> requests)
        {
            #region Preconditions

            if (tableName == null) throw new ArgumentNullException(nameof(tableName));
            if (requests == null) throw new ArgumentNullException(nameof(requests));

            if (requests.Count > 25) throw new ArgumentException("Must be 25 or fewer", "requests.Count");

            #endregion

            TableName = tableName;
            Requests = requests;
        }

        public string TableName { get; }

        public List<ItemRequest> Requests { get; }

        public static TableRequests FromJson(string key, JsonArray batch)
        {
            var requests = new List<ItemRequest>();

            foreach (var r in batch)
            {
                var request = (JsonObject)r;

                /* 
				{ 
					""PutRequest"": { 
						""Item"": { 
							""name"": { ""S"": ""marcywilliams"" }, 
							""ownerId"": { ""N"": ""3033325"" }, 
							""type"": { ""N"": ""1"" } 
						}
					},
					""DeleteRequest"": { 
						""Key"": { 
							""name"": { ""S"": ""marcywilliams"" }, 
							""ownerId"": { ""N"": ""3033325"" }
						}
					} 
				}
				*/

                if (request.ContainsKey("PutRequest"))
                {
                    var itemAttributes = AttributeCollection.FromJson((JsonObject)request["PutRequest"]["Item"]);

                    requests.Add(new PutRequest(itemAttributes));
                }
                else if (request.ContainsKey("DeleteRequest"))
                {
                    var keyAttributes = AttributeCollection.FromJson((JsonObject)request["DeleteRequest"]["Key"]);

                    requests.Add(new DeleteRequest(keyAttributes));
                }
            }

            return new TableRequests(key, requests);
        }

        public KeyValuePair<string, JsonNode> ToJson()
        {
            var requests = new XNodeArray();

            foreach (var request in Requests)
            {
                if (request is PutRequest)
                {
                    requests.Add(new JsonObject {
                        { "PutRequest", request.ToJson() }
                    });
                }
                else if (request is DeleteRequest)
                {
                    requests.Add(new JsonObject {
                        { "DeleteRequest", request.ToJson() }
                    });
                }
            }

            return new KeyValuePair<string, JsonNode>(TableName, requests);
        }
    }
}
