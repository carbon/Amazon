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

        public override string ToString() 
            => Name;

        public bool Equals(AwsService other)
            => Name == other.Name;

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

        public override string ToString() 
            => Name;

        public bool Equals(AwsRegion other)
            => Name == other.Name;

        public static readonly AwsRegion Standard       = new AwsRegion("us-east-1"); // US East (N. Virginia)
        public static readonly AwsRegion USEast1        = Standard;
        public static readonly AwsRegion USEast2        = new AwsRegion("us-east-2"); // US East (Ohio)
        public static readonly AwsRegion USWest1        = new AwsRegion("us-west-1");
        public static readonly AwsRegion USWest2        = new AwsRegion("us-west-2");
        public static readonly AwsRegion SAEast1        = new AwsRegion("sa-east-1");
        public static readonly AwsRegion APNorthEast1   = new AwsRegion("ap-northeast-1");

        public static readonly AwsRegion Google = new AwsRegion("google");

        public static implicit operator AwsRegion(string name)
            => new AwsRegion(name);
    }
}