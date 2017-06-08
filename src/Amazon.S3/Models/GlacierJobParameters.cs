namespace Amazon.S3
{
    public class GlacierJobParameters
    {
        public GlacierJobTier Tier { get; }
    }
}

/*
POST /ObjectName?restore&versionId=VersionID HTTP/1.1
Host: BucketName.s3.amazonaws.com
Date: date
Authorization: authorization string (see Authenticating Requests (AWS Signature Version 4))
Content-MD5: MD5

<RestoreRequest xmlns="http://s3.amazonaws.com/doc/2006-3-01">
   <Days>NumberOfDays</Days>
</RestoreRequest> 
*/
