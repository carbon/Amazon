using System.Security.Cryptography;
using System.Text;

using Amazon.Security;

namespace Amazon.S3.Vectors.Tests;

public class RoutingTests
{
    private const string BucketArn = FakeS3Vectors.BucketArn;

    private const string DoubleEncodedBucketArn = "arn%253Aaws%253As3vectors%253Aus-east-1%253A123456789012%253Abucket%252Fmedia";

    public static TheoryData<string, Func<AmazonS3VectorsClient, Task>> PostOperations => new()
    {
        { "/CreateIndex", c => c.CreateIndexAsync(new() { IndexName = "i", DataType = Models.VectorDataType.Float32, Dimension = 1, DistanceMetric = Models.DistanceMetric.Cosine }) },
        { "/CreateVectorBucket", c => c.CreateVectorBucketAsync(new() { VectorBucketName = "b" }) },
        { "/DeleteIndex", c => c.DeleteIndexAsync(new()) },
        { "/DeleteVectorBucket", c => c.DeleteVectorBucketAsync(new()) },
        { "/DeleteVectorBucketPolicy", c => c.DeleteVectorBucketPolicyAsync(new()) },
        { "/DeleteVectors", c => c.DeleteVectorsAsync(new() { Keys = ["k"] }) },
        { "/GetIndex", c => c.GetIndexAsync(new()) },
        { "/GetVectorBucket", c => c.GetVectorBucketAsync(new()) },
        { "/GetVectorBucketPolicy", c => c.GetVectorBucketPolicyAsync(new()) },
        { "/GetVectors", c => c.GetVectorsAsync(new() { Keys = ["k"] }) },
        { "/ListIndexes", c => c.ListIndexesAsync(new()) },
        { "/ListVectorBuckets", c => c.ListVectorBucketsAsync(new()) },
        { "/ListVectors", c => c.ListVectorsAsync(new()) },
        { "/PutVectorBucketDefaultIndexMode", c => c.PutVectorBucketDefaultIndexModeAsync(new() { DefaultIndexMode = Models.IndexMode.Enhanced }) },
        { "/UpdateIndexMode", c => c.UpdateIndexModeAsync(new() { IndexMode = Models.IndexMode.Enhanced }) },
        { "/PutVectorBucketPolicy", c => c.PutVectorBucketPolicyAsync(new() { Policy = "{}" }) },
        { "/PutVectors", c => c.PutVectorsAsync(new() { Vectors = [] }) },
        { "/QueryVectors", c => c.QueryVectorsAsync(new() { QueryVector = new float[] { 1 }, TopK = 1 }) },
    };

    [Theory]
    [MemberData(nameof(PostOperations))]
    public async Task PostOperationsUseTheirDocumentedPath(string path, Func<AmazonS3VectorsClient, Task> operation)
    {
        var handler = new FakeHandler(FakeS3Vectors.Json("{}"));

        await operation(FakeS3Vectors.Client(handler));

        var request = handler.Request;

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://s3vectors.us-east-1.api.aws" + path, request.RequestUri.AbsoluteUri);
        Assert.Equal("application/json", request.MediaType);
    }

    [Theory]
    [InlineData("us-east-1")]
    [InlineData("eu-central-1")]
    public async Task RequestsGoToTheRegionalApiAwsHost(string region)
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler, region).DeleteVectorBucketAsync(new() { VectorBucketName = "b" });

        Assert.Equal($"s3vectors.{region}.api.aws", handler.Request.RequestUri.Host);
        Assert.Equal("https", handler.Request.RequestUri.Scheme);
    }

    [Fact]
    public async Task RequestsAreSignedForTheS3VectorsService()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler, "eu-west-1").DeleteVectorBucketAsync(new() { VectorBucketName = "b" });

        var request = handler.Request;

        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/", request.Authorization);
        Assert.Contains("/eu-west-1/s3vectors/aws4_request", request.Authorization);
        Assert.Equal("application/json", request.Accept);
        Assert.True(request.HasAmzDate);
    }

    [Fact]
    public async Task ListTagsPutsTheEscapedArnInThePath()
    {
        var handler = new FakeHandler(FakeS3Vectors.Json("""{ "tags": { "team": "search" } }"""));

        var result = await FakeS3Vectors.Client(handler).ListTagsForResourceAsync(new() { ResourceArn = BucketArn });

        Assert.Equal(HttpMethod.Get, handler.Request.Method);
        Assert.Equal("/tags/" + FakeS3Vectors.EncodedBucketArn, handler.Request.RequestUri.AbsolutePath);
        Assert.Equal("search", result.Tags["team"]);
    }

    [Fact]
    public async Task TagResourcePostsTheTagsToTheArnPath()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler).TagResourceAsync(new() {
            ResourceArn = BucketArn,
            Tags = new() { ["team"] = "search" }
        });

        Assert.Equal(HttpMethod.Post, handler.Request.Method);
        Assert.Equal("/tags/" + FakeS3Vectors.EncodedBucketArn, handler.Request.RequestUri.AbsolutePath);
        Assert.Equal("search", handler.Request.Json.GetProperty("tags").GetProperty("team").GetString());
    }

    [Fact]
    public async Task UntagResourceRepeatsAndEscapesTagKeysInTheQuery()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler).UntagResourceAsync(new() {
            ResourceArn = BucketArn,
            TagKeys = ["team", "cost center"]
        });

        Assert.Equal(HttpMethod.Delete, handler.Request.Method);
        Assert.Equal("/tags/" + FakeS3Vectors.EncodedBucketArn, handler.Request.RequestUri.AbsolutePath);
        Assert.Equal("?tagKeys=team&tagKeys=cost%20center", handler.Request.RequestUri.Query);
    }

    // API reference: "The request does not have a request body."
    [Fact]
    public async Task ListTagsSendsNoBody()
    {
        var handler = new FakeHandler(FakeS3Vectors.Json("""{ "tags": {} }"""));

        await FakeS3Vectors.Client(handler).ListTagsForResourceAsync(new() { ResourceArn = BucketArn });

        Assert.Null(handler.Request.Body);
    }

    // API reference: "The request does not have a request body."
    [Fact]
    public async Task UntagResourceSendsNoBody()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler).UntagResourceAsync(new() { ResourceArn = BucketArn, TagKeys = ["team"] });

        Assert.Null(handler.Request.Body);
    }

    // API reference: the body of TagResource holds "tags" only, resourceArn is a URI parameter
    [Fact]
    public async Task TagResourceBodyHoldsOnlyTheTags()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler).TagResourceAsync(new() { ResourceArn = BucketArn, Tags = new() { ["team"] = "search" } });

        Assert.False(handler.Request.Json.TryGetProperty("resourceArn", out _));
    }

    // SigV4 encodes each path segment a second time for every service except S3
    [Fact(Skip = "Bug: SignerV4.WriteCanonicalizedUri leaves segments containing '%' as they are, so ListTagsForResource is signed with a singly encoded ARN and should fail with SignatureDoesNotMatch")]
    public async Task ListTagsSignatureCoversTheDoubleEncodedPath()
    {
        var handler = new FakeHandler(FakeS3Vectors.Json("""{ "tags": {} }"""));

        await FakeS3Vectors.Client(handler).ListTagsForResourceAsync(new() { ResourceArn = BucketArn });

        AssertSignature(handler.Request, "GET", "/tags/" + DoubleEncodedBucketArn, query: "");
    }

    [Fact(Skip = "Bug: SignerV4.WriteCanonicalizedUri leaves segments containing '%' as they are, so UntagResource is signed with a singly encoded ARN and should fail with SignatureDoesNotMatch")]
    public async Task UntagResourceSignatureCoversEveryTagKey()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler).UntagResourceAsync(new() { ResourceArn = BucketArn, TagKeys = ["b", "a"] });

        AssertSignature(handler.Request, "DELETE", "/tags/" + DoubleEncodedBucketArn, query: "tagKeys=a&tagKeys=b");
    }

    // Recomputes the signature from a hand-written canonical request, for requests with no body
    private static void AssertSignature(RecordedRequest request, string method, string canonicalUri, string query)
    {
        const string emptySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        string timestamp = request.AmzDate!;

        var scope = new CredentialScope(
            DateOnly.ParseExact(timestamp[..8], "yyyyMMdd"),
            new AwsRegion("us-east-1"),
            AwsService.S3Vectors);

        string canonicalRequest = $"{method}\n{canonicalUri}\n{query}\nhost:s3vectors.us-east-1.api.aws\nx-amz-date:{timestamp}\n\nhost;x-amz-date\n{emptySha256}";

        byte[] signature = HMACSHA256.HashData(
            SignerV4.ComputeSigningKey("secret", scope),
            Encoding.UTF8.GetBytes(SignerV4.GetStringToSign(scope, timestamp, canonicalRequest)));

        Assert.EndsWith(",SignedHeaders=host;x-amz-date,Signature=" + Convert.ToHexStringLower(signature), request.Authorization);
    }

    // SigV4 signs every query parameter, sorted by name and then by value
    [Fact]
    public async Task EveryTagKeyIsInTheCanonicalQuery()
    {
        var handler = new FakeHandler(FakeS3Vectors.Empty());

        await FakeS3Vectors.Client(handler).UntagResourceAsync(new() { ResourceArn = BucketArn, TagKeys = ["a", "b"] });

        Assert.Equal("tagKeys=a&tagKeys=b", SignerV4.CanonicalizeQueryString(handler.Request.RequestUri));
    }

    [Fact]
    public void EndpointPropertyMatchesTheHostInUse()
    {
        var client = FakeS3Vectors.Client(new FakeHandler());

        Assert.Equal("https://s3vectors.us-east-1.api.aws/", client.Endpoint);
    }
}
