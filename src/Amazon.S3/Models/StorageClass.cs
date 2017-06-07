namespace Amazon.S3
{
    public class StorageClass
    {
        internal StorageClass(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public static readonly StorageClass Standard                 = new StorageClass("STANDARD");
        public static readonly StorageClass StandardInfrequentAccess = new StorageClass("STANDARD_IA");
        public static readonly StorageClass ReducedRedundancy        = new StorageClass("REDUCED_REDUNDANCY");
    }

    // STANDARD | STANDARD_IA | REDUCED_REDUNDANCY
}