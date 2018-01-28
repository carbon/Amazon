using System;

namespace Amazon.Elb
{
    public class AddTagsRequest : IElbRequest
    {
        public AddTagsRequest() { }

        public AddTagsRequest(string[] resourceArns, Tag[] tags)
        {
            ResourceArns = resourceArns ?? throw new ArgumentNullException(nameof(resourceArns));
            Tags         = tags ?? throw new ArgumentNullException(nameof(tags));
        }

        public string Action => "AddTags";

        public string[] ResourceArns { get; set; }

        public Tag[] Tags { get; set; }
    }

}
