using System;

namespace Amazon.Ssm
{
    public class DocumentVersionInfo
    {
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public string DocumentVersion { get; set; }

        public bool IsDefaultVersion { get; set; }
    }
}
