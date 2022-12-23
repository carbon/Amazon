using System.Net;
using System.Net.Http;
using System.Text.Json;

using Amazon.Exceptions;
using Amazon.Scheduling;

namespace Amazon.DynamoDb;

public class DynamoDbException : AwsException, IException
{
    public DynamoDbException(string message, HttpStatusCode statusCode)
        : base(message, statusCode) { }

    public DynamoDbException(string message, string? type, HttpStatusCode statusCode)
      : base(message, statusCode)
    {
        Type = type;
    }

    public DynamoDbException(string message, Exception innerException, HttpStatusCode statusCode = default)
        : base(message, innerException, statusCode) { }

    public string? Type { get; }

    public static async Task<DynamoDbException> FromResponseAsync(HttpResponseMessage response)
    {
        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        using var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);

        JsonElement json = doc.RootElement;

        string type = string.Empty;
        string message = string.Empty;

        foreach (var property in json.EnumerateObject())
        {
            if (property.NameEquals("message"u8))
            {
                message = property.Value.GetString()!;
            }
            else if (property.NameEquals("Message"u8))
            {
                message = property.Value.GetString()!;
            }

            else if (property.NameEquals("__type"u8) && property.Value.ValueKind is JsonValueKind.String)
            {
                type = property.Value.GetString()!;

                int poundIndex = type.IndexOf('#');

                if (poundIndex > -1)
                {
                    type = type.Substring(poundIndex + 1);
                }
            }
        }

        if (type is "ConditionalCheckFailedException")
        {
            return new ConflictException(message, response.StatusCode);
        }

        return new DynamoDbException(message, type, response.StatusCode);
    }

    public bool IsTransient
    {
        get
        {
            // Client Errors = 4xx (Don't retry)
            // Server Errors = 5xx (Retry)

            return Type
                is "InternalServerError"
                or "InternalFailure"
                or "ProvisionedThroughputExceededException"
                or "ThrottlingException";
        }
    }
}

/*
{
"__type":"com.amazonaws.dynamodb.v20111205#ProvisionedThroughputExceededException",
"message":"The level of configured provisioned throughput for the table was exceeded. Consider increasing your provisioning level with the UpdateTable API"
}

{"__type":"com.amazon.coral.service#ExpiredTokenException","message":"The security token included in the request is expired"}

{"__type":"com.amazon.coral.service#SerializationException","Message":"Start of list found where not expected"}
*/
