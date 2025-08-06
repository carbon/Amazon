using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Amazon.Exceptions;

namespace Amazon.Bedrock.Exceptions;

public class BedrockException(string message, HttpStatusCode httpStatusCode) 
    : AwsException(message, httpStatusCode)
{
    public bool WasThrottled => HttpStatusCode is HttpStatusCode.TooManyRequests || Message.Contains("Too many");

    public bool IsTransient => WasThrottled || HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable;

    internal static async Task<BedrockException> FromHttpResponseAsync(HttpResponseMessage response)
    {
        byte[] responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        if (response.Content.Headers.ContentType?.MediaType is MediaTypeNames.Application.Json)
        {
            if (JsonSerializer.Deserialize<Error>(responseBytes) is Error { Message: string errorMessage })
            {
                return new BedrockException(errorMessage, response.StatusCode);
            }
        }

        return new BedrockException(Encoding.UTF8.GetString(responseBytes), response.StatusCode);
    }
}

// Documentation -
// https://docs.aws.amazon.com/bedrock/latest/userguide/troubleshooting-api-error-codes.html