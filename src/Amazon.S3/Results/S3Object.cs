using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Storage;

namespace Amazon.S3;

public sealed class S3Object : IBlobResult, IDisposable
{
    private Stream? _stream;
    private readonly Dictionary<string, string> _properties;
    private HttpResponseMessage? _response;
    private long? contentLength;

    public S3Object(string name, HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(response);

        Key = name;

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
        get => contentLength ??= long.Parse(_properties["Content-Length"], NumberStyles.None, CultureInfo.InvariantCulture);
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
        if (_response is null) throw new ObjectDisposedException(nameof(S3Object));

        return _stream ??= await _response.Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    public async Task CopyToAsync(Stream output)
    {
        if (_response is null) throw new ObjectDisposedException(nameof(S3Object));

        await _response.Content.CopyToAsync(output).ConfigureAwait(false);
    }

    public async Task CopyToAsync(Stream output, CancellationToken cancellationToken)
    {
        if (_response is null) throw new ObjectDisposedException(nameof(S3Object));

        await _response.Content.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
    }

    #region IBlob

    long IBlob.Size => ContentLength;

    DateTime IBlob.Modified => LastModified.UtcDateTime;

    public IReadOnlyDictionary<string, string> Properties => _properties;

    #endregion

    public void Dispose()
    {
        _stream?.Dispose();
        _response?.Dispose();

        _response = null;
    }
}
