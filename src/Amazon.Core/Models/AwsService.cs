using System;
using System.Collections.Generic;

namespace Amazon
{
    public class AwsService : IEquatable<AwsService>
    {
        private AwsService(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;

        public static readonly AwsService Cloudfront       = new AwsService("cloudfront");
        public static readonly AwsService CloudwatchEvents = new AwsService("events");
        public static readonly AwsService DynamoDb         = new AwsService("dynamodb");
        public static readonly AwsService Ec2              = new AwsService("ec2");
        public static readonly AwsService Elb              = new AwsService("elasticloadbalancing");
        public static readonly AwsService ElastiCache      = new AwsService("elasticache");
        public static readonly AwsService Glacier          = new AwsService("glacier");
        public static readonly AwsService Iam              = new AwsService("iam");
        public static readonly AwsService Kinesis          = new AwsService("kinesis");
        public static readonly AwsService Kms              = new AwsService("kms");
        public static readonly AwsService Lambda           = new AwsService("lambda");
        public static readonly AwsService Monitoring       = new AwsService("monitoring"); // Cloudwatch monitoring
        public static readonly AwsService Ses              = new AwsService("email");
        public static readonly AwsService S3               = new AwsService("s3");
        public static readonly AwsService Sns              = new AwsService("sns");
        public static readonly AwsService Sts              = new AwsService("sts");
        public static readonly AwsService Sqs              = new AwsService("sqs");

        #region Equality

        public bool Equals(AwsService other) =>
            other != null && Name == other.Name;

        public override bool Equals(object obj) =>
            this.Equals(obj as AwsService);

        public static bool operator ==(AwsService lhs, AwsService rhs) =>
            lhs?.Name == rhs?.Name;

        public static bool operator !=(AwsService lhs, AwsService rhs) =>
            lhs?.Name != rhs?.Name;

        public override int GetHashCode() =>
            Name.GetHashCode();

        #endregion

        public static implicit operator AwsService(string name)
           => new AwsService(name);
    }
}

// http://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html
