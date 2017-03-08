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
        public static readonly AwsRegion APNortheast2 = new AwsRegion("ap-northeast-2"); // | Asia Pacific  | Seoul	        | 2016-01-06
        public static readonly AwsRegion APSouth1     = new AwsRegion("ap-south-1");     // | Asia Pacific  | Mumbai        | 2016-06-27
        public static readonly AwsRegion USEast2      = new AwsRegion("us-east-2");      // | US            | Ohio          | 2016-10-17
        public static readonly AwsRegion CACentral1   = new AwsRegion("ca-central-1");   // | Canada        | Central       | 2016-12-08
        public static readonly AwsRegion EUWest2      = new AwsRegion("eu-west-2");      // | EU            | London        | 2016-12-13
        
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
            EUWest2      
        };
        
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