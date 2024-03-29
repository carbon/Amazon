﻿using System.Text.Json;

using Amazon.DynamoDb.Serialization;

namespace Amazon.DynamoDb;

public sealed class TableRequests
{
    public TableRequests(string tableName, IReadOnlyList<ItemRequest> requests)
    {
        ArgumentException.ThrowIfNullOrEmpty(tableName);
        ArgumentNullException.ThrowIfNull(requests);

        if (requests.Count > 25)
        {
            throw new ArgumentException($"Must be 25 items or fewer. Was {requests.Count} items", nameof(requests));
        }

        TableName = tableName;
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
                "PutRequest": { 
                    "Item": { 
                        "name": { "S": "marcywilliams" }, 
                        "ownerId": { "N": "3033325" }, 
                        "type": { "N": "1" } 
                    }
                },
                "DeleteRequest": { 
                    "Key": { 
                        "name": { "S": "marcywilliams" }, 
                        "ownerId": { "N": "3033325" }
                    }
                } 
            }
            */

            foreach (var property in request.EnumerateObject())
            {
                if (property.NameEquals("PutRequest"u8))
                {
                    var putRequest = property.Value.Deserialize(DynamoDbSerializationContext.Default.PutRequest);

                    requests.Add(putRequest!);
                }
                else if (property.NameEquals("DeleteRequest"u8))
                {
                    var deleteRequest = property.Value.Deserialize(DynamoDbSerializationContext.Default.DeleteRequest);

                    requests.Add(deleteRequest!);
                }
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