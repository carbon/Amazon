using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

using Carbon.Storage;

namespace Amazon.S3
{
    public class S3ObjectInfo : IBlob
    {
        public S3ObjectInfo(string key, long size)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            Key = key;
            Size = size;
        }

        public S3ObjectInfo(string name, long size, DateTime modified)
        {
            Key = name;
            Size = size;
            Modified = modified;
        }

        public S3ObjectInfo(string bucketName, string name, HttpResponseMessage response)
        {
            #region Preconditions

            if (response == null) throw new ArgumentNullException(nameof(response));

            #endregion

            BucketName = bucketName;
            Key = name;

            Size        = response.Content.Headers.ContentLength.Value;             // Content-Length
            ContentType = response.Content.Headers.ContentType.MediaType;           // Content-Type
            Modified    = response.Content.Headers.LastModified.Value.UtcDateTime;  // Last-Modified

            if (response.Headers.ETag != null)
            {
                ETag = new ETag(response.Headers.ETag.Tag);
            }

            foreach (var header in response.Headers)
            {
                Headers.Add(header.Key, string.Join(";", header.Value));
            }

            foreach (var header in response.Content.Headers)
            {
                Headers.Add(header.Key, string.Join(";", header.Value));
            }
        }

        public string BucketName { get; }

        public string Key { get; }

        public string ContentType { get; }

        public ETag ETag { get; }

        public long Size { get; } // aka Content-Length

        public DateTime Modified { get; }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();   

        #region IBlob

        string IBlob.Name => Key;

        IReadOnlyDictionary<string, string> IBlob.Metadata => Headers;

        Stream IBlob.Open()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
        }

        #endregion

        #region Helpers

        public string VersionId => Headers["x-amz-version-id"];

        public string StorageClass => Headers["x-amz-storage-class"];

        #endregion
    }
}