using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

using Amazon.Route53.Exceptions;

namespace Amazon.Route53;

public sealed class Route53Client : AwsClient
{
    public const string Namespace = "https://route53.amazonaws.com/doc/2013-04-01/";
    public const string Version = "2013-04-01";

    public Route53Client(IAwsCredential credential)
        : base(AwsService.Route53, AwsRegion.USEast1, credential)
    {
    }

    public async Task<ChangeResourceRecordSetsResponse> ChangeResourceRecordSetsAsync(string hostedZoneId, ChangeResourceRecordSetsRequest request)
    {
            return await PostXmlAsync<ChangeResourceRecordSetsRequest, ChangeResourceRecordSetsResponse>(
                path     : $"/hostedzone/{hostedZoneId}/rrset",
                data : request
        ).ConfigureAwait(false);
    }

    public async Task<ListResourceRecordSetsResponse> ListResourceRecordSetsAsync(ListResourceRecordSetsRequest request)
    {
        return await GetAsync<ListResourceRecordSetsResponse>($"/hostedzone/{request.Id}/rrset" + request.ToQueryString()).ConfigureAwait(false);
    }

    #region Geolocations

    public async Task<ListGeoLocationsResponse> ListGeoLocationsAsync(ListGeoLocationsRequest request)
    {
        return await GetAsync<ListGeoLocationsResponse>($"/geolocations" + request.ToQueryString()).ConfigureAwait(false);
    }

    #endregion

    private const string baseUrl = "https://route53.amazonaws.com/";

    private async Task<TResult> GetAsync<TResult>(string path)
        where TResult: notnull
    {
        string url = baseUrl + Version + path;

        var responseText = await SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).ConfigureAwait(false);

        return Route53Serializer<TResult>.DeserializeXml(responseText);
    }

    private async Task<TResult> PostXmlAsync<T, TResult>(string path, T data)
        where T: notnull
        where TResult: notnull
    {
        string url = baseUrl + Version + path;

        var request = new HttpRequestMessage(HttpMethod.Post, url) {
            Content = new ByteArrayContent(Route53Serializer<T>.SerializeToUtf8Bytes(data)) {
                Headers = {
                    { "Content-Type", MediaTypeNames.Application.Xml }
                }
            }
        };

        string responseText = await SendAsync(request).ConfigureAwait(false);

        return Route53Serializer<TResult>.DeserializeXml(responseText);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new Route53Exception(responseText, response.StatusCode);
    }
}