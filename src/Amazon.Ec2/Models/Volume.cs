using System;

namespace Amazon.Ec2
{
    public class Volume
    {
        public int Iops { get; set; }

        public int Size { get; set; }

        public string Status { get; set; }

        public string AvailabilityZone { get; set; }

        public string VolumeId { get; set; }

        public string VolumeType { get; set; } // standard | io1 | gp2 | sc1 | st1
    }

    public class VolumeAttachment
    {
        public DateTime AttachTime { get; set; }

        public string VolumeId { get; set; }

        public string InstanceId { get; set; }

        public string Device { get; set; }

        public string Status { get; set; }
    }
}
