using System;

namespace Amazon.S3
{
    public struct S3ObjectLocation
    {
        public S3ObjectLocation(string bucketName, string key)
        {
            #region Preconditions

            if (bucketName == null)
                throw new ArgumentNullException(nameof(bucketName));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            BucketName = bucketName;
            Key = key;
        }

        public string BucketName { get; }

        public string Key { get; }
    }
}