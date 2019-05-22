#nullable disable


using System;

namespace Amazon.Route53
{
    public class ChangeInfo
    {
        public string Comment { get; set; }

        public string Id { get; set; }

        public string Status { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
