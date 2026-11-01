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

    public async Task<CreateAliasResult> CreateAliasAsync(CreateAliasRequest request)
    {
        // A successful response has an empty body
        await SendAsync("CreateAlias", request, KmsSerializerContext.Default.CreateAliasRequest).ConfigureAwait(false);

        return new CreateAliasResult();
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

    public Task RevokeGrantAsync(RevokeGrantRequest request)
    {
        return SendAsync("RevokeGrant", request, KmsSerializerContext.Default.RevokeGrantRequest);
    }

    public Task<ListGrantsResult> ListGrantsAsync(ListGrantsRequest request)
    {
        return SendAsync("ListGrants", request, KmsSerializerContext.Default.ListGrantsRequest, KmsSerializerContext.Default.ListGrantsResult);
    }

    #endregion

    public Task<DecryptResult> DecryptAsync(DecryptRequest request)
    {
        return SendAsync("Decrypt", request, KmsSerializerContext.Default.DecryptRequest, KmsSerializerContext.Default.DecryptResult);
    }

    public Task<EncryptResult> EncryptAsync(EncryptRequest request)
    {
        return SendAsync("Encrypt", request, KmsSerializerContext.Default.EncryptRequest, KmsSerializerContext.Default.EncryptResult);
    }

    public Task<SignResult> SignAsync(SignRequest request)
    {
        return SendAsync("Sign", request, KmsSerializerContext.Default.SignRequest, KmsSerializerContext.Default.SignResult);
    }

    public Task<VerifyResult> VerifyAsync(VerifyRequest request)
    {
        return SendAsync("Verify", request, KmsSerializerContext.Default.VerifyRequest, KmsSerializerContext.Default.VerifyResult);
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

        if (ParseError(responseBytes) is not { } error)
        {
            return new AwsException(Encoding.UTF8.GetString(responseBytes), response.StatusCode);
        }

        return GetErrorCode(error.Type) switch
        {
            "AccessDeniedException"       => new AccessDeniedException(error, response.StatusCode),
            "DependencyTimeoutException"  => new DependencyTimeoutException(error, response.StatusCode),
            "KMSInternalException"        => new KmsInternalException(error, response.StatusCode),
            "ServiceUnavailableException" => error.Message is null ? new ServiceUnavailableException() : new ServiceUnavailableException(error.Message),
            "KeyUnavailableException"     => new KeyUnavailableException(error, response.StatusCode),
            "ValidationException"         => new KmsValidationException(error, response.StatusCode),
            "DryRunOperationException"    => new DryRunOperationException(error),
            _                             => new KmsException(error, response.StatusCode)
        };
    }

    private static KmsError? ParseError(byte[] responseBytes)
    {
        if (responseBytes.Length is 0 || responseBytes[0] is not (byte)'{')
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize(responseBytes, KmsSerializerContext.Default.KmsError);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    // __type may be a shape name, a full shape id, or either followed by a colon and a URL
    // https://smithy.io/2.0/aws/protocols/aws-json-1_1-protocol.html
    private static string? GetErrorCode(string? type)
    {
        if (type is null) return null;

        int colonIndex = type.IndexOf(':');

        if (colonIndex > -1)
        {
            type = type[..colonIndex];
        }

        return type[(type.IndexOf('#') + 1)..];
    }

    #endregion
}

// http://docs.aws.amazon.com/kms/latest/APIReference/Welcome.html