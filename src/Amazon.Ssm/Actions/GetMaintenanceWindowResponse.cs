#nullable disable

namespace Amazon.Ssm
{
    public class GetMaintenanceWindowResponse
    {
        public bool AllowUnassociatedTargets { get; set; }

        public long CreatedDate { get; set; }

        public int Cutoff { get; set; }

        public int Duration { get; set; }

        public bool Enabled { get; set; }

        public long ModifiedDate { get; set; }

        public string Name { get; set; }

        public string Schedule { get; set; }

        public string WindowId { get; set; }
    }
}