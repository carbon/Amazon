using System;
using System.Collections.Generic;
using System.Text.Json;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class TableRequests
    {
        public TableRequests(string tableName, IReadOnlyList<ItemRequest> requests)
        {
            if (requests.Count > 25)
                throw new ArgumentException("Must be 25 or fewer", "requests.Count");

            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Requests = requests;
        }

        public string TableName { get; }

        public IReadOnlyList<ItemRequest> Requests { get; }

        public static TableRequests FromElementJson(string key, JsonElement batch)
        {
            var requests = new List<ItemRequest>(batch.GetArrayLength());

            foreach (var request in batch.EnumerateArray())
            {
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

                if (request.TryGetProperty("PutRequest", out var putRequestNode))
                {
                    var itemAttributes = AttributeCollection.FromJsonElement(putRequestNode.GetProperty("Item"));

                    requests.Add(new PutRequest(itemAttributes));
                }
                else if (request.TryGetProperty("DeleteRequest", out var deleteRequestNode))
                {
                    var keyAttributes = AttributeCollection.FromJsonElement(deleteRequestNode.GetProperty("Key"));

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
