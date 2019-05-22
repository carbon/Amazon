#nullable disable

namespace Amazon.Route53
{
    public class GeoLocation
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

        public string ContinentCode { get; set; }

        public string? CountryCode { get; set; }

        public string? SubdivisionCode { get; set; }
    }
}
