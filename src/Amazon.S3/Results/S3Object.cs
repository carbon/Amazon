using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;

using Carbon.Storage;

namespace Amazon.S3
{
    public sealed class S3Object : IBlobResult, IDisposable
    {
        private Stream? stream;
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private readonly HttpResponseMessage? response;

        public S3Object(string name, HttpResponseMessage response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            Key = name ?? throw new ArgumentNullException(nameof(name));

            foreach (var header in response.Headers)
            {
                properties.Add(header.Key, string.Join(";", header.Value));
            }

            foreach (var header in response.Content.Headers)
            {
                properties.Add(header.Key, string.Join(";", header.Value));
            }

            StatusCode = response.StatusCode;

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                response.Dispose();

                return;
            }

            this.response = response;
        }

        #region Header Aliases

        public string Key { get; }

        public HttpStatusCode StatusCode { get; }

        public string ContentType
        {
            get => properties["Content-Type"];
            set => properties["Content-Type"] = value;
        }

        public long ContentLength
        {
            get => long.Parse(properties["Content-Length"]);
            set => properties["Content-Length"] = value.ToString();
        }

        public DateTimeOffset? LastModified
        {
           get => DateTime.ParseExact(properties["Last-Modified"], "r", null).ToUniversalTime();
        }

        public CacheControlHeaderValue CacheControl
        {
            get => CacheControlHeaderValue.Parse(properties["Cache-Control"]);
            set => properties["Cache-Control"] = value.ToString(); 
        }

        // x-amz-replication-status
        // x-amz-restore
        // x-amz-object-lock-mode
        // x-amz-object-lock-retain-until-date
        // x-amz-object-lock-legal-hold	

        #endregion

        public async ValueTask<Stream> OpenAsync()
        {
            if (response is null)
            {
                throw new Exception("Response is null");
            }

            return stream ??= await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        #region IBlob

        long IBlob.Size => ContentLength;

        DateTime IBlob.Modified => (LastModified ?? DateTimeOffset.MinValue).UtcDateTime;

        public IReadOnlyDictionary<string, string> Properties => properties;

        #endregion

        public void Dispose()
        {
            stream?.Dispose();
            response?.Dispose();
        }
    }
}