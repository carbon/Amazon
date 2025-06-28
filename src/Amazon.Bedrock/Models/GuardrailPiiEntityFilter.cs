namespace Amazon.Bedrock.Models;
using System.Text.Json.Serialization;

public class GuardrailPiiEntityFilter
{
    // ANONYMIZED | BLOCKED
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("match")]
    public required string Match { get; init; }

    // | ADDRESS
    // | AGE
    // | AWS_ACCESS_KEY
    // | AWS_SECRET_KEY
    // | CA_HEALTH_NUMBER
    // | CA_SOCIAL_INSURANCE_NUMBER
    // | CREDIT_DEBIT_CARD_CVV
    // | CREDIT_DEBIT_CARD_EXPIRY
    // | CREDIT_DEBIT_CARD_NUMBER
    // | DRIVER_ID
    // | EMAIL
    // | INTERNATIONAL_BANK_ACCOUNT_NUMBER
    // | IP_ADDRESS
    // | LICENSE_PLATE
    // | MAC_ADDRESS
    // | NAME
    // | PASSWORD
    // | PHONE
    // | PIN
    // | SWIFT_CODE
    // | UK_NATIONAL_HEALTH_SERVICE_NUMBER
    // | UK_NATIONAL_INSURANCE_NUMBER
    // | UK_UNIQUE_TAXPAYER_REFERENCE_NUMBER
    // | URL
    // | USERNAME
    // | US_BANK_ACCOUNT_NUMBER
    // | US_BANK_ROUTING_NUMBER |
    // | US_INDIVIDUAL_TAX_IDENTIFICATION_NUMBER
    // | US_PASSPORT_NUMBER
    // | US_SOCIAL_SECURITY_NUMBER
    // | VEHICLE_IDENTIFICATION_NUMBER
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}
