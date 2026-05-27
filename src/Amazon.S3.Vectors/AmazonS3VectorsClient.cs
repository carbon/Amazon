using System.Net.Http.Json;

using Amazon.Exceptions;

namespace Amazon.S3.Vectors;

public class AmazonS3VectorsClient : AwsClient
{
    private readonly Uri _endpoint;

    public AmazonS3VectorsClient(AwsRegion region, IAwsCredential credential)
        : base("s3vectors", region, credential)
    {
        _endpoint = new Uri($"https://s3vectors.{region.Name}.api.aws");
    }

    public Task<CreateIndexResult> CreateIndexAsync(
    CreateIndexRequest request,
    CancellationToken cancellationToken = default)
    {
        return SendAsync<CreateIndexRequest, CreateIndexResult>(HttpMethod.Post, "/CreateIndex", request, cancellationToken);
    }

    public Task<CreateVectorBucketResult> CreateVectorBucketAsync(
        CreateVectorBucketRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<CreateVectorBucketRequest, CreateVectorBucketResult>(HttpMethod.Post, "/CreateVectorBucket", request, cancellationToken);
    }

    public Task<DeleteIndexResult> DeleteIndexAsync(
        DeleteIndexRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<DeleteIndexRequest, DeleteIndexResult>(HttpMethod.Post, "/DeleteIndex", request, cancellationToken);
    }

    public Task<DeleteVectorBucketResult> DeleteVectorBucketAsync(
        DeleteVectorBucketRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<DeleteVectorBucketRequest, DeleteVectorBucketResult>(HttpMethod.Post, "/DeleteVectorBucket", request, cancellationToken);
    }

    public Task<DeleteVectorBucketPolicyResult> DeleteVectorBucketPolicyAsync(
        DeleteVectorBucketPolicyRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<DeleteVectorBucketPolicyRequest, DeleteVectorBucketPolicyResult>(HttpMethod.Post, "/DeleteVectorBucketPolicy", request, cancellationToken);
    }

    public Task<DeleteVectorsResult> DeleteVectorsAsync(
        DeleteVectorsRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<DeleteVectorsRequest, DeleteVectorsResult>(HttpMethod.Post, "/DeleteVectors", request, cancellationToken);
    }

    public Task<GetIndexResult> GetIndexAsync(
        GetIndexRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<GetIndexRequest, GetIndexResult>(HttpMethod.Post, "/GetIndex", request, cancellationToken);
    }

    public Task<GetVectorBucketResult> GetVectorBucketAsync(
        GetVectorBucketRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<GetVectorBucketRequest, GetVectorBucketResult>(HttpMethod.Post, "/GetVectorBucket", request, cancellationToken);
    }

    public Task<GetVectorBucketPolicyResult> GetVectorBucketPolicyAsync(
        GetVectorBucketPolicyRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<GetVectorBucketPolicyRequest, GetVectorBucketPolicyResult>(HttpMethod.Post, "/GetVectorBucketPolicy", request, cancellationToken);
    }

    public Task<GetVectorsResult> GetVectorsAsync(
        GetVectorsRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<GetVectorsRequest, GetVectorsResult>(HttpMethod.Post, "/GetVectors", request, cancellationToken);
    }

    public Task<ListIndexesResult> ListIndexesAsync(
        ListIndexesRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<ListIndexesRequest, ListIndexesResult>(HttpMethod.Post, "/ListIndexes", request, cancellationToken);
    }

    public Task<ListTagsForResourceResult> ListTagsForResourceAsync(
        ListTagsForResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<ListTagsForResourceRequest, ListTagsForResourceResult>
            (HttpMethod.Get,
            "/tags/" + Uri.EscapeDataString(request.ResourceArn),
            request,
            cancellationToken);
    }

    public Task<ListVectorBucketsResult> ListVectorBucketsAsync(
        ListVectorBucketsRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<ListVectorBucketsRequest, ListVectorBucketsResult>(HttpMethod.Post, "/ListVectorBuckets", request, cancellationToken);
    }

    public Task<ListVectorsResult> ListVectorsAsync(
        ListVectorsRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<ListVectorsRequest, ListVectorsResult>(HttpMethod.Post, "/ListVectors", request, cancellationToken);
    }

    public Task<PutVectorBucketPolicyResult> PutVectorBucketPolicyAsync(
        PutVectorBucketPolicyRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<PutVectorBucketPolicyRequest, PutVectorBucketPolicyResult>(HttpMethod.Post, "/PutVectorBucketPolicy", request, cancellationToken);
    }

    public Task<PutVectorsResult> PutVectorsAsync(
        PutVectorsRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<PutVectorsRequest, PutVectorsResult>(HttpMethod.Post, "/PutVectors", request, cancellationToken);
    }

    public Task<QueryVectorsResult> QueryVectorsAsync(
        QueryVectorsRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<QueryVectorsRequest, QueryVectorsResult>(HttpMethod.Post, "/QueryVectors", request, cancellationToken);
    }

    public Task<TagResourceResult> TagResourceAsync(
        TagResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<TagResourceRequest, TagResourceResult>(
            HttpMethod.Post,
            "/tags/" + Uri.EscapeDataString(request.ResourceArn),
            request,
            cancellationToken);
    }

    public Task<UntagResourceResult> UntagResourceAsync(
        UntagResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<UntagResourceRequest, UntagResourceResult>(
            HttpMethod.Delete,
            "/tags/" + Uri.EscapeDataString(request.ResourceArn) + "?tagKeys=" + string.Join("&tagKeys=", request.TagKeys.Select(Uri.EscapeDataString)),
            request,
            cancellationToken);
    }

    // Sender -

    private async Task<TResponse> SendAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest? request,
        CancellationToken cancellationToken)
     where TResponse : class, new()
    {
        using var httpRequest = new HttpRequestMessage(method, new Uri(_endpoint, path));

        httpRequest.Headers.Accept.ParseAdd("application/json");

        if (request is not null)
        {
            httpRequest.Content = JsonContent.Create(request);
        }

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient
            .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            string error = response.Content is null
                ? ""
                : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            throw new AwsException(error,response.StatusCode);
        }

        if (response.Content is null ||
            response.Content.Headers.ContentLength is 0)
        {
            return new TResponse();
        }

        TResponse? result = await response.Content
            .ReadFromJsonAsync<TResponse>(cancellationToken)
            .ConfigureAwait(false);

        return result ?? new TResponse();
    }
}
