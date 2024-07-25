using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

[JsonConverter(typeof(JsonStringEnumConverter<PiiEntityType>))]
public enum PiiEntityType
{
    BANK_ACCOUNT_NUMBER,
    BANK_ROUTING,
    CREDIT_DEBIT_NUMBER,
    CREDIT_DEBIT_CVV,
    CREDIT_DEBIT_EXPIRY,
    PIN,
    EMAIL,
    ADDRESS,
    NAME,
    PHONE,
    SSN,
    ALL
}
