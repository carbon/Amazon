using System;
using System.Text;
using System.Threading.Tasks;

using Amazon.Metadata;

namespace Amazon;

public sealed class AwsRegion : IEquatable<AwsRegion>
{
    public AwsRegion(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    public string Name { get; }

    public override string ToString() => Name;

    private byte[]? utf8Bytes;

    internal byte[] Utf8Name => utf8Bytes ??= Encoding.ASCII.GetBytes(Name); 

    #region Equality

    public bool Equals(AwsRegion? other)
    {
        if (other is null) return this is null;

        return ReferenceEquals(this, other) || Name.Equals(other.Name, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => Equals(obj as AwsRegion);

    public static bool operator ==(AwsRegion lhs, AwsRegion rhs) => lhs.Equals(rhs);

    public static bool operator !=(AwsRegion lhs, AwsRegion rhs) =>lhs?.Name != rhs?.Name;
        
    public override int GetHashCode() => Name.GetHashCode();

    #endregion

    // In launch order...
    public static readonly AwsRegion USEast1      = new ("us-east-1");      // | US            | N. Virginia   | 2006-08-25
    public static readonly AwsRegion EUWest1      = new ("eu-west-1");      // | EU            | Ireland       | 2008-12-10
    public static readonly AwsRegion USWest1      = new ("us-west-1");      // | US            | N. California | 2009-12-03
    public static readonly AwsRegion APSouthEast1 = new ("ap-southeast-1"); // | Asia Pacific  | Singapore     | 2010-04-29
    public static readonly AwsRegion APNorthEast1 = new ("ap-northeast-1"); // | Asia Pacific  | Tokyo         | 2011-03-02
    public static readonly AwsRegion USGovWest1   = new ("us-gov-west-1");  // | US            | AWS GovCloud  | 2011-08-16
    public static readonly AwsRegion USWest2      = new ("us-west-2");      // | US            | Oregon        | 2011-11-09
    public static readonly AwsRegion SAEast1      = new ("sa-east-1");      // | South America | São Paulo     | 2011-12-14
    public static readonly AwsRegion APSouthEast2 = new ("ap-southeast-2"); // | Asia Pacific  | Sydney        | 2012-11-12
    public static readonly AwsRegion CNNorth1     = new ("cn-north-1");     // | China         | Beijing       | 2013-12-18
    public static readonly AwsRegion EUCentral1   = new ("eu-central-1");   // | EU            | Frankfurt     | 2014-10-23
    public static readonly AwsRegion APNortheast2 = new ("ap-northeast-2"); // | Asia Pacific  | Seoul         | 2016-01-06
    public static readonly AwsRegion APSouth1     = new ("ap-south-1");     // | Asia Pacific  | Mumbai        | 2016-06-27
    public static readonly AwsRegion USEast2      = new ("us-east-2");      // | US            | Ohio          | 2016-10-17
    public static readonly AwsRegion CACentral1   = new ("ca-central-1");   // | Canada        | Central       | 2016-12-08
    public static readonly AwsRegion EUWest2      = new ("eu-west-2");      // | EU            | London        | 2016-12-13
    public static readonly AwsRegion EUWest3      = new ("eu-west-3");      // | EU            | Paris         | 2017-12-18
    public static readonly AwsRegion MESouth1     = new ("me-south-1");     // | ME            | Bahrain       | 2019-07-29
    public static readonly AwsRegion AFSouth1     = new ("af-south-1");     // |               | Cape Town     | ~2020-04
    public static readonly AwsRegion EUSouth1     = new ("eu-south-1");     // | EU            | Milan         | ~2020-05	
    public static readonly AwsRegion APSouthEast3 = new ("ap-southeast-3"); // | Asia Pacific  | Jakarta       | ~2021-11

    public static readonly AwsRegion[] All = new [] {
        USEast1,
        EUWest1,  
        USWest1,     
        APSouthEast1, 
        APNorthEast1, 
        USGovWest1,   
        USWest2,
        SAEast1,      
        APSouthEast2, 
        CNNorth1,     
        EUCentral1,   
        APNortheast2, 
        APSouth1,     
        USEast2,      
        CACentral1,   
        EUWest2,
        EUWest3,
        MESouth1,
        AFSouth1,
        EUSouth1,
        APSouthEast3
    };
        
    public static AwsRegion Get(string name) => name switch
    {
        "ap-south-1"     => APSouth1,
        "ap-southeast-1" => APSouthEast1,
        "ap-southeast-2" => APSouthEast2,
        "ap-southeast-3" => APSouthEast3,
        "ap-northeast-1" => APNorthEast1,
        "ap-northeast-2" => APNortheast2,
        "us-east-1"      => USEast1,
        "us-east-2"      => USEast2,
        "us-west-1"      => USWest1,
        "us-west-2"      => USWest2,
        "ca-central-1"   => CACentral1,
        "eu-central-1"   => EUCentral1,
        "eu-west-1"      => EUWest1,
        "eu-west-2"      => EUWest2,
        "eu-west-3"      => EUWest3,
        "sa-east-1"      => SAEast1,
        "cn-north-1"     => CNNorth1,
        "us-gov-west-1"  => USGovWest1,
        "me-south-1"     => MESouth1,
        "af-south-1"     => AFSouth1,
        "eu-south-1"     => EUSouth1,
        _                => new AwsRegion(name)
    };

    private static AwsRegion? current;

    public static AwsRegion Get()
    {
        if (current is null)
        {
            string availabilityZone = InstanceMetadataService.Instance.GetAvailabilityZone();

            current = FromAvailabilityZone(availabilityZone);
        }

        return current;
    }

    public static async ValueTask<AwsRegion> GetAsync()
    {
        if (current is null)
        {
            string availabilityZone = await InstanceMetadataService.Instance.GetAvailabilityZoneAsync().ConfigureAwait(false);

            current = FromAvailabilityZone(availabilityZone);
        }

        return current;
    }

    public static AwsRegion FromAvailabilityZone(string availabilityZone)
    {
        string regionName = availabilityZone[0..^1];

        return Get(regionName);
    }
}