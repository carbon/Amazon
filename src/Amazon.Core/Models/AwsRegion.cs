using System;
using System.Threading.Tasks;

using Amazon.Metadata;

namespace Amazon
{
    public sealed class AwsRegion : IEquatable<AwsRegion>
    {
        public AwsRegion(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public override string ToString() => Name;

        #region Equality

        public bool Equals(AwsRegion? other) => ReferenceEquals(this, other) || Name == other?.Name;

        public override bool Equals(object? obj) => Equals(obj as AwsRegion);

        public static bool operator ==(AwsRegion lhs, AwsRegion rhs) => lhs.Equals(rhs);

        public static bool operator !=(AwsRegion lhs, AwsRegion rhs) =>
            lhs?.Name != rhs?.Name;
        
        public override int GetHashCode() => Name.GetHashCode();

        #endregion

        // In launch order...
        public static readonly AwsRegion USEast1      = new AwsRegion("us-east-1");      // | US            | N. Virginia   | 2006-08-25
        public static readonly AwsRegion EUWest1      = new AwsRegion("eu-west-1");      // | EU            | Ireland       | 2008-12-10
        public static readonly AwsRegion USWest1      = new AwsRegion("us-west-1");      // | US            | N. California | 2009-12-03
        public static readonly AwsRegion APSouthEast1 = new AwsRegion("ap-southeast-1"); // | Asia Pacific  | Singapore     | 2010-04-29
        public static readonly AwsRegion APNorthEast1 = new AwsRegion("ap-northeast-1"); // | Asia Pacific  | Tokyo         | 2011-03-02
        public static readonly AwsRegion USGovWest1   = new AwsRegion("us-gov-west-1");  // | US            | AWS GovCloud  | 2011-08-16
        public static readonly AwsRegion USWest2      = new AwsRegion("us-west-2");      // | US            | Oregon        | 2011-11-09
        public static readonly AwsRegion SAEast1      = new AwsRegion("sa-east-1");      // | South America | São Paulo     | 2011-12-14
        public static readonly AwsRegion APSouthEast2 = new AwsRegion("ap-southeast-2"); // | Asia Pacific  | Sydney        | 2012-11-12
        public static readonly AwsRegion CNNorth1     = new AwsRegion("cn-north-1");     // | China         | Beijing       | 2013-12-18
        public static readonly AwsRegion EUCentral1   = new AwsRegion("eu-central-1");   // | EU            | Frankfurt     | 2014-10-23
        public static readonly AwsRegion APNortheast2 = new AwsRegion("ap-northeast-2"); // | Asia Pacific  | Seoul         | 2016-01-06
        public static readonly AwsRegion APSouth1     = new AwsRegion("ap-south-1");     // | Asia Pacific  | Mumbai        | 2016-06-27
        public static readonly AwsRegion USEast2      = new AwsRegion("us-east-2");      // | US            | Ohio          | 2016-10-17
        public static readonly AwsRegion CACentral1   = new AwsRegion("ca-central-1");   // | Canada        | Central       | 2016-12-08
        public static readonly AwsRegion EUWest2      = new AwsRegion("eu-west-2");      // | EU            | London        | 2016-12-13
        public static readonly AwsRegion MESouth1     = new AwsRegion("me-south-1");     // | ME            | Bahrain       | ?

        // Soon: Paris (France), Ningxia (China)

        public static AwsRegion[] All = new AwsRegion[] {
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
            MESouth1
        };
        
        public static AwsRegion Get(string name) => name switch
        {
            "ap-south-1"     => APSouth1,
            "ap-southeast-1" => APSouthEast1,
            "ap-southeast-2" => APSouthEast2,
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
            "sa-east-1"      => SAEast1,
            "cn-north-1"     => CNNorth1,
            "us-gov-west-1"  => USGovWest1,
            "me-south-1"     => MESouth1,
            _                => new AwsRegion(name)
        };

        private static AwsRegion? current;

        // TODO: Return ValueTask<AwsRegion>
        public static async Task<AwsRegion> GetAsync()
        {
            if (current is null)
            {
                var availabilityZone = await InstanceMetadata.GetAvailabilityZoneAsync().ConfigureAwait(false);

                current = FromAvailabilityZone(availabilityZone);
            }

            return current;
        }

        public static AwsRegion FromAvailabilityZone(string availabilityZone)
        {
            string regionName = availabilityZone.Substring(0, availabilityZone.Length - 1);

            return Get(regionName);
        }
    }
}