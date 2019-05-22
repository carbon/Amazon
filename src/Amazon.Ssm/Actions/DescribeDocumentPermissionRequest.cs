using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class DescribeDocumentPermissionRequest : ISsmRequest
    {
        public DescribeDocumentPermissionRequest(string name, string permissionType)
        {
            Name = name;
            PermissionType = permissionType;
        }

        [Required]
        public string Name { get; }

        [Required]
        public string PermissionType { get; }
    }
}