namespace Amazon.Ssm;

public sealed class DeletePatchBaselineRequest
{
    public DeletePatchBaselineRequest(string baselineId)
    {
        BaselineId = baselineId;
    }

    public string BaselineId { get; }
}
