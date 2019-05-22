#nullable disable


namespace Amazon.Route53
{
    public sealed class GetGeoLocationRequest
    {
        public GetGeoLocationRequest() { }

        public GetGeoLocationRequest(string continentCode, string countryCode, string subdivisionCode)
        {
            ContinentCode = continentCode;
            CountryCode = countryCode;
            SubdivisionCode = subdivisionCode;
        }

        public string ContinentCode { get; set; }

        public string CountryCode { get; set; }

        public string SubdivisionCode { get; set; }

    }

    // AF, AN, AS, EU, OC, NA, SA
}
