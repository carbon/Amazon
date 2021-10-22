#nullable disable


namespace Amazon.Ec2;

public sealed class IamInstanceProfileSpecification
{
    public IamInstanceProfileSpecification() { }

    public IamInstanceProfileSpecification(string nameOrArn)
    {
        ArgumentNullException.ThrowIfNull(nameOrArn);

        if (nameOrArn.StartsWith("arn:", StringComparison.Ordinal))
        {
            Arn = nameOrArn;
        }
        else
        {
            Name = nameOrArn;
        }
    }

    public string Arn { get; init; }

    public string Name { get; init; }
}
