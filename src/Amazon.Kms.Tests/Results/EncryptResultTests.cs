using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Results.Tests;

public class EncryptResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        EncryptResult? result = JsonSerializer.Deserialize(
            """
            {
              "CiphertextBlob": "CiDPoCH188S65r5Cy7pAhIFJMXDlU7mewhSlYUpuQIVBrhKmAQEBAgB4z6Ah9fPEuua+Qsu6QISBSTFw5VO5nsIUpWFKbkCFQa4AAAB9MHsGCSqGSIb3DQEHBqBuMGwCAQAwZwYJKoZIhvcNAQcBMB4GCWCGSAFlAwQBLjARBAxLc9b6QThC9jB/ZjYCARCAOt8la8qXLO5wB3JH2NlwWWzWRU2RKqpO9A/0psE5UWwkK6CnwoeC3Zj9Q0A66apZkbRglFfY1lTY+Tc=",
              "KeyId": "arn:aws:kms:us-east-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab",
              "EncryptionAlgorithm": "SYMMETRIC_DEFAULT"
            }
            """u8, KmsSerializerContext.Default.EncryptResult);

        Assert.NotNull(result);
        Assert.StartsWith("arn:", result.KeyId);
        Assert.Equal(EncryptionAlgorithm.SYMMETRIC_DEFAULT, result.EncryptionAlgorithm);
    }
}

// Test data from: https://docs.aws.amazon.com/kms/latest/APIReference/API_Encrypt.html