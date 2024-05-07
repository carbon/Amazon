using System.Globalization;

using Amazon.Helpers;

namespace Amazon.Route53;

public sealed class ListGeoLocationsRequest
{
    public int? MaxItems { get; set; }

    public string? StartContinentCode { get; set; }

    public string? StartCountryCode { get; set; }

    public string? StartSubdivision { get; set; }

    public string ToQueryString()
    {
        var items = new List<KeyValuePair<string, string>>(4);

        if (MaxItems.HasValue)          items.Add(new("maxitems", MaxItems.Value.ToString(CultureInfo.InvariantCulture)));
        if (StartContinentCode != null) items.Add(new("startcontinentcode", StartContinentCode));
        if (StartCountryCode != null)   items.Add(new("startcountrycode", StartCountryCode));
        if (StartSubdivision != null)   items.Add(new("startsubdivision", StartSubdivision));

        return items.ToQueryString();
    }
}