using System;

namespace Amazon
{
    public class CredentialScope
    {
        public CredentialScope(DateTime date, AwsRegion region, AwsService service)
        {
            #region Preconditions

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            #endregion

            Date = date;
            Region = region;
            Service = service;
        }

        public DateTime Date { get; }

        public AwsRegion Region { get; }

        public AwsService Service { get; }

        // 20120228/us-east-1/iam/aws4_request
        public override string ToString()
            => $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request";
    }

    // http://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html
    public class AwsService : IEquatable<AwsService>
    {
        private AwsService(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;

        public bool Equals(AwsService other) => Name == other.Name;

        public static readonly AwsService Cloudfront        = new AwsService("cloudfront");
        public static readonly AwsService CloudwatchEvents  = new AwsService("events");
        public static readonly AwsService DynamoDb          = new AwsService("dynamodb");
        public static readonly AwsService Ec2               = new AwsService("ec2");
        public static readonly AwsService Elb               = new AwsService("elasticloadbalancing");
        public static readonly AwsService ElastiCache       = new AwsService("elasticache");
        public static readonly AwsService Glacier           = new AwsService("glacier");
        public static readonly AwsService Iam               = new AwsService("iam");
        public static readonly AwsService Kinesis           = new AwsService("kinesis");
        public static readonly AwsService Kms               = new AwsService("kms");
        public static readonly AwsService Lambda            = new AwsService("lambda");
        public static readonly AwsService Monitoring        = new AwsService("monitoring"); // Cloudwatch monitoring
        public static readonly AwsService Ses               = new AwsService("email");
        public static readonly AwsService S3                = new AwsService("s3");
        public static readonly AwsService Sns               = new AwsService("sns");
        public static readonly AwsService Sts               = new AwsService("sts");
        public static readonly AwsService Sqs               = new AwsService("sqs");

        public static implicit operator AwsService(string name)
           => new AwsService(name);
    }

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
            return this.Equals(obj as AwsRegion);

        public static bool operator ==(AwsRegion lhs, AwsRegion rhs) =>
            lhs.Name == rhs.Name;

        public static bool operator !=(AwsRegion lhs, AwsRegion rhs) =>
            lhs.Name != rhs.Name;
        
        public override int GetHashCode() =>    
            Name.GetHashCode();

        #endregion

        public static readonly AwsRegion Standard     = USEast1;       
        public static readonly AwsRegion USEast1      = new AwsRegion("us-east-1");      // US East (N. Virginia)
        public static readonly AwsRegion USEast2      = new AwsRegion("us-east-2");      // US East (Ohio)
        public static readonly AwsRegion USWest1      = new AwsRegion("us-west-1");      // US West (N. California)
        public static readonly AwsRegion USWest2      = new AwsRegion("us-west-2");      // US West (Oregon)
        public static readonly AwsRegion CACentral1   = new AwsRegion("ca-central-1");   // Canada (Central)
        public static readonly AwsRegion APSouth1     = new AwsRegion("ap-south-1");     // Asia Pacific (Mumbai)
        public static readonly AwsRegion APSouthEast1 = new AwsRegion("ap-southeast-1"); // Asia Pacific (Singapore)
        public static readonly AwsRegion APSouthEast2 = new AwsRegion("ap-southeast-2"); // Asia Pacific (Sydney)
        public static readonly AwsRegion APNorthEast1 = new AwsRegion("ap-northeast-1"); // Asia Pacific (Tokyo)
        public static readonly AwsRegion APNortheast2 = new AwsRegion("ap-northeast-2"); // Asia Pacific (Seoul)	
        public static readonly AwsRegion EUCentral1   = new AwsRegion("eu-central-1");   // EU (Frankfurt)
        public static readonly AwsRegion EUWest1      = new AwsRegion("eu-west-1");      // EU (Ireland)
        public static readonly AwsRegion EUWest2      = new AwsRegion("eu-west-2");      // EU (London)
        public static readonly AwsRegion SAEast1      = new AwsRegion("sa-east-1");      // South America (São Paulo)

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
            }

            // TODO: Validate format

            return new AwsRegion(name);
        }

        public static implicit operator AwsRegion(string name) => Get(name);
    }
}