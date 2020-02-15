namespace Amazon.Ssm
{
    public sealed class AddTagsToResourceRequest : ISsmRequest
    {
        public string ResourceId { get; set; }

        public string ResourceType { get; set; }

        public Tag[] Tags { get; set; }
    }
}