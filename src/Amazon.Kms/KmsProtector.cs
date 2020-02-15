using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amazon.Kms
{
    public sealed class KmsProtector
    {
        private readonly KmsClient client;
        private readonly string keyId;

        public KmsProtector(AwsRegion region, string keyId, IAwsCredential credential)
            : this(new KmsClient(region, credential), keyId) { }

        public KmsProtector(KmsClient client, string keyId)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.keyId  = keyId  ?? throw new ArgumentNullException(nameof(keyId));
        }

        public async Task<byte[]> EncryptAsync(
            byte[] plaintext, 
            IEnumerable<KeyValuePair<string, string>>? aad = null)
        {             
            var request = new EncryptRequest(keyId, plaintext, GetEncryptionContext(aad));

            EncryptResponse result = await client.EncryptAsync(request).ConfigureAwait(false);

            return result.CiphertextBlob;
        }
        
        public async Task<byte[]> DecryptAsync(
            byte[] ciphertext, 
            IEnumerable<KeyValuePair<string, string>>? aad = null)
        {
            var request = new DecryptRequest(keyId, ciphertext, GetEncryptionContext(aad));

            var result = await client.DecryptAsync(request).ConfigureAwait(false);

            return result.Plaintext;
        }

        public async Task<GenerateDataKeyResponse> GenerateKeyAsync(
            IEnumerable<KeyValuePair<string, string>>? context = null)
        {
            var result = await client.GenerateDataKeyAsync(new GenerateDataKeyRequest(
               keyId             : keyId,
               keySpec           : KeySpec.AES_256,
               encryptionContext : GetEncryptionContext(context)
           )).ConfigureAwait(false);

            return result;
        }
        
        public async Task RetireGrantAsync(string grantId)
        {
            await client.RetireGrantAsync(new RetireGrantRequest(
                keyId   : keyId,
                grantId : grantId
            )).ConfigureAwait(false);
        }
        
        #region Helpers

        private static IReadOnlyDictionary<string, string>? GetEncryptionContext(IEnumerable<KeyValuePair<string, string>>? aad)
        {
            if (aad is null) return null;

            if (aad is IReadOnlyDictionary<string, string> aadRod)
            {
                return aadRod;
            }

            var json = new Dictionary<string, string>();

            foreach (var pair in aad)
            {
                json.Add(pair.Key, pair.Value);
            }

            return json;
        }

        #endregion
    }
}
