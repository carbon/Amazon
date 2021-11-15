#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Route53;

public sealed class GetGeoLocationRequest
{
    public GetGeoLocationRequest() { }

    public GetGeoLocationRequest(
        string continentCode,
        string countryCode,
        string subdivisionCode)
    {
        ContinentCode = continentCode;
        CountryCode = countryCode;
        SubdivisionCode = subdivisionCode;
    }

    public string ContinentCode { get; init; }

    [MaxLength(2)]
    public string CountryCode { get; init; }

    public string SubdivisionCode { get; init; }
}

// AF, AN, AS, EU, OC, NA, SA
