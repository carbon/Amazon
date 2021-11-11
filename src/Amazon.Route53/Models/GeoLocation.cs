#nullable disable

namespace Amazon.Route53;

public sealed class GeoLocation
{
    public GeoLocation() { }

#nullable enable

    public GeoLocation(
        string continentCode,
        string? countryCode = null,
        string? subdivisionCode = null)
    {
        ContinentCode = continentCode;
        CountryCode = countryCode;
        SubdivisionCode = subdivisionCode;
    }

    public string ContinentCode { get; init; }

    public string? CountryCode { get; init; }

    public string? SubdivisionCode { get; init; }
}
