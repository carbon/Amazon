using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Security;

namespace Amazon;

public abstract class AwsClient
{
    private readonly AwsService _service;
    protected readonly IAwsCredential _credential;
    protected readonly HttpClient _httpClient;

    public AwsClient(AwsService service, AwsRegion region, IAwsCredential credential)
    {
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(region);
        ArgumentNullException.ThrowIfNull(credential);

        _service = service;
        Region = region;
        _credential = credential;

        Endpoint = $"https://{service.Name}.{region.Name}.amazonaws.com/";

        _httpClient = new HttpClient(new SocketsHttpHandler {
            AutomaticDecompression = DecompressionMethods.All,                
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1) // Default is 2 minutes
        })
        {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/3.0" }
            }
        };
    }

    public AwsClient(AwsService service, AwsRegion region, IAwsCredential credential, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(region);
        ArgumentNullException.ThrowIfNull(credential);
        ArgumentNullException.ThrowIfNull(httpClient);

        _service = service;
        Region = region;
        _credential = credential;
        _httpClient = httpClient;

        Endpoint = $"https://{service.Name}.{region.Name}.amazonaws.com/";
    }

    public string Endpoint { get; }

    public AwsRegion Region { get; }

    protected async Task<string> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        await SignAsync(request).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    protected async ValueTask SignAsync(HttpRequestMessage request)
    {
        if (_credential.ShouldRenew)
        {
            await _credential.RenewAsync().ConfigureAwait(false);   
        }

        DateTimeOffset date = DateTimeOffset.UtcNow;

        request.Headers.Host = request.RequestUri!.Host;
        request.Headers.Date = date;

        if (_credential.SecurityToken is not null)
        {
            request.Headers.Add("x-amz-security-token", _credential.SecurityToken);
        }

        request.Headers.Add("x-amz-date", date.UtcDateTime.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture));

        SignerV4.Sign(_credential, scope: GetCredentialScope(request), request: request);
    }

    protected virtual async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new Exception(responseText);
    }

    private CredentialScope GetCredentialScope(HttpRequestMessage httpRequest)
    {
        if (httpRequest.Headers.Date is null)
        {
            throw new Exception("Headers.Date must be set");
        }

        return new CredentialScope(httpRequest.Headers.Date.Value.UtcDateTime, Region, _service);
    }
}