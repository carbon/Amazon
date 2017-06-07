using System.Net.Http;

namespace Amazon.S3
{
    public class PutObjectResult
    {
        public PutObjectResult(HttpResponseMessage response)
        {
            ETag = response.Headers.ETag.Tag;

            if (response.Headers.TryGetValues("x-amz-version-id", out var versionId))
            {
                VersionId = versionId.ToString();
            }
        }

        public string ETag { get; }

        public string VersionId { get; }
    }
}

// NOTES: 
// Amazon's ETag is the a hexidecimal encoded MD5 digest of the blobs bytes wrapped in quotes
// For all PUT requests, Amazon S3 computes its own MD5, stores it with the object, and then returns the computed MD5 as part of the PUT response code in the ETag.  
