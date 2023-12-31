using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Results.Tests;

public class DecryptResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        DecryptResult? result = JsonSerializer.Deserialize(
            """
            {
              "KeyId": "arn:aws:kms:us-west-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab",
              "Plaintext": "VGhpcyBpcyBEYXkgMSBmb3IgdGhlIEludGVybmV0Cg==",
              "EncryptionAlgorithm": "SYMMETRIC_DEFAULT" 
            }
            """u8, KmsSerializerContext.Default.DecryptResult);

        Assert.NotNull(result);
        Assert.StartsWith("arn:", result.KeyId);
        Assert.Equal(EncryptionAlgorithm.SYMMETRIC_DEFAULT, result.EncryptionAlgorithm);
    }
}

// Test data from: https://docs.aws.amazon.com/kms/latest/APIReference/API_Decrypt.html