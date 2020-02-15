#nullable disable

namespace Amazon.Kms
{
    public sealed class Grant
    {
        public string GrantId { get; set; }

        public GrantConstraints Constraints { get; set; }

        public string GranteePrincipal { get; set; }

        public string IssuingAccount { get; set; }

        public string Name { get; set; }

        public string[] Operations { get; set; }

        public string RetiringPrincipal { get; set; }

        // UnixTime seconds
        public double CreationDate { get; set; }
    }
}

/*
{
 "Constraints": { 
    "EncryptionContextEquals": { 
        "string" : "string" 
    },
    "EncryptionContextSubset": { 
        "string" : "string" 
    }
    },
    "CreationDate": number,
    "GranteePrincipal": "string",
    "GrantId": "string",
    "IssuingAccount": "string",
    "KeyId": "string",
    "Name": "string",
    "Operations": [ "string" ],
    "RetiringPrincipal": "string"
}
*/
