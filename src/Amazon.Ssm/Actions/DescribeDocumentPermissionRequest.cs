namespace Amazon.Ssm;

public sealed class DescribeDocumentPermissionRequest : ISsmRequest
{
    public DescribeDocumentPermissionRequest(string name, string permissionType)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(permissionType);

        Name = name;
        PermissionType = permissionType;
    }

    public string Name { get; }

    public string PermissionType { get; }
}
