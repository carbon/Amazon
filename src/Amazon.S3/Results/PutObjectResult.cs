namespace Amazon.S3;

public sealed class PutObjectResult
{
    public required string ETag { get; init; }

    public string? VersionId { get; init; }
}

// NOTES: 
// Amazon's ETag is the a hexidecimal encoded MD5 digest of the blobs bytes wrapped in quotes
// For all PUT requests, Amazon S3 computes its own MD5, stores it with the object, 
// and then returns the computed MD5 as part of the PUT response code in the ETag.  
