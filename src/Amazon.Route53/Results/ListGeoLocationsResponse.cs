#nullable disable


using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class ListGeoLocationsResponse
    {
        [XmlArrayItem("GeoLocationDetails")]
        public GeoLocationDetails[] GeoLocationDetailsList { get; set; }

        public bool IsTruncated { get; set; }

        public string NextCountryCode { get; set; }

        public string NextSubdivisionCode { get; set; }

        public int MaxItems { get; set; }
    }

}