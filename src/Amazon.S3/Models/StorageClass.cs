namespace Amazon.S3;

public readonly struct StorageClass
{
    private StorageClass(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public readonly override string ToString() => Name;

    public static readonly StorageClass Standard                 = new ("STANDARD");
    public static readonly StorageClass StandardInfrequentAccess = new ("STANDARD_IA");
    public static readonly StorageClass ReducedRedundancy        = new ("REDUCED_REDUNDANCY");
    public static readonly StorageClass Glacier                  = new ("GLACIER"); 
}