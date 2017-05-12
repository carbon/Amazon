namespace Amazon.Elb
{
    public class AddTagsRequest : IElbRequest
    {
        public string Action => "AddTags";

        public string[] ResponseArns { get; set; }

        public Tag[] Tags { get; set; }
    }

}
