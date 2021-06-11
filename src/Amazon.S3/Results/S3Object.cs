using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Storage;

namespace Amazon.S3
{
    public sealed class S3Object : IBlobResult, IDisposable
    {
        private Stream? stream;
        private readonly Dictionary<string, string> properties = new();
        private HttpResponseMessage? response;

        public S3Object(string name, HttpResponseMessage response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            Key = name ?? throw new ArgumentNullException(nameof(name));

            foreach (var header in response.Headers)
            {
                properties.Add(header.Key, string.Join(';', header.Value));
            }

            foreach (var header in response.Content.Headers)
            {
                properties.Add(header.Key, string.Join(';', header.Value));
            }

            StatusCode = response.StatusCode;

            if (response.StatusCode is HttpStatusCode.NotModified)
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
            get => long.Parse(properties["Content-Length"], CultureInfo.InvariantCulture);
            set => properties["Content-Length"] = value.ToString();
        }

        public DateTimeOffset? LastModified
        {
           get => DateTime.ParseExact(properties["Last-Modified"], "r", CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public CacheControlHeaderValue? CacheControl
        {
            get
            {
                return properties.TryGetValue("Cache-Control", out var cacheControl) 
                    ? CacheControlHeaderValue.Parse(cacheControl) 
                    : null;
            }
            set
            {
                if (value is not null)
                {
                    properties["Cache-Control"] = value.ToString();
                }
                else
                {
                    properties.Remove("Cache-Control");
                }
            }
        }

        public ContentRangeHeaderValue? ContentRange
        {
            get => properties.TryGetValue("Content-Range", out var contentRange) ? ContentRangeHeaderValue.Parse(contentRange) : null;
        }

        #endregion

        public async ValueTask<Stream> OpenAsync()
        {
            if (response is null) throw new ObjectDisposedException(nameof(S3Object));

            return stream ??= await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public async Task CopyToAsync(Stream output)
        {
            if (response is null) throw new ObjectDisposedException(nameof(S3Object));

            await response.Content.CopyToAsync(output).ConfigureAwait(false);
        }

        public async Task CopyToAsync(Stream output, CancellationToken cancellationToken)
        {
            if (response is null) throw new ObjectDisposedException(nameof(S3Object));

            await response.Content.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
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

            response = null;
        }
    }
}