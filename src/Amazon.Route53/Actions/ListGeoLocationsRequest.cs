#nullable disable

using Amazon.Helpers;

namespace Amazon.Route53;

public sealed class ListGeoLocationsRequest
{
    public int? MaxItems { get; set; }

    public string StartContinentCode { get; set; }

    public string StartCountryCode { get; set; }

    public string StartSubdivision { get; set; }

    public string ToQueryString()
    {
        var items = new Dictionary<string, string>();

        if (MaxItems != null) items.Add("maxitems", MaxItems.Value.ToString());
        if (StartContinentCode != null) items.Add("startcontinentcode", StartContinentCode);
        if (StartCountryCode != null) items.Add("startcountrycode", StartCountryCode);
        if (StartSubdivision != null) items.Add("startsubdivision", StartSubdivision);

        return items.ToQueryString();
    }
}