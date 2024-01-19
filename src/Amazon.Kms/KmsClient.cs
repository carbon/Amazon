using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Amazon.Exceptions;
using Amazon.Kms.Exceptions;
using Amazon.Kms.Serialization;

namespace Amazon.Kms;

public sealed class KmsClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Kms, region, credential)
{
    public const string Version = "2014-11-01";

    #region Aliases

    public Task<CreateAliasResult> CreateAliasAsync(CreateAliasRequest request)
    {
        return SendAsync("CreateAlias", request, KmsSerializerContext.Default.CreateAliasRequest, KmsSerializerContext.Default.CreateAliasResult);
    }

    #endregion

    #region Grants

    public Task<CreateGrantResult> CreateGrantAsync(CreateGrantRequest request)
    {
        return SendAsync("CreateGrant", request, KmsSerializerContext.Default.CreateGrantRequest, KmsSerializerContext.Default.CreateGrantResult);
    }

    public Task RetireGrantAsync(RetireGrantRequest request)
    {
        return SendAsync("RetireGrant", request, KmsSerializerContext.Default.RetireGrantRequest);
    }

    public Task<ListGrantsResult> ListGrantsAsync(ListGrantsRequest request)
    {
        return SendAsync("ListGrants", request, KmsSerializerContext.Default.ListGrantsRequest, KmsSerializerContext.Default.ListGrantsResult);
    }

    #endregion

    public Task<EncryptResult> EncryptAsync(EncryptRequest request)
    {
        return SendAsync("Encrypt", request, KmsSerializerContext.Default.EncryptRequest, KmsSerializerContext.Default.EncryptResult);
    }

    public Task<DecryptResult> DecryptAsync(DecryptRequest request)
    {
        return SendAsync("Decrypt", request, KmsSerializerContext.Default.DecryptRequest, KmsSerializerContext.Default.DecryptResult);
    }

    #region Data Keys

    public Task<GenerateDataKeyResult> GenerateDataKeyAsync(GenerateDataKeyRequest request)
    {
        return SendAsync("GenerateDataKey", request, KmsSerializerContext.Default.GenerateDataKeyRequest, KmsSerializerContext.Default.GenerateDataKeyResult);
    }

    public Task<GenerateDataKeyResult> GenerateDataKeyWithoutPlaintextAsync(GenerateDataKeyRequest request)
    {
        return SendAsync("GenerateDataKeyWithoutPlaintext", request, KmsSerializerContext.Default.GenerateDataKeyRequest, KmsSerializerContext.Default.GenerateDataKeyResult);
    }

    #endregion

    #region Helpers

    private async Task<TResult> SendAsync<TRequest, TResult>(
        string action,
        TRequest request,
        JsonTypeInfo<TRequest> requestJsonType,
        JsonTypeInfo<TResult> resultJsonType)
        where TRequest : KmsRequest
        where TResult : KmsResult
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, requestJsonType);

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

        var result = await response.Content.ReadFromJsonAsync(resultJsonType).ConfigureAwait(false);

        return result!;
    }

    private async Task SendAsync<TRequest>(string action, TRequest request, JsonTypeInfo<TRequest> jsonRequestType)
      where TRequest : KmsRequest
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, jsonRequestType);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
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
    }

    private static async Task<Exception> GetExceptionFromResponseAsync(HttpResponseMessage response)
    {
        byte[] responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        if (responseBytes.Length > 0 && responseBytes[0] is (byte)'{')
        {
            KmsError error = JsonSerializer.Deserialize(responseBytes, KmsSerializerContext.Default.KmsError)!;

            return error.Type switch
            {
                "AccessDeniedException"       => new AccessDeniedException(error, response.StatusCode),
                "ServiceUnavailableException" => new ServiceUnavailableException(), // TODO: Provide the message
                "KeyUnavailableException"     => new KeyUnavailableException(error, response.StatusCode),
                "ValidationException"         => new KmsValidationException(error, response.StatusCode),
                _                             => new KmsException(error, response.StatusCode)
            };
        }
        else
        {
            throw new AwsException(Encoding.UTF8.GetString(responseBytes), response.StatusCode);
        }
    }

    #endregion
}

// http://docs.aws.amazon.com/kms/latest/APIReference/Welcome.html