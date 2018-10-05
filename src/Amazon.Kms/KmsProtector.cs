using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Kms
{
    public class KmsProtector
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
            IEnumerable<KeyValuePair<string, string>> aad = null)
        { 
            if (plaintext is null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (plaintext.Length > 1024 * 4)
            {
                throw new ArgumentException("Must be less than 4KB", nameof(plaintext));
            }
            
            var request = new EncryptRequest(keyId, plaintext, GetEncryptionContext(aad));

            var result = await client.EncryptAsync(request).ConfigureAwait(false);

            return result.CiphertextBlob;
        }
        
        public async Task<byte[]> DecryptAsync(
            byte[] ciphertext, 
            IEnumerable<KeyValuePair<string, string>> aad = null)
        {
            var request = new DecryptRequest(keyId, ciphertext, GetEncryptionContext(aad));

            var result = await client.DecryptAsync(request).ConfigureAwait(false);

            return result.Plaintext;
        }

        public async Task<GenerateDataKeyResponse> GenerateKeyAsync(
            IEnumerable<KeyValuePair<string, string>> context = null)
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
            if (grantId is null)
                throw new ArgumentNullException(nameof(grantId));

            await client.RetireGrantAsync(new RetireGrantRequest {
                GrantId = grantId,
                KeyId = keyId
            }).ConfigureAwait(false);
        }
        
        #region Helpers

        private static JsonObject GetEncryptionContext(
            IEnumerable<KeyValuePair<string, string>> authenticatedProperties)
        {
            if (authenticatedProperties is null) return null;

            var json = new JsonObject();

            foreach (var pair in authenticatedProperties)
            {
                json.Add(pair.Key, pair.Value);
            }

            return json;
        }

        #endregion
    }
}
