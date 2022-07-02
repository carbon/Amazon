using System.Text.Json;

namespace Amazon.S3.Events.Tests;

public class S3EventTests
{
    private static readonly JsonSerializerOptions jso = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    [Fact]
    public void CanDeserialize()
    {
        var model = JsonSerializer.Deserialize<S3Event>(
            """
            {  
                "eventVersion":"2.0",
                "eventSource":"aws:s3",
                "awsRegion":"us-east-1",
                "eventTime": "1970-01-01T00:00:00.000Z",
                "eventName":"ObjectCreated:Put",
                "userIdentity":{  
                    "principalId":"1"
                },
                "requestParameters":{  
                    "sourceIPAddress":"ip"
                },
                "responseElements":{  
                    "x-amz-request-id":"Amazon S3 generated request ID",
                    "x-amz-id-2":"Amazon S3 host that processed the request"
                },
                "s3":{  
                    "s3SchemaVersion":"1.0",
                    "configurationId":"ID found in the bucket notification configuration",
                    "bucket":{  
                        "name":"bucket-name",
                        "ownerIdentity":{  
                            "principalId":"A3NL1KOZZKExample"
                        },
                        "arn":"arn:aws:s3:::bucket-name"
                    },
                    "object":{  
                        "key":"object-key",
                        "size": 1,
                        "eTag":"object eTag",
                        "versionId":"1",
                        "sequencer": "a string representation of a hexadecimal value used to determine event sequence, only used with PUTs and DELETEs"            
                    }
                }
            }
            """, jso);

        Assert.Equal("2.0",                      model.EventVersion);
        Assert.Equal("aws:s3",                   model.EventSource);
        Assert.Equal("us-east-1",                model.AwsRegion);
        Assert.Equal("ObjectCreated:Put",        model.EventName);
        Assert.Equal("1",                        model.UserIdentity.PrincipalId);
        Assert.Equal("bucket-name",              model.S3.Bucket.Name);
        Assert.Equal("A3NL1KOZZKExample",        model.S3.Bucket.OwnerIdentity.PrincipalId);
        Assert.Equal("arn:aws:s3:::bucket-name", model.S3.Bucket.Arn);
        Assert.Equal("object-key",               model.S3.Object.Key);
        Assert.Equal(1,                          model.S3.Object.Size);
        Assert.Equal("1",                        model.S3.Object.VersionId);
    }
}