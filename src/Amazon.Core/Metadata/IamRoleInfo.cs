#pragma warning disable IDE0057 // Use range operator

#nullable disable

using System;

namespace Amazon.Metadata
{
    internal sealed class IamRoleInfo
    {
        public string Code { get; init; }

        public DateTime LastUpdated { get; init; }

        public string InstanceProfileArn { get; init; }

        public string InstanceProfileId { get; init; }

        public string ProfileName => InstanceProfileArn.Substring(InstanceProfileArn.IndexOf('/') + 1);
    }
}
