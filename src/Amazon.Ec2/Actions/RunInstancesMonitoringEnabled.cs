namespace Amazon.Ec2
{
    public class RunInstancesMonitoringEnabled
    {
        public RunInstancesMonitoringEnabled() { }

        public RunInstancesMonitoringEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        public bool Enabled { get; set; }
    }
}
