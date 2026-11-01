using System.Text.Json;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors.Tests;

public class RequestSerializationTests
{
    private static async Task<RecordedRequest> Capture(Func<AmazonS3VectorsClient, Task> operation, string response = "{}")
    {
        var handler = new FakeHandler(FakeS3Vectors.Json(response));

        await operation(FakeS3Vectors.Client(handler));

        return handler.Request;
    }

    private static JsonElement Parse(string json) => JsonDocument.Parse(json).RootElement;

    [Fact]
    public async Task UnsetMembersAreOmitted()
    {
        Assert.Equal("{}", (await Capture(c => c.ListVectorBucketsAsync(new()))).Body);
        Assert.Equal("{}", (await Capture(c => c.ListIndexesAsync(new()))).Body);
        Assert.Equal("{}", (await Capture(c => c.ListVectorsAsync(new()))).Body);
        Assert.Equal("{}", (await Capture(c => c.DeleteVectorBucketAsync(new()))).Body);
    }

    [Fact]
    public async Task EncryptionConfigurationOmitsAnUnsetKmsKey()
    {
        var request = await Capture(c => c.CreateVectorBucketAsync(new() {
            VectorBucketName = "media",
            EncryptionConfiguration = new() { SseType = "AES256" }
        }));

        var encryption = request.Json.GetProperty("encryptionConfiguration");

        Assert.Equal("AES256", encryption.GetProperty("sseType").GetString());
        Assert.False(encryption.TryGetProperty("kmsKeyArn", out _));
    }

    [Fact]
    public async Task CreateVectorBucket()
    {
        var request = await Capture(c => c.CreateVectorBucketAsync(new() {
            VectorBucketName = "media",
            EncryptionConfiguration = new() { SseType = "aws:kms", KmsKeyArn = "key-arn" },
            Tags = new() { ["team"] = "search" }
        }));

        var json = request.Json;

        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());
        Assert.Equal("aws:kms", json.GetProperty("encryptionConfiguration").GetProperty("sseType").GetString());
        Assert.Equal("key-arn", json.GetProperty("encryptionConfiguration").GetProperty("kmsKeyArn").GetString());
        Assert.Equal("search", json.GetProperty("tags").GetProperty("team").GetString());
    }

    [Fact]
    public async Task CreateVectorBucketOmitsOptionalMembers()
    {
        var request = await Capture(c => c.CreateVectorBucketAsync(new() { VectorBucketName = "media" }));

        Assert.False(request.Json.TryGetProperty("encryptionConfiguration", out _));
        Assert.False(request.Json.TryGetProperty("tags", out _));
    }

    [Theory]
    [InlineData(DistanceMetric.Cosine, "cosine")]
    [InlineData(DistanceMetric.Euclidean, "euclidean")]
    public async Task CreateIndex(DistanceMetric metric, string expectedMetric)
    {
        var request = await Capture(c => c.CreateIndexAsync(new() {
            VectorBucketName = "media",
            VectorBucketArn = FakeS3Vectors.BucketArn,
            IndexName = "frames",
            DataType = VectorDataType.Float32,
            Dimension = 1024,
            DistanceMetric = metric,
            MetadataConfiguration = new() { NonFilterableMetadataKeys = ["caption"] },
            EncryptionConfiguration = new() { SseType = "AES256" },
            Tags = new() { ["team"] = "search" }
        }));

        var json = request.Json;

        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());
        Assert.Equal(FakeS3Vectors.BucketArn, json.GetProperty("vectorBucketArn").GetString());
        Assert.Equal("frames", json.GetProperty("indexName").GetString());
        Assert.Equal("float32", json.GetProperty("dataType").GetString());
        Assert.Equal(1024, json.GetProperty("dimension").GetInt32());
        Assert.Equal(expectedMetric, json.GetProperty("distanceMetric").GetString());
        Assert.Equal("caption", json.GetProperty("metadataConfiguration").GetProperty("nonFilterableMetadataKeys")[0].GetString());
        Assert.Equal("AES256", json.GetProperty("encryptionConfiguration").GetProperty("sseType").GetString());
        Assert.Equal("search", json.GetProperty("tags").GetProperty("team").GetString());
    }

    [Fact]
    public async Task CreateIndexOmitsOptionalMembers()
    {
        var request = await Capture(c => c.CreateIndexAsync(new() {
            VectorBucketName = "media",
            IndexName = "frames",
            DataType = VectorDataType.Float32,
            Dimension = 3,
            DistanceMetric = DistanceMetric.Cosine
        }));

        Assert.False(request.Json.TryGetProperty("metadataConfiguration", out _));
        Assert.False(request.Json.TryGetProperty("encryptionConfiguration", out _));
        Assert.False(request.Json.TryGetProperty("tags", out _));
    }

    [Fact]
    public async Task IndexOperationsAddressByNameOrArn()
    {
        var byName = await Capture(c => c.GetIndexAsync(new() { VectorBucketName = "media", IndexName = "frames" }), """{ "index": null }""");

        Assert.Equal("media", byName.Json.GetProperty("vectorBucketName").GetString());
        Assert.Equal("frames", byName.Json.GetProperty("indexName").GetString());
        Assert.True(byName.IsAbsent("indexArn"));

        var byArn = await Capture(c => c.DeleteIndexAsync(new() { IndexArn = "index-arn" }));

        Assert.Equal("index-arn", byArn.Json.GetProperty("indexArn").GetString());
        Assert.True(byArn.IsAbsent("indexName"));
        Assert.True(byArn.IsAbsent("vectorBucketName"));

        var deleteByName = await Capture(c => c.DeleteIndexAsync(new() { VectorBucketName = "media", IndexName = "frames" }));

        Assert.Equal("frames", deleteByName.Json.GetProperty("indexName").GetString());
    }

    [Fact]
    public async Task BucketOperationsAddressByNameOrArn()
    {
        Func<AmazonS3VectorsClient, string?, string?, Task>[] operations = [
            (c, name, arn) => c.DeleteVectorBucketAsync(new() { VectorBucketName = name, VectorBucketArn = arn }),
            (c, name, arn) => c.DeleteVectorBucketPolicyAsync(new() { VectorBucketName = name, VectorBucketArn = arn }),
            (c, name, arn) => c.GetVectorBucketPolicyAsync(new() { VectorBucketName = name, VectorBucketArn = arn }),
            (c, name, arn) => c.PutVectorBucketPolicyAsync(new() { VectorBucketName = name, VectorBucketArn = arn, Policy = "{}" }),
        ];

        foreach (var operation in operations)
        {
            var byName = await Capture(c => operation(c, "media", null));

            Assert.Equal("media", byName.Json.GetProperty("vectorBucketName").GetString());
            Assert.True(byName.IsAbsent("vectorBucketArn"));

            var byArn = await Capture(c => operation(c, null, FakeS3Vectors.BucketArn));

            Assert.Equal(FakeS3Vectors.BucketArn, byArn.Json.GetProperty("vectorBucketArn").GetString());
            Assert.True(byArn.IsAbsent("vectorBucketName"));
        }
    }

    [Fact]
    public async Task GetVectorBucketAddressesByName()
    {
        var handler = new FakeHandler(FakeS3Vectors.Json("""{ "vectorBucket": null }"""));

        await FakeS3Vectors.Client(handler).GetVectorBucketAsync(new() { VectorBucketArn = FakeS3Vectors.BucketArn, VectorBucketName = "media" });

        Assert.Equal(FakeS3Vectors.BucketArn, handler.Request.Json.GetProperty("vectorBucketArn").GetString());
        Assert.Equal("media", handler.Request.Json.GetProperty("vectorBucketName").GetString());
    }

    // The policy is a JSON document carried as a string, and must not be inlined
    [Fact]
    public async Task PutVectorBucketPolicySendsThePolicyAsAString()
    {
        const string policy = """{"Version":"2012-10-17","Statement":[]}""";

        var request = await Capture(c => c.PutVectorBucketPolicyAsync(new() { VectorBucketName = "media", Policy = policy }));

        Assert.Equal(JsonValueKind.String, request.Json.GetProperty("policy").ValueKind);
        Assert.Equal(policy, request.Json.GetProperty("policy").GetString());
    }

    [Fact]
    public async Task PutVectors()
    {
        var request = await Capture(c => c.PutVectorsAsync(new() {
            IndexArn = "index-arn",
            IndexName = "frames",
            VectorBucketName = "media",
            Vectors = [
                new() { Key = "a", Data = new float[] { 0.5f, -1.25f }, Metadata = Parse("""{ "genre": "scifi", "year": 2020 }""") },
                new() { Key = "b", Data = new VectorData { Float32 = [2f] } }
            ]
        }));

        var json = request.Json;
        var vectors = json.GetProperty("vectors");

        Assert.Equal("index-arn", json.GetProperty("indexArn").GetString());
        Assert.Equal("frames", json.GetProperty("indexName").GetString());
        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());

        Assert.Equal("a", vectors[0].GetProperty("key").GetString());
        Assert.Equal([0.5f, -1.25f], vectors[0].GetProperty("data").GetProperty("float32").EnumerateArray().Select(v => v.GetSingle()));
        Assert.Equal("scifi", vectors[0].GetProperty("metadata").GetProperty("genre").GetString());
        Assert.Equal(2020, vectors[0].GetProperty("metadata").GetProperty("year").GetInt32());

        Assert.Equal(2f, vectors[1].GetProperty("data").GetProperty("float32")[0].GetSingle());
        Assert.False(vectors[1].TryGetProperty("metadata", out _));
    }

    [Fact]
    public async Task GetVectors()
    {
        var request = await Capture(c => c.GetVectorsAsync(new() {
            IndexArn = "index-arn",
            IndexName = "frames",
            VectorBucketName = "media",
            Keys = ["a", "b"],
            ReturnData = true,
            ReturnMetadata = true
        }));

        var json = request.Json;

        Assert.Equal("index-arn", json.GetProperty("indexArn").GetString());
        Assert.Equal("frames", json.GetProperty("indexName").GetString());
        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());
        Assert.Equal(["a", "b"], json.GetProperty("keys").EnumerateArray().Select(k => k.GetString()));
        Assert.True(json.GetProperty("returnData").GetBoolean());
        Assert.True(json.GetProperty("returnMetadata").GetBoolean());
    }

    [Fact]
    public async Task GetVectorsLeavesFalseFlagsToTheServiceDefault()
    {
        var request = await Capture(c => c.GetVectorsAsync(new() { Keys = ["a"] }));

        Assert.False(request.Json.TryGetProperty("returnData", out _));
        Assert.False(request.Json.TryGetProperty("returnMetadata", out _));
    }

    [Fact]
    public async Task DeleteVectors()
    {
        var request = await Capture(c => c.DeleteVectorsAsync(new() {
            IndexArn = "index-arn",
            IndexName = "frames",
            VectorBucketName = "media",
            Keys = ["a", "b"]
        }));

        var json = request.Json;

        Assert.Equal("index-arn", json.GetProperty("indexArn").GetString());
        Assert.Equal("frames", json.GetProperty("indexName").GetString());
        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());
        Assert.Equal(["a", "b"], json.GetProperty("keys").EnumerateArray().Select(k => k.GetString()));
    }

    [Fact]
    public async Task QueryVectors()
    {
        var request = await Capture(c => c.QueryVectorsAsync(new() {
            IndexArn = "index-arn",
            IndexName = "frames",
            VectorBucketName = "media",
            QueryVector = new float[] { 0.25f, 0.75f },
            TopK = 5,
            Filter = Parse("""{ "genre": { "$eq": "scifi" } }"""),
            ReturnDistance = true,
            ReturnMetadata = true
        }), """{ "distanceMetric": "cosine", "vectors": [] }""");

        var json = request.Json;

        Assert.Equal("index-arn", json.GetProperty("indexArn").GetString());
        Assert.Equal("frames", json.GetProperty("indexName").GetString());
        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());
        Assert.Equal([0.25f, 0.75f], json.GetProperty("queryVector").GetProperty("float32").EnumerateArray().Select(v => v.GetSingle()));
        Assert.Equal(5, json.GetProperty("topK").GetInt32());
        Assert.Equal("scifi", json.GetProperty("filter").GetProperty("genre").GetProperty("$eq").GetString());
        Assert.True(json.GetProperty("returnDistance").GetBoolean());
        Assert.True(json.GetProperty("returnMetadata").GetBoolean());
    }

    [Fact]
    public async Task QueryVectorsOmitsOptionalMembers()
    {
        var request = await Capture(c => c.QueryVectorsAsync(new() {
            QueryVector = new float[] { 1f },
            TopK = 1
        }), """{ "distanceMetric": "cosine", "vectors": [] }""");

        Assert.False(request.Json.TryGetProperty("filter", out _));
        Assert.False(request.Json.TryGetProperty("returnDistance", out _));
        Assert.False(request.Json.TryGetProperty("returnMetadata", out _));
    }

    [Theory]
    [InlineData(IndexMode.Classic, "CLASSIC")]
    [InlineData(IndexMode.Enhanced, "ENHANCED")]
    public async Task QueryVectorsSendsTheQueryMode(IndexMode mode, string expected)
    {
        var request = await Capture(c => c.QueryVectorsAsync(new() {
            QueryVector = new float[] { 1f },
            TopK = 1,
            QueryMode = mode
        }), """{ "distanceMetric": "cosine", "vectors": [] }""");

        Assert.Equal(expected, request.Json.GetProperty("queryMode").GetString());
    }

    [Fact]
    public async Task QueryVectorsOmitsAnUnsetQueryMode()
    {
        var request = await Capture(c => c.QueryVectorsAsync(new() {
            QueryVector = new float[] { 1f },
            TopK = 1
        }), """{ "distanceMetric": "cosine", "vectors": [] }""");

        Assert.True(request.IsAbsent("queryMode"));
    }

    [Fact]
    public async Task PutVectorBucketDefaultIndexMode()
    {
        var request = await Capture(c => c.PutVectorBucketDefaultIndexModeAsync(new() {
            VectorBucketName = "media",
            VectorBucketArn = FakeS3Vectors.BucketArn,
            DefaultIndexMode = IndexMode.Enhanced
        }));

        Assert.Equal("media", request.Json.GetProperty("vectorBucketName").GetString());
        Assert.Equal(FakeS3Vectors.BucketArn, request.Json.GetProperty("vectorBucketArn").GetString());
        Assert.Equal("ENHANCED", request.Json.GetProperty("defaultIndexMode").GetString());
    }

    [Fact]
    public async Task UpdateIndexMode()
    {
        var request = await Capture(c => c.UpdateIndexModeAsync(new() {
            IndexArn = "index-arn",
            IndexName = "frames",
            VectorBucketName = "media",
            IndexMode = IndexMode.Classic
        }));

        Assert.Equal("index-arn", request.Json.GetProperty("indexArn").GetString());
        Assert.Equal("frames", request.Json.GetProperty("indexName").GetString());
        Assert.Equal("media", request.Json.GetProperty("vectorBucketName").GetString());
        Assert.Equal("CLASSIC", request.Json.GetProperty("indexMode").GetString());
    }

    [Fact]
    public async Task ListVectorBuckets()
    {
        var request = await Capture(c => c.ListVectorBucketsAsync(new() { Prefix = "me", MaxResults = 10, NextToken = "t" }));

        Assert.Equal("me", request.Json.GetProperty("prefix").GetString());
        Assert.Equal(10, request.Json.GetProperty("maxResults").GetInt32());
        Assert.Equal("t", request.Json.GetProperty("nextToken").GetString());
    }

    [Fact]
    public async Task ListIndexes()
    {
        var request = await Capture(c => c.ListIndexesAsync(new() {
            VectorBucketName = "media",
            VectorBucketArn = FakeS3Vectors.BucketArn,
            Prefix = "fr",
            MaxResults = 10,
            NextToken = "t"
        }));

        Assert.Equal("media", request.Json.GetProperty("vectorBucketName").GetString());
        Assert.Equal(FakeS3Vectors.BucketArn, request.Json.GetProperty("vectorBucketArn").GetString());
        Assert.Equal("fr", request.Json.GetProperty("prefix").GetString());
        Assert.Equal(10, request.Json.GetProperty("maxResults").GetInt32());
        Assert.Equal("t", request.Json.GetProperty("nextToken").GetString());
    }

    [Fact]
    public async Task ListVectors()
    {
        var request = await Capture(c => c.ListVectorsAsync(new() {
            IndexArn = "index-arn",
            IndexName = "frames",
            VectorBucketName = "media",
            MaxResults = 100,
            NextToken = "t",
            SegmentCount = 4,
            SegmentIndex = 0,
            ReturnData = true,
            ReturnMetadata = false
        }));

        var json = request.Json;

        Assert.Equal("index-arn", json.GetProperty("indexArn").GetString());
        Assert.Equal("frames", json.GetProperty("indexName").GetString());
        Assert.Equal("media", json.GetProperty("vectorBucketName").GetString());
        Assert.Equal(100, json.GetProperty("maxResults").GetInt32());
        Assert.Equal("t", json.GetProperty("nextToken").GetString());
        Assert.Equal(4, json.GetProperty("segmentCount").GetInt32());
        Assert.Equal(0, json.GetProperty("segmentIndex").GetInt32()); // segment ids are zero-based, so 0 must be sent
        Assert.True(json.GetProperty("returnData").GetBoolean());
        Assert.False(json.GetProperty("returnMetadata").GetBoolean());
    }
}
