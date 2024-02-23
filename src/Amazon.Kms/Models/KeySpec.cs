using System.Text.Json.Serialization;

namespace Amazon.Kms;

[JsonConverter(typeof(JsonStringEnumConverter<KeySpec>))]
public enum KeySpec
{
    AES_256 = 1,
    AES_128 = 2,

    RSA_2048 = 3,
    RSA_3072 = 4,
    RSA_4096 = 5,
    ECC_NIST_P256 = 6,
    ECC_NIST_P384 = 7,
    ECC_NIST_P521 = 8,
    ECC_SECG_P256K1 = 9,
    SYMMETRIC_DEFAULT = 10,
    HMAC_224 = 11,
    HMAC_256 = 12,
    HMAC_384 = 13,
    HMAC_512 = 14,
    SM2 = 15
}