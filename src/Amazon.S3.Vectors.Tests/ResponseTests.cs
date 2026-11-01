using System.Net;
using System.Text.Json;

using Amazon.Exceptions;
using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors.Tests;

public class ResponseTests
{
    private static readonly DateTimeOffset s_created = DateTimeOffset.FromUnixTimeSeconds(1752624000);

    private static AmazonS3VectorsClient Client(string response)
    {
        return FakeS3Vectors.Client(new FakeHandler(FakeS3Vectors.Json(response)));
    }

    [Fact]
    public async Task CreateResultsCarryTheArn()
    {
        var bucket = await Client("""{ "vectorBucketArn": "bucket-arn" }""").CreateVectorBucketAsync(new() { VectorBucketName = "b" });

        Assert.Equal("bucket-arn", bucket.VectorBucketArn);

        var index = await Client("""{ "indexArn": "index-arn" }""").CreateIndexAsync(new() {
            IndexName = "i",
            DataType = VectorDataType.Float32,
            Dimension = 1,
            DistanceMetric = DistanceMetric.Cosine
        });

        Assert.Equal("index-arn", index.IndexArn);
    }

    [Fact]
    public async Task GetIndexMapsTheIndex()
    {
        var result = await Client("""
            {
              "index": {
                "vectorBucketName": "media",
                "indexName": "frames",
                "indexArn": "index-arn",
                "creationTime": 1752624000,
                "indexMode": "ENHANCED",
                "dataType": "float32",
                "dimension": 1024,
                "distanceMetric": "euclidean",
                "metadataConfiguration": { "nonFilterableMetadataKeys": ["caption"] },
                "encryptionConfiguration": { "sseType": "aws:kms", "kmsKeyArn": "key-arn" }
              }
            }
            """).GetIndexAsync(new() { IndexArn = "index-arn" });

        var index = result.Index;

        Assert.Equal("media", index.VectorBucketName);
        Assert.Equal("frames", index.IndexName);
        Assert.Equal("index-arn", index.IndexArn);
        Assert.Equal(IndexMode.Enhanced, index.IndexMode);
        Assert.Equal("float32", index.DataType);
        Assert.Equal(1024, index.Dimension);
        Assert.Equal(DistanceMetric.Euclidean, index.DistanceMetric);
        Assert.Equal(["caption"], index.MetadataConfiguration!.NonFilterableMetadataKeys);
        Assert.Equal("aws:kms", index.EncryptionConfiguration!.SseType);
        Assert.Equal("key-arn", index.EncryptionConfiguration.KmsKeyArn);
    }

    [Fact]
    public async Task GetVectorBucketMapsTheBucket()
    {
        var result = await Client("""
            {
              "vectorBucket": {
                "vectorBucketName": "media",
                "vectorBucketArn": "bucket-arn",
                "creationTime": 1752624000,
                "defaultIndexMode": "CLASSIC",
                "encryptionConfiguration": { "sseType": "AES256" }
              }
            }
            """).GetVectorBucketAsync(new() { VectorBucketName = "media" });

        Assert.Equal("media", result.VectorBucket.VectorBucketName);
        Assert.Equal("bucket-arn", result.VectorBucket.VectorBucketArn);
        Assert.Equal(s_created, result.VectorBucket.CreationTime);
        Assert.Equal(IndexMode.Classic, result.VectorBucket.DefaultIndexMode);
        Assert.Equal("AES256", result.VectorBucket.EncryptionConfiguration!.SseType);
    }

    [Fact]
    public async Task GetVectorBucketPolicyReturnsThePolicyString()
    {
        var result = await Client("""{ "policy": "{\"Version\":\"2012-10-17\"}" }""").GetVectorBucketPolicyAsync(new() { VectorBucketName = "b" });

        Assert.Equal("""{"Version":"2012-10-17"}""", result.Policy);
    }

    [Fact]
    public async Task GetVectorsMapsDataAndMetadata()
    {
        var result = await Client("""
            {
              "vectors": [
                { "key": "a", "data": { "float32": [0.5, -1.25] }, "metadata": { "genre": "scifi" } },
                { "key": "b" }
              ]
            }
            """).GetVectorsAsync(new() { Keys = ["a", "b"], ReturnData = true, ReturnMetadata = true });

        Assert.Equal("a", result.Vectors[0].Key);
        Assert.Equal([0.5f, -1.25f], result.Vectors[0].Data!.Float32!);
        Assert.Equal("scifi", result.Vectors[0].Metadata!.Value.GetProperty("genre").GetString());

        Assert.Equal("b", result.Vectors[1].Key);
        Assert.Null(result.Vectors[1].Data);
        Assert.Null(result.Vectors[1].Metadata);
    }

    [Fact]
    public async Task QueryVectorsMapsDistanceAndMetadata()
    {
        var result = await Client("""
            {
              "distanceMetric": "cosine",
              "vectors": [
                { "key": "a", "distance": 0.125, "metadata": { "genre": "scifi" } },
                { "key": "b" }
              ]
            }
            """).QueryVectorsAsync(new() { QueryVector = new float[] { 1f }, TopK = 2 });

        Assert.Equal(DistanceMetric.Cosine, result.DistanceMetric);
        Assert.Equal("a", result.Vectors[0].Key);
        Assert.Equal(0.125f, result.Vectors[0].Distance);
        Assert.Equal("scifi", result.Vectors[0].Metadata!.Value.GetProperty("genre").GetString());
        Assert.Null(result.Vectors[1].Distance);
        Assert.Null(result.Vectors[1].Metadata);
    }

    [Fact]
    public async Task ListVectorsPagesWithNextToken()
    {
        var handler = new FakeHandler(
            FakeS3Vectors.Json("""{ "vectors": [{ "key": "a", "data": { "float32": [1] }, "metadata": { "n": 1 } }], "nextToken": "page-2" }"""),
            FakeS3Vectors.Json("""{ "vectors": [{ "key": "b" }] }"""));

        var client = FakeS3Vectors.Client(handler);
        var keys = new List<string>();

        string? token = null;

        do
        {
            var page = await client.ListVectorsAsync(new() { IndexArn = "index-arn", NextToken = token });

            keys.AddRange(page.Vectors.Select(v => v.Key));

            if (page.Vectors[0].Key is "a")
            {
                Assert.Equal([1f], page.Vectors[0].Data!.Float32!);
                Assert.Equal(1, page.Vectors[0].Metadata!.Value.GetProperty("n").GetInt32());
            }

            token = page.NextToken;
        }
        while (token is not null);

        Assert.Equal(["a", "b"], keys);
        Assert.Equal(2, handler.Requests.Count);
        Assert.True(handler.Requests[0].IsAbsent("nextToken"));
        Assert.Equal("page-2", handler.Requests[1].Json.GetProperty("nextToken").GetString());
    }

    [Fact]
    public async Task ListVectorBucketsPagesWithNextToken()
    {
        var handler = new FakeHandler(
            FakeS3Vectors.Json("""{ "vectorBuckets": [{ "vectorBucketName": "a", "vectorBucketArn": "arn-a", "creationTime": 1752624000 }], "nextToken": "page-2" }"""),
            FakeS3Vectors.Json("""{ "vectorBuckets": [{ "vectorBucketName": "b", "vectorBucketArn": "arn-b", "creationTime": 1752624000 }] }"""));

        var client = FakeS3Vectors.Client(handler);

        var first = await client.ListVectorBucketsAsync(new());
        var second = await client.ListVectorBucketsAsync(new() { NextToken = first.NextToken });

        Assert.Equal("arn-a", first.VectorBuckets[0].VectorBucketArn);
        Assert.Equal("page-2", first.NextToken);
        Assert.Equal("page-2", handler.Requests[1].Json.GetProperty("nextToken").GetString());
        Assert.Equal("b", second.VectorBuckets[0].VectorBucketName);
        Assert.Null(second.NextToken);
    }

    [Fact]
    public async Task ListIndexesPagesWithNextToken()
    {
        var handler = new FakeHandler(
            FakeS3Vectors.Json("""
                {
                  "indexes": [{ "vectorBucketName": "media", "indexName": "a", "indexArn": "arn-a", "creationTime": 1752624000 }],
                  "nextToken": "page-2"
                }
                """),
            FakeS3Vectors.Json("""{ "indexes": [] }"""));

        var client = FakeS3Vectors.Client(handler);

        var first = await client.ListIndexesAsync(new() { VectorBucketName = "media" });
        var second = await client.ListIndexesAsync(new() { VectorBucketName = "media", NextToken = first.NextToken });

        var index = Assert.Single(first.Indexes);

        Assert.Equal("media", index.VectorBucketName);
        Assert.Equal("a", index.IndexName);
        Assert.Equal("arn-a", index.IndexArn);
        Assert.Equal(s_created, index.CreationTime);
        Assert.Equal("page-2", handler.Requests[1].Json.GetProperty("nextToken").GetString());
        Assert.Empty(second.Indexes);
        Assert.Null(second.NextToken);
    }

    [Fact]
    public async Task EmptyBodiesReturnAnEmptyResult()
    {
        var client = FakeS3Vectors.Client(new FakeHandler(FakeS3Vectors.Empty()));

        Assert.NotNull(await client.CreateVectorBucketAsync(new() { VectorBucketName = "b" }));
        Assert.NotNull(await client.DeleteIndexAsync(new()));
        Assert.NotNull(await client.DeleteVectorBucketAsync(new()));
        Assert.NotNull(await client.DeleteVectorBucketPolicyAsync(new()));
        Assert.NotNull(await client.DeleteVectorsAsync(new() { Keys = ["a"] }));
        Assert.NotNull(await client.PutVectorBucketDefaultIndexModeAsync(new() { DefaultIndexMode = IndexMode.Enhanced }));
        Assert.NotNull(await client.UpdateIndexModeAsync(new() { IndexMode = IndexMode.Enhanced }));
        Assert.NotNull(await client.PutVectorBucketPolicyAsync(new() { Policy = "{}" }));
        Assert.NotNull(await client.PutVectorsAsync(new() { Vectors = [] }));
        Assert.NotNull(await client.TagResourceAsync(new() { ResourceArn = "arn", Tags = [] }));
        Assert.NotNull(await client.UntagResourceAsync(new() { ResourceArn = "arn", TagKeys = ["a"] }));
    }

    [Fact]
    public async Task JsonNullBodyReturnsAnEmptyResult()
    {
        var result = await Client("null").ListVectorsAsync(new());

        Assert.Empty(result.Vectors);
    }

    [Fact]
    public async Task ErrorResponsesThrowWithStatusAndBody()
    {
        const string body = """{ "message": "No such index" }""";

        var handler = new FakeHandler(FakeS3Vectors.Json(body, HttpStatusCode.NotFound));

        var ex = await Assert.ThrowsAsync<AwsException>(() => FakeS3Vectors.Client(handler).GetIndexAsync(new() { IndexArn = "arn" }));

        Assert.Equal(HttpStatusCode.NotFound, ex.HttpStatusCode);
        Assert.Equal(body, ex.Message);
    }

    [Fact]
    public async Task ValidationErrorsKeepTheFieldListInTheBody()
    {
        const string body = """{ "message": "Invalid", "fieldList": [{ "path": "topK", "message": "must be positive" }] }""";

        var ex = await Assert.ThrowsAsync<AwsException>(() =>
            FakeS3Vectors.Client(new FakeHandler(FakeS3Vectors.Json(body, HttpStatusCode.BadRequest)))
                .QueryVectorsAsync(new() { QueryVector = new float[] { 1f }, TopK = 0 }));

        var error = JsonSerializer.Deserialize<S3VectorsErrorResult>(ex.Message)!;

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.Equal("Invalid", error.Message);
        Assert.Equal("topK", error.FieldList![0].Path);
        Assert.Equal("must be positive", error.FieldList[0].Message);
    }

    // The client has no retry policy: a throttled request surfaces straight away
    [Theory]
    [InlineData(HttpStatusCode.TooManyRequests)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task TransientErrorsAreNotRetried(HttpStatusCode statusCode)
    {
        var handler = new FakeHandler(FakeS3Vectors.Json("""{ "message": "slow down" }""", statusCode));

        var ex = await Assert.ThrowsAsync<AwsException>(() => FakeS3Vectors.Client(handler).ListVectorsAsync(new()));

        Assert.Equal(statusCode, ex.HttpStatusCode);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public void ImplicitConversionWrapsAFloatArray()
    {
        VectorData data = new float[] { 1f, 2f };

        Assert.Equal([1f, 2f], data.Float32!);
    }

    // API reference: creationTime is a number (rest-json timestamps default to epoch seconds)
    [Fact]
    public async Task GetIndexParsesCreationTime()
    {
        var result = await Client("""
            { "index": { "vectorBucketName": "media", "indexName": "frames", "indexArn": "arn", "creationTime": 1752624000, "dataType": "float32", "dimension": 3, "distanceMetric": "cosine" } }
            """).GetIndexAsync(new() { IndexArn = "arn" });

        Assert.Equal(s_created, result.Index.CreationTime);
    }

    [Fact]
    public async Task ListVectorBucketsParsesCreationTime()
    {
        var result = await Client("""
            { "vectorBuckets": [{ "vectorBucketName": "media", "vectorBucketArn": "arn", "creationTime": 1752624000 }] }
            """).ListVectorBucketsAsync(new());

        Assert.Equal(s_created, result.VectorBuckets[0].CreationTime);
    }

    [Fact]
    public async Task FractionalCreationTimeKeepsMilliseconds()
    {
        var result = await Client("""
            { "vectorBuckets": [{ "vectorBucketName": "media", "vectorBucketArn": "arn", "creationTime": 1752624000.125 }] }
            """).ListVectorBucketsAsync(new());

        Assert.Equal(s_created.AddMilliseconds(125), result.VectorBuckets[0].CreationTime);
    }

    [Fact]
    public void CreationTimeWritesEpochSeconds()
    {
        var json = JsonSerializer.Serialize(new VectorBucketSummary { VectorBucketName = "media", VectorBucketArn = "arn", CreationTime = s_created.AddMilliseconds(125) });

        Assert.Equal(1752624000.125m, JsonDocument.Parse(json).RootElement.GetProperty("creationTime").GetDecimal());
    }

    // API reference: QueryVectors takes and returns nextToken
    [Fact]
    public async Task QueryVectorsPagesWithNextToken()
    {
        var handler = new FakeHandler(
            FakeS3Vectors.Json("""{ "distanceMetric": "cosine", "vectors": [{ "key": "a" }], "nextToken": "page-2" }"""),
            FakeS3Vectors.Json("""{ "distanceMetric": "cosine", "vectors": [{ "key": "b" }] }"""));

        var client = FakeS3Vectors.Client(handler);

        var first = await client.QueryVectorsAsync(new() { QueryVector = new float[] { 1f }, TopK = 2 });
        var second = await client.QueryVectorsAsync(new() { QueryVector = new float[] { 1f }, TopK = 2, NextToken = first.NextToken });

        Assert.Equal("page-2", first.NextToken);
        Assert.False(handler.Requests[0].Json.TryGetProperty("nextToken", out _));
        Assert.Equal("page-2", handler.Requests[1].Json.GetProperty("nextToken").GetString());
        Assert.Equal("b", second.Vectors[0].Key);
        Assert.Null(second.NextToken);
    }

    [Fact]
    public async Task IndexModesAreOptionalInResponses()
    {
        var result = await Client("""
            { "index": { "vectorBucketName": "media", "indexName": "frames", "indexArn": "arn", "creationTime": 1752624000, "dataType": "float32", "dimension": 3, "distanceMetric": "cosine" } }
            """).GetIndexAsync(new() { IndexArn = "arn" });

        Assert.Null(result.Index.IndexMode);
    }

    // AWS always sends required members, so a response without one is rejected
    [Fact]
    public async Task MissingRequiredResponseMemberThrows()
    {
        await Assert.ThrowsAsync<JsonException>(() =>
            Client("""{ "vectors": [{ "data": { "float32": [1] } }] }""").GetVectorsAsync(new() { Keys = ["a"] }));
    }

    // API reference: "an HTTP 200 response with an empty HTTP body". Only Content-Length: 0 is recognised as empty
    [Fact]
    public async Task EmptyBodyWithoutContentLengthReturnsAnEmptyResult()
    {
        var handler = new FakeHandler(() => new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new StreamContent(new NonSeekableEmptyStream())
        });

        Assert.NotNull(await FakeS3Vectors.Client(handler).DeleteVectorsAsync(new() { Keys = ["a"] }));
    }

    private sealed class NonSeekableEmptyStream : MemoryStream
    {
        public override bool CanSeek => false;
    }
}
