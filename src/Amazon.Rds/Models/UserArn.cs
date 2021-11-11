namespace Amazon.Rds;

public readonly struct UserArn
{
    private readonly string _name;

    public UserArn(
        AwsRegion region,
        string accountId,
        string databaseId,
        string userName)
    {
        _name = $"arn:aws:rds-db:{region.Name}:{accountId}:dbuser:{databaseId}/{userName}";
    }

    public override string ToString() => _name;
}

// arn:aws:rds-db:us-west-2:12345678:dbuser:db-12ABC34DEFG5HIJ6KLMNOP78QR/jane_doe