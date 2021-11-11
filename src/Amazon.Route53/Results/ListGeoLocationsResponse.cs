#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53;

public sealed class ListGeoLocationsResponse
{
    [XmlArrayItem("GeoLocationDetails")]
    public GeoLocationDetails[] GeoLocationDetailsList { get; init; }

    public bool IsTruncated { get; init; }

    public string NextCountryCode { get; init; }

    public string NextSubdivisionCode { get; init; }

    public int MaxItems { get; init; }
}