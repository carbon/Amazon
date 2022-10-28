﻿using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;

using Amazon.Kms;
using Amazon.S3;

namespace Amazon.Ses;

public sealed class EmailMessageDecryptor
{
    private readonly KmsClient _kms;

    public EmailMessageDecryptor(KmsClient kms)
    {
        _kms = kms;
    }

    public async Task<byte[]> DecryptAsync(S3Object encryptedBlob)
    {
        ArgumentNullException.ThrowIfNull(encryptedBlob);

        var cekAlg = encryptedBlob.Properties["x-amz-meta-x-amz-cek-alg"];

        if (cekAlg is not "AES/GCM/NoPadding")
        {
            throw new Exception($"Expected AES/GCM/NoPadding. Was '{cekAlg}'");
        }

        var encryptionContext = JsonSerializer.Deserialize<Dictionary<string, string>>(encryptedBlob.Properties["x-amz-meta-x-amz-matdesc"])!;

        byte[] envelopeKey = Convert.FromBase64String(encryptedBlob.Properties["x-amz-meta-x-amz-key-v2"]);
        byte[] envelopeIV  = Convert.FromBase64String(encryptedBlob.Properties["x-amz-meta-x-amz-iv"]);
        long contentLength = long.Parse(encryptedBlob.Properties["x-amz-meta-x-amz-unencrypted-content-length"], NumberStyles.None, CultureInfo.InvariantCulture);
        int tagLength      = int.Parse(encryptedBlob.Properties["x-amz-meta-x-amz-tag-len"], NumberStyles.None, CultureInfo.InvariantCulture);

        if (tagLength != 128)
        {
            throw new Exception($"x-amz-tag-len must be 128 bits. Was {tagLength}");
        }

        if (contentLength > 10_000_000)
        {
            throw new Exception("> 10MB");
        }

        string kmsCmkId = encryptionContext["kms_cmk_id"];

        byte[] blobBytes = await encryptedBlob.ReadAsByteArrayAsync().ConfigureAwait(false);

        var tag        = blobBytes[^16..];
        var ciphertext = blobBytes[0..^16];

        var decryptResult = await _kms.DecryptAsync(new DecryptRequest(kmsCmkId, envelopeKey, encryptionContext));
       
        using var aes = new AesGcm(decryptResult.Plaintext);

        var message = new byte[contentLength];

        aes.Decrypt(envelopeIV, ciphertext: ciphertext, tag: tag, plaintext: message);

        return message;
    }
}