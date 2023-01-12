using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Exceptions;
using Amazon.Kms.Exceptions;

namespace Amazon.Kms;

public sealed class KmsClient : AwsClient
{
    public const string Version = "2014-11-01";

    public KmsClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Kms, region, credential)
    { }

    #region Aliases

    public Task<CreateAliasResponse> CreateAliasAsync(CreateAliasRequest request)
    {
        return SendAsync<CreateAliasRequest, CreateAliasResponse>("CreateAlias", request);
    }

    #endregion

    #region Grants

    public Task<CreateGrantResponse> CreateGrantAsync(CreateGrantRequest request)
    {
        return SendAsync<CreateGrantRequest, CreateGrantResponse>("CreateGrant", request);
    }

    public Task<RetireGrantResponse> RetireGrantAsync(RetireGrantRequest request)
    {
        return SendAsync<RetireGrantRequest, RetireGrantResponse>("RetireGrant", request);
    }

    public Task<ListGrantsResponse> ListGrantsAsync(ListGrantsRequest request)
    {
        return SendAsync<ListGrantsRequest, ListGrantsResponse>("ListGrants", request);
    }

    #endregion

    public Task<EncryptResponse> EncryptAsync(EncryptRequest request)
    {
        return SendAsync<EncryptRequest, EncryptResponse>("Encrypt", request);
    }

    public Task<DecryptResponse> DecryptAsync(DecryptRequest request)
    {
        return SendAsync<DecryptRequest, DecryptResponse>("Decrypt", request);
    }

    #region Data Keys

    public Task<GenerateDataKeyResponse> GenerateDataKeyAsync(GenerateDataKeyRequest request)
    {
        return SendAsync<GenerateDataKeyRequest, GenerateDataKeyResponse>("GenerateDataKey", request);
    }

    public Task<GenerateDataKeyResponse> GenerateDataKeyWithoutPlaintextAsync(GenerateDataKeyRequest request)
    {
        return SendAsync<GenerateDataKeyRequest, GenerateDataKeyResponse>("GenerateDataKeyWithoutPlaintext", request);
    }

    #endregion

    #region Helpers

    private static readonly JsonSerializerOptions jso = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private async Task<TResult> SendAsync<TRequest, TResult>(string action, TRequest request)
        where TRequest : KmsRequest
        where TResult : KmsResponse
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, jso);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Headers = {
                { "x-amz-target", $"TrentService.{action}" }
            },
            Content = new ByteArrayContent(jsonBytes) {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.1" }
                }
            }
        };

        await SignAsync(httpRequest).ConfigureAwait(false);

        using var response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionFromResponseAsync(response).ConfigureAwait(false);
        }

        if (response.StatusCode is HttpStatusCode.NoContent || response.Content.Headers.ContentLength is 0)
        {
            return null!;
        }

        var result = await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);

        return result!;
    }

    private static async Task<Exception> GetExceptionFromResponseAsync(HttpResponseMessage response)
    {
        byte[] responseText = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        if (responseText.Length > 0 && responseText[0] is (byte)'{')
        {
            var error = JsonSerializer.Deserialize<KmsError>(responseText)!;

            return error.Type switch
            {
                "AccessDeniedException"       => new AccessDeniedException(error, response.StatusCode),
                "ServiceUnavailableException" => new ServiceUnavailableException(), // TODO: Provide the message
                "KeyUnavailableException"     => new KeyUnavailableException(error, response.StatusCode),
                _ => new KmsException(error, response.StatusCode)
            };
        }
        else
        {
            throw new AwsException(Encoding.UTF8.GetString(responseText), response.StatusCode);
        }
    }

    #endregion
}

// http://docs.aws.amazon.com/kms/latest/APIReference/Welcome.html