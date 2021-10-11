#nullable disable

namespace Amazon.Ssm;

public sealed class RegisterPatchBaselineForPatchGroupRequest
{
    public string BaselineId { get; set; }

    public string PathGroup { get; set; }
}