namespace Amazon.Ssm
{
    public class DeregisterPatchBaselineForPatchGroupRequest
    {
        public string BaselineId { get; set; }

        public string PatchGroup { get; set; }
    }
}