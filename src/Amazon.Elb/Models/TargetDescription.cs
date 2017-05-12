using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class TargetDescription
    {
        [Required]
        public string Id { get; set; }

        [Range(1, 65535)]
        public ushort? Port { get; set; }
    }
}
