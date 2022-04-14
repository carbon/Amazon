using System.ComponentModel.DataAnnotations;

namespace Amazon.Ses;

public sealed class ReceiptRule
{
    public ReceiptRule(string name!!)
    {
        Name = name;
    }

    public ReceiptAction[]? Actions { get; set; }

    [Required, StringLength(64)]
    public string Name { get; }

    public bool Enabled { get; set; }

    public string[]? Recipients { get; set; }

    public bool? ScanEnabled { get; set; }

    // Require | Optional
    public string? TlsPolicy { get; set; }
}