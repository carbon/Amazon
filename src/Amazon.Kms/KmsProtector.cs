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
            this.keyId = keyId ?? throw new ArgumentNullException(nameof(keyId));
        }

        public async Task<byte[]> EncryptAsync(byte[] data, IDictionary<string, string> context = null)
        { 
            #region Preconditions

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length > 1024 * 4)
            {
                throw new ArgumentException("Must be less than 4KB", nameof(data));
            }

            #endregion
            
            var request = new EncryptRequest(keyId, data, context != null ? new JsonObject(context) : null);

            var result = await client.EncryptAsync(request).ConfigureAwait(false);

            return result.CiphertextBlob;
        }
        
        public async Task<byte[]> DecryptAsync(byte[] ciphertextBlob, IDictionary<string, string> context = null)
        {
            var request = new DecryptRequest(keyId, ciphertextBlob, context != null ? new JsonObject(context) : null);

            var result = await client.DecryptAsync(request).ConfigureAwait(false);

            return result.Plaintext;
        }
    }
}

