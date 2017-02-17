using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Json;

namespace Amazon.Kms
{
    // Implement IAsyncProtector ?

    public class KmsProtector
    {
        private readonly KmsClient client;
        private readonly string keyId;

        public KmsProtector(IAwsCredentials credentials, string keyId)
            : this(AwsRegion.Standard, credentials, keyId) { }

        public KmsProtector(AwsRegion region, IAwsCredentials credentials, string keyId)
        {
            #region Preconditions

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            if (keyId == null)
                throw new ArgumentNullException(nameof(keyId));

            #endregion

            this.client = new KmsClient(region, credentials);
            this.keyId = keyId;
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

