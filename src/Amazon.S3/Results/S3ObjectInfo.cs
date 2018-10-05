using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Storage;

namespace Amazon.S3
{
    public sealed class S3ObjectInfo : IBlob
    {
        public S3ObjectInfo(string key, long size)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ContentLength = size;
        }

        public S3ObjectInfo(string key, long size, DateTime modified)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ContentLength = size;
            Modified = modified;
        }

        public S3ObjectInfo(string bucketName, string name, HttpResponseMessage response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            BucketName = bucketName;
            Key = name;

            ContentLength = response.Content.Headers.ContentLength.Value;             // Content-Length
            Modified = response.Content.Headers.LastModified.Value.UtcDateTime;  // Last-Modified

            if (response.Headers.ETag != null)
            {
                ETag = new ETag(response.Headers.ETag.Tag);
            }

            var headers = new Dictionary<string, string>();

            foreach (var header in response.Headers)
            {
                headers.Add(header.Key, string.Join(";", header.Value));
            }

            foreach (var header in response.Content.Headers)
            {
                headers.Add(header.Key, string.Join(";", header.Value));
            }

            Properties = headers;
        }

        public string BucketName { get; }

        public string Key { get; }

        public ETag ETag { get; }

        public long ContentLength { get; }

        public DateTime Modified { get; }

        public IReadOnlyDictionary<string, string> Properties { get; }

        #region IBlob

        long IBlob.Size => ContentLength;

        ValueTask<Stream> IBlob.OpenAsync()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose() { }

        #endregion

        #region Helpers

        public string ContentType => Properties["Content-Type"];

        public string VersionId => Properties.TryGetValue("x-amz-version-id", out var version) ? version : null;

        public string StorageClass => Properties["x-amz-storage-class"];

        #endregion
    }
}