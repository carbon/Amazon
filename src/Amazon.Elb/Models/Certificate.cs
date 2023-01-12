#nullable disable

namespace Amazon.Elb;

public sealed class Certificate
{
    public Certificate() { }

#nullable enable

    public Certificate(string arn)
    {
        ArgumentException.ThrowIfNullOrEmpty(arn);

        CertificateArn = arn;
    }

    public string CertificateArn { get; init; }
}