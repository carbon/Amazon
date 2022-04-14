namespace Amazon.Ec2;

public readonly struct Filter
{
    public Filter(string name!!, string value!!)
    {
        Name = name;
        Value = value;
    }

    public Filter(string name!!, bool value)
    {
        Name = name;
        Value = value ? "true" : "false";
    }

    public string Name { get; }

    public string Value { get; }
}