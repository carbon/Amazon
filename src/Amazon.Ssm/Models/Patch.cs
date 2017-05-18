using System;

namespace Amazon.Ssm
{
    public class Patch
    {
        public string Classification { get; set; }

        public string ContentUrl { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        public string KbNumber { get; set; }

        public string Language { get; set; }

        public string MsrcNumber { get; set; }

        public string MsrcSeverity { get; set; }

        public string Product { get; set; }

        public string ProductFamily { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string Title { get; set; }

        public string Vender { get; set; }
    }
}