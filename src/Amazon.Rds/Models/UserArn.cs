namespace Amazon.Rds;

public readonly struct UserArn(
    AwsRegion region,
    string accountId,
    string databaseId,
    string userName)
{
    private readonly string _name = $"arn:aws:rds-db:{region.Name}:{accountId}:dbuser:{databaseId}/{userName}";

    public override string ToString() => _name;
}

// arn:aws:rds-db:us-west-2:12345678:dbuser:db-12ABC34DEFG5HIJ6KLMNOP78QR/jane_doe