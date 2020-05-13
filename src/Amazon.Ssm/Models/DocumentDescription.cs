#nullable disable

namespace Amazon.Ssm
{
    public sealed class DocumentDescription
    {
        public Timestamp CreateDate { get; set; }

        public string DefaultVersion { get; set; }

        public string Description { get; set; }

        public string DocumentType { get; set; }

        public string DocumentVersion { get; set; }

        public string Hash { get; set; }

        public string HashType { get; set; }

        public string LatestVersion { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public DocumentParameter[] Parameters { get; set; }

        public string[] PlatformTypes { get; set; }

        public string SchemaVersion { get; set; }

        public string Sha1 { get; set; }

        public string Status { get; set; }
    }
}

/*
{
   "DocumentDescription": { 
      "CreatedDate": number,
      "DefaultVersion": "string",
      "Description": "string",
      "DocumentType": "string",
      "DocumentVersion": "string",
      "Hash": "string",
      "HashType": "string",
      "LatestVersion": "string",
      "Name": "string",
      "Owner": "string",
      "Parameters": [ 
         { 
            "DefaultValue": "string",
            "Description": "string",
            "Name": "string",
            "Type": "string"
         }
      ],
      "PlatformTypes": [ "string" ],
      "SchemaVersion": "string",
      "Sha1": "string",
      "Status": "string"
   }
}
*/
