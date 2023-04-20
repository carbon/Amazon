namespace Amazon.S3;

internal static class S3HeaderNames
{
    public static readonly string StorageClass                          = "x-amz-storage-class";
    public static readonly string ContentSha256                         = "x-amz-content-sha256";
    public static readonly string MetadataDirective                     = "x-amz-metadata-directive";
    public static readonly string Tagging                               = "x-amz-tagging";
    public static readonly string DeleteMarker                          = "x-amz-delete-marker";
    public static readonly string RequestCharged                        = "x-amz-request-charged";                                                                        
    public static readonly string CopySource                            = "x-amz-copy-source";
    public static readonly string CopySourceIfMatch                     = "x-amz-copy-source-if-match";
    public static readonly string CopySourceIfNoneMatch                 = "x-amz-copy-source-if-none-match";
    public static readonly string CopySourceIfUnmodifiedSince           = "x-amz-copy-source-if-unmodified-since";
    public static readonly string CopySourceIfModifiedSince             = "x-amz-copy-source-if-modified-since";                                                                        
    public static readonly string ObjectLockMode                        = "x-amz-object-lock-mode";
    public static readonly string ObjectLockRetainUntilDate             = "x-amz-object-lock-retain-until-date";
    public static readonly string ObjectLockLegalHold                   = "x-amz-object-lock-legal-hold";                                                                        
    public static readonly string ReplicationStatus                     = "x-amz-replication-status";
    public static readonly string ServerSideEncryptionCustomerAlgorithm = "x-amz-server-side-encryption-customer-algorithm";
    public static readonly string ServerSideEncryptionCustomerKey       = "x-amz-server-side-encryption-customer-key";
    public static readonly string ServerSideEncryptionCustomerKeyMD5    = "x-amz-server-side-encryption-customer-key-MD5";
    public static readonly string VersionId                             = "x-amz-version-id";
}

// x-amz-restore