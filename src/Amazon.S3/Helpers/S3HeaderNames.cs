namespace Amazon.S3
{
    internal static class S3HeaderNames
    {
        public static readonly string StorageClass                = "x-amz-storage-class";
        public static readonly string VersionId                   = "x-amz-version-id"; 
        public static readonly string ContentSha256               = "x-amz-content-sha256";
        public static readonly string MetadataDirective           = "x-amz-metadata-directive";
        public static readonly string Tagging                     = "x-amz-tagging";
        public static readonly string DeleteMarker                = "x-amz-delete-marker";

        public const string CopySourceIfMatch                     = "x-amz-copy-source-if-match";
        public const string CopySourceIfNoneMatch                 = "x-amz-copy-source-if-none-match";
        public const string CopySourceIfUnmodifiedSince           = "x-amz-copy-source-if-unmodified-since";
        public const string CopySourceIfModifiedSince             = "x-amz-copy-source-if-modified-since";

        public const string ServerSideEncryptionCustomerAlgorithm = "x-amz-server-side-encryption-customer-algorithm";
        public const string ServerSideEncryptionCustomerKey       = "x-amz-server-side-encryption-customer-key";
        public const string ServerSideEncryptionCustomerKeyMD5    = "x-amz-server-side-encryption-customer-key-MD5";
    }   
}

// x-amz-replication-status
// x-amz-restore
// x-amz-object-lock-mode
// x-amz-object-lock-retain-until-date
// x-amz-object-lock-legal-hold