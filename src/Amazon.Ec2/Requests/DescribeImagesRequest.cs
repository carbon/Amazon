using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeImagesRequest : DescribeRequest, IEc2Request
    {        
        public List<string> ImageIds { get; } = new List<string>();

        public List<string> OwnerIds { get; } = new List<string>();

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeImages");

            AddIds(parameters, "ImageId", ImageIds);

            AddIds(parameters, "OwnerId", ImageIds);


            return parameters;
        }
    }
}