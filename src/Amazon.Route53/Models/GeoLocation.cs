namespace Amazon.Route53;

public sealed class GeoLocation
{
    public ContinentCode? ContinentCode { get; init; }

    public string? CountryCode { get; init; }

    public string? SubdivisionCode { get; init; }
}

public enum ContinentCode
{
    Unknown = 0,
    AF = 1,
    AN = 2,
    AS = 3,
    EU = 4, 
    OC = 5,
    NA = 6,
    SA = 7
}