using System.Security.Cryptography;

using Amazon.Cryptography.Algorithms;

namespace Amazon.Cryptography.Tests;

public class AlgorithmSuiteTests
{
    // Obviously fake inputs. Expected values were computed independently with Python's hmac module (RFC 5869 extract + expand).
    private static readonly byte[] s_dataKey = Convert.FromHexString("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f");
    private static readonly byte[] s_messageId = Convert.FromHexString("a0a1a2a3a4a5a6a7a8a9aaabacadaeafb0b1b2b3b4b5b6b7b8b9babbbcbdbebf");

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void Get_ReturnsCommittingSuite_WithDocumentedParameters(AlgorithmSuiteId id)
    {
        var suite = AlgorithmSuite.Get(id);

        Assert.Equal(id, suite.AlgorithmId);
        Assert.Equal(2, suite.MessageFormatVersion);
        Assert.Equal(256, suite.DataKeyLength.BitCount);
        Assert.Equal(32, suite.AlgorithmSuiteDataLengthBytes);
        Assert.Equal(KeyDerivationAlgorithmType.HKDF_SHA512, suite.KeyDerivationAlgorithm);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_IV12_TAG16_HKDF_SHA384_ECDSA_P384)]
    [InlineData(AlgorithmSuiteId.AES_128_GCM_IV12_TAG16_NO_KDF)]
    [InlineData((AlgorithmSuiteId)0xFFFF)]
    public void Get_RejectsUnsupportedSuites(AlgorithmSuiteId id)
    {
        Assert.Throws<NotImplementedException>(() => AlgorithmSuite.Get(id));
    }

    [Fact]
    public void UnsignedSuite_HasNoSignatureAlgorithm()
    {
        Assert.Null(AlgorithmSuite.Get(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY).SignatureAlgorithm);
    }

    [Fact]
    public void SigningSuite_ReportsP384SignatureAlgorithm()
    {
        Assert.Equal(SignatureAlgorithmType.ECDSA_P384_SHA384, AlgorithmSuite.Get(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384).SignatureAlgorithm);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, "371fd7058a7d02303fd724d9c257c787ab372840ff64e7c22916701a08910ff6")]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, "d65475519b877e94ef7a69555e0a90de76307ac45b2d852b973ec8418d935e91")]
    public void DeriveKey_MatchesIndependentVector(AlgorithmSuiteId id, string expected)
    {
        var derivedKey = new byte[32];

        AlgorithmSuite.Get(id).DeriveKey(s_dataKey, s_messageId, derivedKey);

        Assert.Equal(expected, Convert.ToHexStringLower(derivedKey));
    }

    [Fact]
    public void CalculateCommitmentKey_MatchesIndependentVector_ForBothSuites()
    {
        const string expected = "8f0a89e98409b45c4215734e5b0b32f53bec97d19af9eb7807832f259dad7e87";

        var a = new byte[32];
        var b = new byte[32];

        AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default.CalculateCommitmentKey(s_dataKey, s_messageId, a);
        AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384.Default.CalculateCommitmentKey(s_dataKey, s_messageId, b);

        Assert.Equal(expected, Convert.ToHexStringLower(a));
        Assert.Equal(expected, Convert.ToHexStringLower(b));
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void DeriveKey_DependsOnDataKeyAndMessageId(AlgorithmSuiteId id)
    {
        var suite = AlgorithmSuite.Get(id);

        var baseline = new byte[32];
        var otherKey = new byte[32];
        var otherMessage = new byte[32];

        suite.DeriveKey(s_dataKey, s_messageId, baseline);
        suite.DeriveKey(Flip(s_dataKey), s_messageId, otherKey);
        suite.DeriveKey(s_dataKey, Flip(s_messageId), otherMessage);

        Assert.NotEqual(baseline, otherKey);
        Assert.NotEqual(baseline, otherMessage);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 31)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 33)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, 16)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, 0)]
    public void DeriveKey_RejectsWrongMessageIdLength(AlgorithmSuiteId id, int length)
    {
        Assert.Throws<ArgumentException>(() => AlgorithmSuite.Get(id).DeriveKey(s_dataKey, new byte[length], new byte[32]));
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY, 16)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, 64)]
    public void DeriveKey_RejectsWrongOutputLength(AlgorithmSuiteId id, int length)
    {
        var e = Assert.Throws<ArgumentException>(() => AlgorithmSuite.Get(id).DeriveKey(s_dataKey, s_messageId, new byte[length]));

        Assert.Equal("derivedKey", e.ParamName);
    }

    [Theory]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY)]
    [InlineData(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384)]
    public void DeriveKey_RejectsWrongDataKeyLength(AlgorithmSuiteId id)
    {
        Assert.ThrowsAny<Exception>(() => AlgorithmSuite.Get(id).DeriveKey(new byte[16], s_messageId, new byte[32]));
    }

    [Theory]
    [InlineData(31, 32, 32)]
    [InlineData(32, 31, 32)]
    [InlineData(32, 32, 16)]
    public void CalculateCommitmentKey_RejectsWrongLengths(int dataKey, int messageId, int output)
    {
        Assert.Throws<ArgumentException>(() => AES256_GCM_HKDF_SHA512_COMMIT_KEY.Default.CalculateCommitmentKey(new byte[dataKey], new byte[messageId], new byte[output]));
    }

    [Fact]
    public void BaseSuite_DoesNotImplementKeyDerivationByDefault()
    {
        var suite = new StubSuite();

        Assert.Equal(0, suite.AlgorithmSuiteDataLengthBytes);
        Assert.Null(suite.SignatureAlgorithm);
        Assert.Throws<NotImplementedException>(() => suite.DeriveKey(s_dataKey, s_messageId, new byte[32]));
    }

    private static byte[] Flip(byte[] value)
    {
        var copy = (byte[])value.Clone();

        copy[^1] ^= 1;

        return copy;
    }

    private sealed class StubSuite : AlgorithmSuite
    {
        public override AlgorithmSuiteId AlgorithmId => AlgorithmSuiteId.AES_128_GCM_IV12_TAG16_NO_KDF;

        public override byte MessageFormatVersion => 1;

        public override KeySize DataKeyLength => KeySize.FromByteCount(16);

        public override KeyDerivationAlgorithmType? KeyDerivationAlgorithm => null;
    }
}
