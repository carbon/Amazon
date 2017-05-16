using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class DeleteMaintenanceWindowRequest
    {
        [Required]
        public string WindowId { get; set; }
    }
}