#nullable enable

using System;
using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class TableRequests
    {
        public TableRequests(string tableName, List<ItemRequest> requests)
        {
            if (requests.Count > 25)
                throw new ArgumentException("Must be 25 or fewer", "requests.Count");

            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Requests = requests;
        }

        public string TableName { get; }

        public List<ItemRequest> Requests { get; }

        public static TableRequests FromJson(string key, JsonArray batch)
        {
            var requests = new List<ItemRequest>(batch.Count);

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

                if (request.TryGetValue("PutRequest", out var putRequestNode))
                {
                    var itemAttributes = AttributeCollection.FromJson((JsonObject)putRequestNode["Item"]);

                    requests.Add(new PutRequest(itemAttributes));
                }
                else if (request.TryGetValue("DeleteRequest", out var deleteRequestNode))
                {
                    var keyAttributes = AttributeCollection.FromJson((JsonObject)deleteRequestNode["Key"]);

                    requests.Add(new DeleteRequest(keyAttributes));
                }
            }

            return new TableRequests(key, requests);
        }

        public KeyValuePair<string, JsonNode> ToJson()
        {
            var requests = new JsonNodeList();

            foreach (ItemRequest request in Requests)
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
