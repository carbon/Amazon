using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class DescribeDocumentPermissionRequest : ISsmRequest
    {
        public DescribeDocumentPermissionRequest() { }

        public DescribeDocumentPermissionRequest(string name, string permissionType)
        {
            Name = name;
            PermissionType = permissionType;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PermissionType { get; set; }
    }
}