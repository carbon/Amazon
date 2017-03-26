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
    public class S3Object : IBlobResult, IDisposable
    {
        private Stream stream;
        private readonly Dictionary<string, string> headers = new Dictionary<string, string>();
        private readonly HttpResponseMessage response;

        public S3Object(string name, HttpResponseMessage response)
        {
            #region Preconditions

            if (response == null)
                throw new ArgumentNullException(nameof(response));

            #endregion

            Key = name ?? throw new ArgumentNullException(nameof(name));

            foreach (var header in response.Headers)
            {
                headers.Add(header.Key, string.Join(";", header.Value));
            }

            foreach (var header in response.Content.Headers)
            {
                headers.Add(header.Key, string.Join(";", header.Value));
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
            get { return headers["Content-Type"]; }
            set { headers["Content-Type"] = value; }
        }

        public long ContentLength
        {
            get { return long.Parse(headers["Content-Length"]); }
            set { headers["Content-Length"] = value.ToString(); }
        }

        public DateTimeOffset? LastModified
        {
           get => DateTime.ParseExact(headers["Last-Modified"], "r", null).ToUniversalTime();
        }

        public CacheControlHeaderValue CacheControl
        {
            get => CacheControlHeaderValue.Parse(headers["Cache-Control"]);
            set => headers["Cache-Control"] = value.ToString(); 
        }

        #endregion

        public async ValueTask<Stream> OpenAsync()
        {
            if (stream == null)
            {
                stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }

            return stream;
        }

        public Dictionary<string, string> Headers => headers;

        #region IBlob

        string IBlob.Name => Key;

        long IBlob.Size => ContentLength;

        DateTime IBlob.Modified => (LastModified ?? DateTimeOffset.MinValue).UtcDateTime;

        IReadOnlyDictionary<string, string> IBlob.Metadata => headers;

        #endregion

        public void Dispose()
        {
            stream?.Dispose();
            response?.Dispose();
        }
    }
}