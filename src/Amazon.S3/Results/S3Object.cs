using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

using Carbon.Storage;

namespace Amazon.S3;

public sealed class S3Object : IBlobResult, IDisposable
{
    private Stream? _stream;
    private readonly Dictionary<string, string> _properties;
    private HttpResponseMessage? _response;
    private long? _contentLength;

    public S3Object(string key, HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(response);

        Key = key;

        _properties = response.GetProperties();

        StatusCode = response.StatusCode;

        if (response.StatusCode is HttpStatusCode.NotModified)
        {
            response.Dispose();

            return;
        }

        _response = response;
    }

    #region Header Aliases

    public string Key { get; }

    public HttpStatusCode StatusCode { get; }

    public string ContentType => _properties["Content-Type"];

    public long ContentLength
    {
        get => _contentLength ??= long.Parse(_properties["Content-Length"], NumberStyles.None, CultureInfo.InvariantCulture);
    }

    public DateTimeOffset LastModified
    {
        get => DateTimeOffset.ParseExact(_properties["Last-Modified"], "r", CultureInfo.InvariantCulture);
    }

    public CacheControlHeaderValue? CacheControl
    {
        get => _properties.TryGetValue("Cache-Control", out var cacheControl)
            ? CacheControlHeaderValue.Parse(cacheControl)
            : null;
    }

    public ContentRangeHeaderValue? ContentRange
    {
        get => _properties.TryGetValue("Content-Range", out var contentRange)
            ? ContentRangeHeaderValue.Parse(contentRange)
            : null;
    }

    #endregion

    public async ValueTask<Stream> OpenAsync()
    {
        ObjectDisposedException.ThrowIf(_response is null, this);

        return _stream ??= await _response.Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    public Task<byte[]> ReadAsByteArrayAsync()
    {
        ObjectDisposedException.ThrowIf(_response is null, this);

        return _response.Content.ReadAsByteArrayAsync();
    }

    public async Task CopyToAsync(Stream output)
    {
        ObjectDisposedException.ThrowIf(_response is null, this);

        await _response.Content.CopyToAsync(output).ConfigureAwait(false);
    }

    public async Task CopyToAsync(Stream output, CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_response is null, this);

        await _response.Content.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
    }

#region IBlob

    long IBlob.Size => ContentLength;

    DateTime IBlob.Modified => LastModified.UtcDateTime;

    public IReadOnlyDictionary<string, string> Properties => _properties;

#endregion

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        InternalDispose();
    }

    ~S3Object()
    {
        InternalDispose();
    }

    private void InternalDispose()
    {
        _stream?.Dispose();
        _response?.Dispose();

        _stream = null;
        _response = null;
    }
}
