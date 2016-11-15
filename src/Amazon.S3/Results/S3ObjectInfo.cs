using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

using Carbon.Storage;

namespace Amazon.S3
{
    public class S3ObjectInfo : IBlob
    {
        public S3ObjectInfo(string name, long size)
        {
            Name = name;
            Size = size;
        }

        public S3ObjectInfo(string bucketName, string name, HttpResponseMessage response)
        {
            #region Preconditions

            if (response == null) throw new ArgumentNullException(nameof(response));

            #endregion

            BucketName = bucketName;
            Name = name;

            Size        = response.Content.Headers.ContentLength.Value;             // Content-Length
            ContentType = response.Content.Headers.ContentType.MediaType;           // Content-Type
            Modified    = response.Content.Headers.LastModified.Value.UtcDateTime;  // Last-Modified
            ETag        = response.Headers.ETag.Tag;

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

        public string Name { get; }

        public string ContentType { get; }

        public string ETag { get; }

        public long Size { get; } // aka Content-Length

        public DateTime Modified { get; }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        IDictionary<string, string> Metadata => Headers;

        public Stream Open()
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }

        #region Metadata

        IDictionary<string, string> IBlob.Metadata => Headers;

        #endregion
        #region Helpers

        public string VersionId => Headers["x-amz-version-id"];

        public string StorageClass => Headers["x-amz-storage-class"];

        /*
        public byte[] MD5
        {
            get
            {
                // Generally the ETAG is the MD5 of the object -- hexidecimal encoded and wrapped in quootes.
                // If the object was uploaded using multipart upload then this is the MD5 all of the upload-part-md5s.

                // Multipart uploads are
                // 1f8ada2ce841b291cfcd6b9b4b645044-2

                if (ETag == null || ETag.Contains('-')) return default(Hash);

                try
                {
                    return new Hash(HashType.MD5, HexString.ToBytes(ETag.Trim('"')));
                }
                catch
                {
                    return default(Hash);
                }
            }
        }
        */
        #endregion
    }
}