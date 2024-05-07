using System.Net.Http;
using System.Net.Mime;

using Amazon.Route53.Exceptions;

namespace Amazon.Route53;

public sealed class Route53Client(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Route53, region, credential)
{
    public const string Namespace = "https://route53.amazonaws.com/doc/2013-04-01/";
    public const string Version = "2013-04-01";

    public Task<ChangeResourceRecordSetsResponse> ChangeResourceRecordSetsAsync(
        string hostedZoneId,
        ChangeResourceRecordSetsRequest request)
    {
        ArgumentException.ThrowIfNullOrEmpty(hostedZoneId);
        ArgumentNullException.ThrowIfNull(request);

        return PostXmlAsync<ChangeResourceRecordSetsRequest, ChangeResourceRecordSetsResponse>(
            path : $"/hostedzone/{hostedZoneId}/rrset",
            data : request
        );
    }

    public Task<ListResourceRecordSetsResponse> ListResourceRecordSetsAsync(ListResourceRecordSetsRequest request)
    {
        return GetAsync<ListResourceRecordSetsResponse>($"/hostedzone/{request.Id}/rrset" + request.ToQueryString());
    }

    #region Geolocations

    public Task<ListGeoLocationsResponse> ListGeoLocationsAsync(ListGeoLocationsRequest request)
    {
        return GetAsync<ListGeoLocationsResponse>("/geolocations" + request.ToQueryString());
    }

    #endregion

    private const string baseUrl = "https://route53.amazonaws.com/";

    private async Task<TResult> GetAsync<TResult>(string path)
        where TResult: notnull
    {
        string url = $"{baseUrl}{Version}{path}";

        byte[] responseBytes = await SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).ConfigureAwait(false);

        return Route53Serializer<TResult>.DeserializeXml(responseBytes);
    }

    private async Task<TResult> PostXmlAsync<T, TResult>(string path, T data)
        where T: notnull
        where TResult: notnull
    {
        string url = $"{baseUrl}{Version}{path}";

        var request = new HttpRequestMessage(HttpMethod.Post, url) {
            Content = new ByteArrayContent(Route53Serializer<T>.SerializeToUtf8Bytes(data)) {
                Headers = {
                    { "Content-Type", MediaTypeNames.Application.Xml }
                }
            }
        };

        byte[] responseBytes = await SendAsync(request).ConfigureAwait(false);

        return Route53Serializer<TResult>.DeserializeXml(responseBytes);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new Route53Exception(responseText, response.StatusCode);
    }
}