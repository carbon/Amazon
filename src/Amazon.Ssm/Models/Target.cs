using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class Target
    {
        [StringLength(128)]
        public string Key { get; set; }

        public string[] Values { get; set; }
    }
}