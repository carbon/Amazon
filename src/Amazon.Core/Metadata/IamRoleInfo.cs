using System;
using Amazon.Helpers;

namespace Amazon
{
    internal class IamRoleInfo
    {
        public string Code { get; set; }

        public DateTime LastUpdated { get; set; }

        public string InstanceProfileArn { get; set; }

        public string InstanceProfileId { get; set; }

        public string ProfileName => InstanceProfileArn.Split(Seperators.ForwardSlash)[1];
    }
}