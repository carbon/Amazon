using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Data.Protection;

namespace Amazon.Kms
{
    public class KmsProtector : IDataProtector
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

        public async ValueTask<byte[]> EncryptAsync(
            byte[] plaintext, 
            IEnumerable<KeyValuePair<string, string>> context = null)
        { 
            #region Preconditions

            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (plaintext.Length > 1024 * 4)
            {
                throw new ArgumentException("Must be less than 4KB", nameof(plaintext));
            }

            #endregion
            
            var request = new EncryptRequest(keyId, plaintext, GetEncryptionContext(context));

            var result = await client.EncryptAsync(request).ConfigureAwait(false);

            return result.CiphertextBlob;
        }
        
        public async ValueTask<byte[]> DecryptAsync(
            byte[] ciphertext, 
            IEnumerable<KeyValuePair<string, string>> context = null)
        {
            var request = new DecryptRequest(keyId, ciphertext, GetEncryptionContext(context));

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
        
        public async Task RetireGrantAsync(string id)
        {
            await client.RetireGrantAsync(new RetireGrantRequest {
                GrantId = id,
                KeyId = keyId
            }).ConfigureAwait(false);
        }
        
        #region Helpers

        private static JsonObject GetEncryptionContext(
            IEnumerable<KeyValuePair<string, string>> authenticatedProperties)
        {
            if (authenticatedProperties == null) return null;

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

