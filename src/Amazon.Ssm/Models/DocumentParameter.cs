#nullable disable

namespace Amazon.Ssm;

public sealed class DocumentParameter
{
    public string DefaultValue { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    // String | StringList
    public string Type { get; set; }
}
