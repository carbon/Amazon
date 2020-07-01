using System;

namespace Amazon
{
    public sealed class AwsService : IEquatable<AwsService>
    {
        private AwsService(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public override string ToString() => Name;

        public static readonly AwsService Cloudfront       = new AwsService("cloudfront");
        public static readonly AwsService CloudwatchEvents = new AwsService("events");
        public static readonly AwsService CodeBuild        = new AwsService("codebuild");
        public static readonly AwsService DynamoDb         = new AwsService("dynamodb");
        public static readonly AwsService Ec2              = new AwsService("ec2");
        public static readonly AwsService Elb              = new AwsService("elasticloadbalancing");
        public static readonly AwsService ElastiCache      = new AwsService("elasticache");
        public static readonly AwsService Glacier          = new AwsService("glacier");
        public static readonly AwsService Iam              = new AwsService("iam");
        public static readonly AwsService Kinesis          = new AwsService("kinesis");
        public static readonly AwsService KinesisFirehose  = new AwsService("firehose");
        public static readonly AwsService Kms              = new AwsService("kms");
        public static readonly AwsService Lambda           = new AwsService("lambda");
        public static readonly AwsService Monitoring       = new AwsService("monitoring"); // Cloudwatch monitoring
        public static readonly AwsService Route53          = new AwsService("route53");
        public static readonly AwsService RdsDb            = new AwsService("rds-db");
        public static readonly AwsService Ses              = new AwsService("email");
        public static readonly AwsService Ssm              = new AwsService("ssm"); // Amazon EC2 Systems Manager (SSM)
        public static readonly AwsService S3               = new AwsService("s3");
        public static readonly AwsService Sns              = new AwsService("sns");
        public static readonly AwsService Sts              = new AwsService("sts");
        public static readonly AwsService Sqs              = new AwsService("sqs");
        public static readonly AwsService Translate        = new AwsService("translate");
        public static readonly AwsService Waf              = new AwsService("waf");

        public bool Equals(AwsService? other)
        {
            return ReferenceEquals(this, other) || Name.Equals(other?.Name, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj) => this.Equals(obj as AwsService);

        public static bool operator ==(AwsService lhs, AwsService rhs) => lhs.Equals(rhs);

        public static bool operator !=(AwsService lhs, AwsService rhs) => !lhs.Equals(rhs);

        public override int GetHashCode() => Name.GetHashCode();

        public static implicit operator AwsService(string name) => new AwsService(name);
    }
}

// http://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html