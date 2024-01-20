using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class Certificate
{
    public Certificate() { }

    [SetsRequiredMembers]
    public Certificate(string arn)
    {
        ArgumentException.ThrowIfNullOrEmpty(arn);

        CertificateArn = arn;
    }

    public required string CertificateArn { get; init; }
}