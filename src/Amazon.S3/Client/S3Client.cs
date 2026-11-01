using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

using Carbon.Storage;

namespace Amazon.S3;

public class S3Client : AwsClient
{
    public const string Namespace = "http://s3.amazonaws.com/doc/2006-03-01/";

    public S3Client(AwsRegion region, IAwsCredential credential)
        : this(region, host: $"s3.dualstack.{region.Name}.amazonaws.com", credential: credential) { }

    public S3Client(AwsRegion region, string host, IAwsCredential credential)
        : this(region, host, credential, HttpClientFactory.Create()) { }

    public S3Client(AwsRegion region, string host, IAwsCredential credential, HttpClient httpClient)
      : base(AwsService.S3, region, credential, httpClient)
    {
        ArgumentNullException.ThrowIfNull(host);

        Host = host;
    }

    public S3Client WithTimeout(TimeSpan timeout)
    {
        _httpClient.Timeout = timeout;

        return this;
    }

    public string Host { get; }

    // TODO: CreateBucketAsync

    public async Task<ListBucketResult> ListBucketAsync(string bucketName, ListBucketOptions options, CancellationToken cancellationToken = default)
    {
        var request = new ListBucketRequest(Host, bucketName, options);

        return await SendXmlAsync<ListBucketResult>(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ListVersionsResult> ListObjectVersionsAsync(string bucketName, ListVersionsOptions options, CancellationToken cancellationToken = default)
    {
        var request = new ListVersionsRequest(Host, bucketName, options);

        return await SendXmlAsync<ListVersionsResult>(request, cancellationToken).ConfigureAwait(false);
    }

    #region Multipart Uploads

    public Task AbortMultipartUploadAsync(AbortMultipartUploadRequest request)
    {
        return SendAsync(request);
    }

    public async Task<InitiateMultipartUploadResult> InitiateMultipartUploadAsync(InitiateMultipartUploadRequest request)
    {
        byte[] responseText = await SendAsync(request).ConfigureAwait(false);

        return InitiateMultipartUploadResult.Deserialize(responseText);
    }

    public async Task<UploadPartResult> UploadPartAsync(UploadPartRequest request, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await SendS3RequestAsync(request, request.CompletionOption, cancellationToken).ConfigureAwait(false);

        return new UploadPartResult(
            uploadId   : request.UploadId,
            partNumber : request.PartNumber,
            eTag       : response.Headers.ETag!.Tag
        );
    }

    public async Task<CompleteMultipartUploadResult> CompleteMultipartUploadAsync(CompleteMultipartUploadRequest request, CancellationToken cancellationToken = default)
    {
        return await SendXmlAsync<CompleteMultipartUploadResult>(request, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    public async Task<PutObjectResult> PutObjectAsync(PutObjectRequest request, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await SendS3RequestAsync(request, request.CompletionOption, cancellationToken).ConfigureAwait(false);

        string? versionId = response.Headers.GetValueOrDefault(S3HeaderNames.VersionId);

        return new PutObjectResult {
            ETag      = response.Headers.ETag!.Tag!,
            VersionId = versionId
        };
    }

    public async Task<CopyObjectResult> CopyObjectAsync(CopyObjectRequest request, CancellationToken cancellationToken = default)
    {
        return await SendXmlAsync<CopyObjectResult>(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<DeleteObjectResult> DeleteObjectAsync(DeleteObjectRequest request, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await SendS3RequestAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode is not HttpStatusCode.NoContent)
        {
            throw new S3Exception("Expected 204", response.StatusCode);
        }

        return new DeleteObjectResult(
            deleteMarker   : response.Headers.GetValueOrDefault(S3HeaderNames.DeleteMarker),
            requestCharged : response.Headers.GetValueOrDefault(S3HeaderNames.RequestCharged),
            versionId      : response.Headers.GetValueOrDefault(S3HeaderNames.VersionId)
        );
    }

    public async Task<DeleteResult> DeleteObjectsAsync(DeleteObjectsRequest request, CancellationToken cancellationToken = default)
    {
        return await SendXmlAsync<DeleteResult>(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<RestoreObjectResult> RestoreObjectAsync(RestoreObjectRequest request, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await SendS3RequestAsync(request, request.CompletionOption, cancellationToken).ConfigureAwait(false);

        return new RestoreObjectResult(response.StatusCode);
    }

    public async Task<S3Object> GetObjectAsync(GetObjectRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendS3RequestAsync(request, request.CompletionOption, cancellationToken).ConfigureAwait(false);

        return new S3Object(request.ObjectName!, response);
    }

    public async Task<S3ObjectInfo> GetObjectHeadAsync(ObjectHeadRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await SendS3RequestAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        return S3ObjectInfo.FromResponse(request.BucketName, request.ObjectName!, response);
    }

    protected async Task<HttpResponseMessage> SendS3RequestAsync(
        HttpRequestMessage request,
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken)
    {
        await SignAsync(request).ConfigureAwait(false);

        var response = await _httpClient.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode is HttpStatusCode.NotModified)
        {
            return response;
        }

        if (!response.IsSuccessStatusCode)
        {
            using (response)
            {
                throw await GetExceptionAsync(response).ConfigureAwait(false);
            }
        }

        return response;
    }

    private async Task<T> SendXmlAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        where T : class
    {
        using HttpResponseMessage response = await SendS3RequestAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        byte[] buffer = ArrayPool<byte>.Shared.Rent(65_536); // 64KiB

        try
        {
            int length = 0;

            // HttpClient.Timeout stops applying once the headers are read
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            timeout.CancelAfter(_httpClient.Timeout);

            try
            {
                using Stream stream = await response.Content.ReadAsStreamAsync(timeout.Token).ConfigureAwait(false);

                int read;

                while ((read = await stream.ReadAsync(buffer.AsMemory(length), timeout.Token).ConfigureAwait(false)) > 0)
                {
                    length += read;

                    if (length == buffer.Length)
                    {
                        byte[] larger = ArrayPool<byte>.Shared.Rent(buffer.Length * 2);

                        Buffer.BlockCopy(buffer, 0, larger, 0, length);

                        ArrayPool<byte>.Shared.Return(buffer);

                        buffer = larger;
                    }
                }
            }
            catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException($"The response was not read within the configured HttpClient.Timeout of {_httpClient.Timeout.TotalSeconds} seconds.", new TimeoutException(ex.Message, ex));
            }
            catch (IOException ex)
            {
                // Matches HttpClient when it buffers the response, which the retry policies rely on
                throw new HttpRequestException("Error while reading the response content.", ex);
            }

            // XmlSerializer reads synchronously, so it reads the buffered response rather than the network
            using var xmlStream = new MemoryStream(buffer, 0, length, writable: false);

            return S3Serializer<T>.Deserialize(xmlStream);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public string GetPresignedUrl(GetPresignedUrlRequest request)
    {
        return S3Helper.GetPresignedUrl(request, _credential);
    }

    #region Helpers

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            string key = response.RequestMessage!.RequestUri!.AbsolutePath;

            if (key.Length > 0 && key[0] is '/')
            {
                key = key.Substring(1);
            }

            throw StorageException.NotFound(key);
        }

        byte[] responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        // Wasabi returns a non-standard ErrorResponse
        if (responseBytes.AsSpan().IndexOf("<ErrorResponse"u8) > -1)
        {
            if (S3Serializer<S3ErrorResponse>.TryDeserialize(responseBytes, out var wasabiError))
            {
                throw new S3Exception(wasabiError.Error, response.StatusCode);
            }
        }

        else if (responseBytes.AsSpan().IndexOf("<Error>"u8) > -1 && S3Error.TryDeserialize(responseBytes, out var error))
        {
            throw new S3Exception(error, response.StatusCode);
        }

        throw new S3Exception($"Unexpected S3 error. status = {response.StatusCode} | {Encoding.UTF8.GetString(responseBytes)}", response.StatusCode);
    }

    #endregion
}