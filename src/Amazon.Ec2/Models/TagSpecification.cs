using System.Runtime.Serialization;

namespace Amazon.Ec2
{
    public class TagSpecification
    {
        public TagSpecification() { }

        public TagSpecification(string resourceType, params Tag[] tags)
        {
            ResourceType = resourceType;
            Tags = tags;
        }

        // instance and volume.
        public string ResourceType { get; set; }

        [DataMember(Name = "Tag")]
        public Tag[] Tags { get; set; }
    }
}
