namespace Amazon.S3
{
    public readonly struct StorageClass
    {
        private StorageClass(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;

        public static readonly StorageClass Standard                 = new StorageClass("STANDARD");
        public static readonly StorageClass StandardInfrequentAccess = new StorageClass("STANDARD_IA");
        public static readonly StorageClass ReducedRedundancy        = new StorageClass("REDUCED_REDUNDANCY");
        public static readonly StorageClass Glacier                  = new StorageClass("GLACIER"); 
    }

    // STANDARD | STANDARD_IA | REDUCED_REDUNDANCY
}