#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class Grant
{
    public string GrantId { get; init; }

#nullable enable
    public string? KeyId { get; init; }
#nullable disable

    public GrantConstraints Constraints { get; init; }

    public string GranteePrincipal { get; init; }

    public string IssuingAccount { get; init; }

    public string Name { get; init; }

    [JsonPropertyName("Operations")]
    public string[] Operations { get; init; }

    // UnixTime seconds
    public double CreationDate { get; init; }

    public string RetiringPrincipal { get; init; }
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
