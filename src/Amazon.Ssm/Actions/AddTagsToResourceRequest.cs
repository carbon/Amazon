namespace Amazon.Ssm
{
    public class AddTagsToResourceRequest : ISsmRequest
    {
        public string ResourceId { get; set; }

        public string ResourceType { get; set; }

        public Tag[] Tags { get; set; }
    }
}