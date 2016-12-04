namespace Amazon.Ec2
{
    public class Image
    {
        public string Architecture { get; set; }

        public string Description { get; set; }

        public bool EnaSupport { get; set; }

        public string Hypervisor { get; set; }

        public string ImageId { get; set; }

        public string ImageLocation { get; set; }

        public string ImageOwnerAlias { get; set; }

        public string ImageState { get; set; }

        public string ImageType { get; set; }

        public bool IsPublic { get; set; }

        public string KernelId { get; set; }

        public string Name { get; set; }

        public string Platform { get; set; }

        public string RamDiskId { get; set; }

        public string RootDeviceType { get; set; }

        public string VirtualizationType { get; set; }
    }
}
