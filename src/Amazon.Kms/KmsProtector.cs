namespace Amazon.Kms;

public sealed class KmsProtector
{
    private readonly KmsClient _client;
    private readonly string _keyId;

    public KmsProtector(AwsRegion region, string keyId, IAwsCredential credential)
        : this(new KmsClient(region, credential), keyId) { }

    public KmsProtector(KmsClient client, string keyId)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(keyId);

        _client = client;
        _keyId = keyId;
    }

    public async Task<byte[]> EncryptAsync(
        byte[] plaintext,
        IEnumerable<KeyValuePair<string, string>>? aad = null)
    {
        var request = new EncryptRequest(_keyId, plaintext, GetEncryptionContext(aad));

        EncryptResponse result = await _client.EncryptAsync(request).ConfigureAwait(false);

        return result.CiphertextBlob;
    }

    public async Task<byte[]> DecryptAsync(
        byte[] ciphertext,
        IEnumerable<KeyValuePair<string, string>>? aad = null)
    {
        var request = new DecryptRequest(_keyId, ciphertext, GetEncryptionContext(aad));

        var result = await _client.DecryptAsync(request).ConfigureAwait(false);

        return result.Plaintext;
    }

    public async Task<GenerateDataKeyResponse> GenerateKeyAsync(
        IEnumerable<KeyValuePair<string, string>>? context = null)
    {
        var result = await _client.GenerateDataKeyAsync(new GenerateDataKeyRequest(
           keyId: _keyId,
           keySpec: KeySpec.AES_256,
           encryptionContext: GetEncryptionContext(context)
       )).ConfigureAwait(false);

        return result;
    }

    public async Task RetireGrantAsync(string grantId)
    {
        await _client.RetireGrantAsync(new RetireGrantRequest(
            keyId   : _keyId,
            grantId : grantId
        )).ConfigureAwait(false);
    }

    #region Helpers

    private static IReadOnlyDictionary<string, string>? GetEncryptionContext(IEnumerable<KeyValuePair<string, string>>? aad)
    {
        if (aad is null) return null;

        if (aad is IReadOnlyDictionary<string, string> aadReadOnlyDictionary)
        {
            return aadReadOnlyDictionary;
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
