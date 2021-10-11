#nullable disable

namespace Amazon.Ssm;

public sealed class DeregisterPatchBaselineForPatchGroupRequest
{
    public string BaselineId { get; init; }

    public string PatchGroup { get; init; }
}
