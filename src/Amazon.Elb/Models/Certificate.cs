#nullable disable

namespace Amazon.Elb;

public sealed class Certificate
{
    public Certificate() { }

    public Certificate(string arn)
    {
        ArgumentNullException.ThrowIfNull(arn);

        CertificateArn = arn;
    }

    public string CertificateArn { get; init; }
}