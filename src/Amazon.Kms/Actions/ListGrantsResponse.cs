#nullable disable

using System.Collections.Generic;

namespace Amazon.Kms
{
    public sealed class ListGrantsResponse : KmsResponse
    {
        public string NextMarker { get; set; }

        public bool Truncated { get; set; }

        public List<Grant> Grants { get; set; }
    }
}