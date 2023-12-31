using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        return SendAsync<CreateAliasRequest, CreateAliasResult>("CreateAlias", request);
    }

    #endregion

    #region Grants

    public Task<CreateGrantResult> CreateGrantAsync(CreateGrantRequest request)
    {
        return SendAsync<CreateGrantRequest, CreateGrantResult>("CreateGrant", request);
    }

    public Task RetireGrantAsync(RetireGrantRequest request)
    {
        return SendAsync("RetireGrant", request);
    }

    public Task<ListGrantsResult> ListGrantsAsync(ListGrantsRequest request)
    {
        return SendAsync<ListGrantsRequest, ListGrantsResult>("ListGrants", request);
    }

    #endregion

    public Task<EncryptResult> EncryptAsync(EncryptRequest request)
    {
        return SendAsync<EncryptRequest, EncryptResult>("Encrypt", request);
    }

    public Task<DecryptResult> DecryptAsync(DecryptRequest request)
    {
        return SendAsync<DecryptRequest, DecryptResult>("Decrypt", request);
    }

    #region Data Keys

    public Task<GenerateDataKeyResult> GenerateDataKeyAsync(GenerateDataKeyRequest request)
    {
        return SendAsync<GenerateDataKeyRequest, GenerateDataKeyResult>("GenerateDataKey", request);
    }

    public Task<GenerateDataKeyResult> GenerateDataKeyWithoutPlaintextAsync(GenerateDataKeyRequest request)
    {
        return SendAsync<GenerateDataKeyRequest, GenerateDataKeyResult>("GenerateDataKeyWithoutPlaintext", request);
    }

    #endregion

    #region Helpers

    private static readonly JsonSerializerOptions s_jso = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private async Task<TResult> SendAsync<TRequest, TResult>(string action, TRequest request)
        where TRequest : KmsRequest
        where TResult : KmsResult
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, s_jso);

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

    private async Task SendAsync<TRequest>(string action, TRequest request)
      where TRequest : KmsRequest
    {
        byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(request, s_jso);

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