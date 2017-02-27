using System;

namespace Amazon
{
    public class AwsRegion : IEquatable<AwsRegion>
    {
        private AwsRegion(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;

        #region Equality

        public bool Equals(AwsRegion other) => 
            other != null && Name == other.Name;

        public override bool Equals(object obj) =>
            Equals(obj as AwsRegion);

        public static bool operator ==(AwsRegion lhs, AwsRegion rhs) =>
            lhs?.Name == rhs?.Name;

        public static bool operator !=(AwsRegion lhs, AwsRegion rhs) =>
            lhs?.Name != rhs?.Name;
        
        public override int GetHashCode() => Name.GetHashCode();

        #endregion

        // Soon: Paris (France), Ningxia (China)

        public static readonly AwsRegion USEast1      = new AwsRegion("us-east-1");      // | US East       | N. Virginia
        public static readonly AwsRegion USEast2      = new AwsRegion("us-east-2");      // | US East       | Ohio
        public static readonly AwsRegion USWest1      = new AwsRegion("us-west-1");      // | US West       | N. California
        public static readonly AwsRegion USWest2      = new AwsRegion("us-west-2");      // | US West       | Oregon
        public static readonly AwsRegion CACentral1   = new AwsRegion("ca-central-1");   // | Canada        | Central
        public static readonly AwsRegion APSouth1     = new AwsRegion("ap-south-1");     // | Asia Pacific  | Mumbai
        public static readonly AwsRegion APSouthEast1 = new AwsRegion("ap-southeast-1"); // | Asia Pacific  | Singapore
        public static readonly AwsRegion APSouthEast2 = new AwsRegion("ap-southeast-2"); // | Asia Pacific  | Sydney
        public static readonly AwsRegion APNorthEast1 = new AwsRegion("ap-northeast-1"); // | Asia Pacific  | Tokyo
        public static readonly AwsRegion APNortheast2 = new AwsRegion("ap-northeast-2"); // | Asia Pacific  | Seoul	
        public static readonly AwsRegion EUCentral1   = new AwsRegion("eu-central-1");   // | EU            | Frankfurt
        public static readonly AwsRegion EUWest1      = new AwsRegion("eu-west-1");      // | EU            | Ireland
        public static readonly AwsRegion EUWest2      = new AwsRegion("eu-west-2");      // | EU            | London
        public static readonly AwsRegion SAEast1      = new AwsRegion("sa-east-1");      // | South America | São Paulo
        public static readonly AwsRegion CNNorth1     = new AwsRegion("cn-north-1");     // | China         | Beijing
        public static readonly AwsRegion USGovWest1   = new AwsRegion("us-gov-west-1");  // | US            | AWS GovCloud

        public static AwsRegion[] All = new AwsRegion[] {
            USEast1,
            USEast2,
            USWest1,
            USWest2,
            CACentral1,
            APSouth1,
            APSouthEast1,
            APSouthEast2,
            APNorthEast1,
            APNortheast2,
            EUCentral1,
            EUWest1,
            EUWest2,
            SAEast1,
            CNNorth1,
            USGovWest1
        };

        public static readonly AwsRegion Standard = USEast1;

        public static AwsRegion Get(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            switch (name)
            {
                case "ap-south-1"     : return APSouth1;
                case "ap-southeast-1" : return APSouthEast1;
                case "ap-southeast-2" : return APSouthEast2;
                case "ap-northeast-1" : return APNorthEast1;
                case "ap-northeast-2" : return APNortheast2;
                case "us-east-1"      : return USEast1;
                case "us-east-2"      : return USEast2;
                case "us-west-1"      : return USWest1;
                case "us-west-2"      : return USWest2;
                case "ca-central-1"   : return CACentral1;
                case "eu-central-1"   : return EUCentral1;
                case "eu-west-1"      : return EUWest1;
                case "eu-west-2"      : return EUWest2;
                case "sa-east-1"      : return SAEast1;
                case "cn-north-1"     : return CNNorth1;
                case "us-gov-west-1"  : return USGovWest1;
            }

            throw new ArgumentException("Unexpected region:" + name);
        }
    }
}