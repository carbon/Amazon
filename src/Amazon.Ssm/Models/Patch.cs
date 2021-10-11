#nullable disable

namespace Amazon.Ssm;

public sealed class Patch
{
    public string Id { get; set; }

    public string Classification { get; set; }

#nullable enable

    public string? ContentUrl { get; set; }

    public string? Description { get; set; }

    public string? KbNumber { get; set; }

    public string? Language { get; set; }

    public string? MsrcNumber { get; set; }

    public string? MsrcSeverity { get; set; }

    public string? Product { get; set; }

    public string? ProductFamily { get; set; }

    public Timestamp? ReleaseDate { get; set; }

    public string? Title { get; set; }

    public string? Vender { get; set; }
}
