using Amazon;

namespace Wasabi;

public sealed class WasabiEndpoint(string host, AwsRegion region)
{
    public string Host { get; } = host;

    public AwsRegion Region { get; } = region;

    public static readonly WasabiEndpoint USEast1      = new("s3.us-east-1.wasabisys.com",      AwsRegion.USEast1);      // N.Virginia
    public static readonly WasabiEndpoint USEast2      = new("s3.us-east-2.wasabisys.com",      AwsRegion.USEast2);      // N.Virginia
    public static readonly WasabiEndpoint USCentral1   = new("s3.us-central-1.wasabisys.com",   "us-central-1");         // Texas
    public static readonly WasabiEndpoint USWest1      = new("s3.us-west-1.wasabisys.com",      AwsRegion.USWest1);      // Oregon
    public static readonly WasabiEndpoint CACentral1   = new("s3.ca-central-1.wasabisys.com",   AwsRegion.CACentral1);   // Toronto
    public static readonly WasabiEndpoint EUCentral1   = new("s3.eu-central-1.wasabisys.com",   AwsRegion.EUCentral1);   // Amsterdam     | s3.nl-1.wasabisys.com
    public static readonly WasabiEndpoint EUCentral2   = new("s3.eu-central-2.wasabisys.com",   AwsRegion.EUCentral2);   // Frankfurt     | s3.de-1.wasabisys.com
    public static readonly WasabiEndpoint EUWest1      = new("s3.eu-west-1.wasabisys.com",      AwsRegion.EUWest1);      // London    
    public static readonly WasabiEndpoint EUWest2      = new("s3.eu-west-2.wasabisys.com",      AwsRegion.EUWest2);      // Paris         | s3.fr-1.wasabisys.com
    public static readonly WasabiEndpoint APNorthEast1 = new("s3.ap-northeast-1.wasabisys.com", AwsRegion.APNorthEast1); // Tokyo
    public static readonly WasabiEndpoint APNorthEast2 = new("s3.ap-northeast-2.wasabisys.com", AwsRegion.APNorthEast2); // Osaka
    public static readonly WasabiEndpoint APSouthEast1 = new("s3.ap-southeast-1.wasabisys.com", AwsRegion.APSouthEast1); // Singapore
    public static readonly WasabiEndpoint APSouthEast2 = new("s3.ap-southeast-2.wasabisys.com", AwsRegion.APSouthEast2); // Sydney
}