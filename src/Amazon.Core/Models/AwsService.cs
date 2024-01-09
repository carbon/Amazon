﻿using System.Text;

namespace Amazon;

public sealed class AwsService(string name) : IEquatable<AwsService>
{
    private byte[]? _utf8Bytes;

    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    internal ReadOnlySpan<byte> Utf8Name => _utf8Bytes ??= Encoding.ASCII.GetBytes(Name);

    public override string ToString() => Name;

    public static readonly AwsService CloudFront       = new("cloudfront");
    public static readonly AwsService CloudWatchEvents = new("events");
    public static readonly AwsService CodeBuild        = new("codebuild");
    public static readonly AwsService DynamoDb         = new("dynamodb");
    public static readonly AwsService Ec2              = new("ec2");
    public static readonly AwsService Elb              = new("elasticloadbalancing");
    public static readonly AwsService ElastiCache      = new("elasticache");
    public static readonly AwsService Glacier          = new("glacier");
    public static readonly AwsService Iam              = new("iam");
    public static readonly AwsService Kinesis          = new("kinesis");
    public static readonly AwsService KinesisFirehose  = new("firehose");
    public static readonly AwsService Kms              = new("kms");
    public static readonly AwsService Lambda           = new("lambda");
    public static readonly AwsService Monitoring       = new("monitoring"); // CloudWatch monitoring
    public static readonly AwsService OpenSearch       = new("es");
    public static readonly AwsService Rekognition      = new("rekognition");
    public static readonly AwsService Route53          = new("route53");
    public static readonly AwsService RdsDb            = new("rds-db");
    public static readonly AwsService Ses              = new("email");
    public static readonly AwsService Ssm              = new("ssm"); // Amazon EC2 Systems Manager (SSM)
    public static readonly AwsService S3               = new("s3");
    public static readonly AwsService Sns              = new("sns");
    public static readonly AwsService Sts              = new("sts");
    public static readonly AwsService Sqs              = new("sqs");
    public static readonly AwsService Waf              = new("waf");

    public bool Equals(AwsService? other)
    {
        if (other is null) return this is null;

        return ReferenceEquals(this, other) || Name.Equals(other.Name, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => obj is AwsService other && Equals(other);

    public static bool operator ==(AwsService lhs, AwsService rhs) => lhs.Equals(rhs);

    public static bool operator !=(AwsService lhs, AwsService rhs) => !lhs.Equals(rhs);

    public override int GetHashCode() => Name.GetHashCode();

    public static implicit operator AwsService(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return new AwsService(name);
    }
}

// http://docs.aws.amazon.com/general/latest/gr/aws-arns-and-namespaces.html