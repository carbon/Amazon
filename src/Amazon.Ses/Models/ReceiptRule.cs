using System.ComponentModel.DataAnnotations;

namespace Amazon.Ses;

public sealed class ReceiptRule
{
    public ReceiptRule() { }

    public ReceiptRule(string name)
    {
        Name = name;
    }

    public ReceiptAction[]? Actions { get; init; }

    [StringLength(64)]
    public required string Name { get; init; }

    public bool Enabled { get; init; }

    public string[]? Recipients { get; init; }

    public bool? ScanEnabled { get; init; }

    // Require | Optional
    public string? TlsPolicy { get; init; }
}