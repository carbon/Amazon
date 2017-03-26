using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Storage;

namespace Amazon.S3
{
    public class S3ObjectInfo : IBlob
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
            #region Preconditions

            if (response == null) throw new ArgumentNullException(nameof(response));

            #endregion

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

            Metadata = headers;
        }

        public string BucketName { get; }

        public string Key { get; }

        public ETag ETag { get; }

        public long ContentLength { get; }

        public DateTime Modified { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        #region IBlob

        string IBlob.Name => Key;

        long IBlob.Size => ContentLength;

        ValueTask<Stream> IBlob.OpenAsync()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose() { }

        #endregion

        #region Helpers

        public string ContentType => Metadata["Content-Type"];

        public string VersionId => Metadata.TryGetValue("x-amz-version-id", out var version) ? version : null;

        public string StorageClass => Metadata["x-amz-storage-class"];

        #endregion
    }
}