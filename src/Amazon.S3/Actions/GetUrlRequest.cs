using System;

namespace Amazon.S3
{
    public class GetUrlRequest
    {
        public GetUrlRequest(string bucketName, string key)
        {
            #region Preconditions

            if (bucketName == null)
                throw new ArgumentNullException(nameof(bucketName));

            if (bucketName.Length == 0)
                throw new ArgumentException("may not be empty", nameof(bucketName));

            #endregion

            BucketName = bucketName;
            Key = key;
            ExpiresIn = TimeSpan.FromMinutes(60);
        }

        public string BucketName { get; }

        public string Key { get; }

        public TimeSpan ExpiresIn { get; set; }
    }
}
