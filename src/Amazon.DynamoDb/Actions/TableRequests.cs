using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class TableRequests
    {
        public TableRequests(string tableName, IReadOnlyList<ItemRequest> requests)
        {
            if (requests.Count > 25)
            {
                throw new ArgumentException($"May not exceed 25 items. Was {requests.Count} items.", nameof(requests));
            }

            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Requests = requests;
        }

        public string TableName { get; }

        public IReadOnlyList<ItemRequest> Requests { get; }

        public static TableRequests FromJsonElement(string key, in JsonElement batch)
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

        public List<object> SerializeList()
        {
            var requests = new List<object>(Requests.Count);

            foreach (ItemRequest request in Requests)
            {
                if (request is PutRequest put)
                {
                    requests.Add(new { PutRequest = put });
                }
                else if (request is DeleteRequest delete)
                {
                    requests.Add(new { DeleteRequest = delete });
                }
            }

            return requests;
        }
    }
}