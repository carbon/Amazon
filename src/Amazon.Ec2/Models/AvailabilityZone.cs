#nullable disable

namespace Amazon.Ec2;

public sealed class AvailabilityZone
{
    public string RegionName { get; init; }

    public string ZoneName { get; init; }

    /// <summary>
    ///  available | information | impaired | unavailable
    /// </summary>
    public string ZoneState { get; init; }

    /// <summary>
    /// availability-zone, local-zone, and wavelength-zone.
    /// </summary>
    public string ZoneType { get; init; }
}
