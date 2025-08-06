using System.Buffers;

namespace Amazon.Cryptography.Tests;

public class EncryptedMessageTests
{
    [Fact]
    public void CanDeserialize()
    {
        // via https://docs.aws.amazon.com/encryption-sdk/latest/developer-guide/message-format-examples.html

        var messageBytes = TestHelper.GetBytes(
            """
            +--------+
            | Header |
            +--------+
            02                                         Version (2.0)
            0578                                       Algorithm ID (see Algorithms reference)
            122747eb 21dfe39b 38631c61 7fad7340
            cc621a30 32a11cc3 216d0204 fd148459        Message ID (random 256-bit value)
            008e                                       AAD Length (142)
            0004                                       AAD Key-Value Pair Count (4)
            0005                                       AAD Key-Value Pair 1, Key Length (5)
            30546869 73                                AAD Key-Value Pair 1, Key ("0This")
            0002                                       AAD Key-Value Pair 1, Value Length (2)
            6973                                       AAD Key-Value Pair 1, Value ("is")
            0003                                       AAD Key-Value Pair 2, Key Length (3)
            31616e                                     AAD Key-Value Pair 2, Key ("1an")
            000a                                       AAD Key-Value Pair 2, Value Length (10)
            656e6372 79707469 6f6e                     AAD Key-Value Pair 2, Value ("encryption")
            0008                                       AAD Key-Value Pair 3, Key Length (8)
            32636f6e 74657874                          AAD Key-Value Pair 3, Key ("2context")
            0007                                       AAD Key-Value Pair 3, Value Length (7)
            6578616d 706c65                            AAD Key-Value Pair 3, Value ("example")
            0015                                       AAD Key-Value Pair 4, Key Length (21)
            6177732d 63727970 746f2d70 75626c69        AAD Key-Value Pair 4, Key ("aws-crypto-public-key")
            632d6b65 79
            0044                                       AAD Key-Value Pair 4, Value Length (68)
            41746733 72703845 41345161 36706669        AAD Key-Value Pair 4, Value ("QXRnM3JwOEVBNFFhNnBmaTk3MUlTNTk3NHpOMnlZWE5vSmtwRHFPc0dIYkVaVDRqME5OMlFkRStmbTFVY01WdThnPT0=")
            39373149 53353937 347a4e32 7959584e
            6f4a6b70 44714f73 47486245 5a54346a
            304e4e32 5164452b 666d3155 634d5675
            38673d3d
            0001                                       Encrypted Data Key Count (1)
            0007                                       Encrypted Data Key 1, Key Provider ID Length (7)
            6177732d 6b6d73                            Encrypted Data Key 1, Key Provider ID ("aws-kms")
            004b                                       Encrypted Data Key 1, Key Provider Information Length (75)
            61726e3a 6177733a 6b6d733a 75732d77        Encrypted Data Key 1, Key Provider Information ("arn:aws:kms:us-west-2:658956600833:key/b3537ef1-d8dc-4780-9f5a-55776cbb2f7f")
            6573742d 323a3635 38393536 36303038
            33333a6b 65792f62 33353337 6566312d
            64386463 2d343738 302d3966 35612d35
            35373736 63626232 663766
            00a7                                       Encrypted Data Key 1, Encrypted Data Key Length (167)
            01010100 7840f38c 275e3109 7416c107        Encrypted Data Key 1, Encrypted Data Key
            29515057 1964ada3 ef1c21e9 4c8ba0bd
            bc9d0fb4 14000000 7e307c06 092a8648
            86f70d01 0706a06f 306d0201 00306806
            092a8648 86f70d01 0701301e 06096086
            48016503 04012e30 11040c39 32d75294
            06063803 f8460802 0110803b 2a46bc23
            413196d2 903bf1d7 3ed98fc8 a94ac6ed
            e00ee216 74ec1349 12777577 7fa052a5
            ba62e9e4 f2ac8df6 bcb1758f 2ce0fb21
            cc9ee5c9 7203bb
            02                                         Content Type (2, framed data)
            00001000                                   Frame Length (4096)
            05cd035b 29d5499d 4587570b 87502afe        Algorithm Suite Data (key commitment)
            634f7b2c c3df2aa9 88a10105 4a2c7687 
            76cb339f 2536741f 59a1c202 4f2594ab        Authentication Tag
            +------+
            | Body |
            +------+
            ffffffff                                   Final Frame, Sequence Number End
            00000001                                   Final Frame, Sequence Number (1)
            00000000 00000000 00000001                 Final Frame, IV
            00000009                                   Final Frame, Encrypted Content Length (9)
            fa6e39c6 02927399 3e                       Final Frame, Encrypted Content
            f683a564 405d68db eeb0656c d57c9eb0        Final Frame, Authentication Tag
            +--------+
            | Footer |
            +--------+
            0067                                       Signature Length (103)
            30650230 2a1647ad 98867925 c1712e8f        Signature 
            ade70b3f 2a2bc3b8 50eb91ef 56cfdd18 
            967d91d8 42d92baf 357bba48 f636c7a0
            869cade2 023100aa ae12d08f 8a0afe85
            e5054803 110c9ed8 11b2e08a c4a052a9
            074217ea 3b01b660 534ac921 bf091d12
            3657e2b0 9368bd  
            """);

        var message = EncryptedMessage.Parse(messageBytes);

        Assert.Equal(AlgorithmSuiteId.AES_256_GCM_HKDF_SHA512_COMMIT_KEY_ECDSA_P384, message.Header.AlgorithmId);

        Assert.Equal(4, message.Header.EncryptionContext.Count);

        Assert.Equal(2, message.Header.ContentType); // framed
        Assert.Equal(4096u, message.Header.FrameLength);

        Assert.Equal("EidH6yHf45s4Yxxhf61zQMxiGjAyoRzDIW0CBP0UhFk=", Convert.ToBase64String(message.Header.MessageId));

        Assert.Single(message.Header.EncryptedDataKeys);

        Assert.Equal("aws-kms", message.Header.EncryptedDataKeys[0].ProviderId);
        Assert.Equal("arn:aws:kms:us-west-2:658956600833:key/b3537ef1-d8dc-4780-9f5a-55776cbb2f7f", message.Header.EncryptedDataKeys[0].ProviderInfo);

        Assert.Single(message.Frames);

        Assert.Equal("Bc0DWynVSZ1Fh1cLh1Aq/mNPeyzD3yqpiKEBBUosdoc=", Convert.ToBase64String(message.Header.AlgorithmSuiteData!));
        Assert.Equal("dssznyU2dB9ZocICTyWUqw==", Convert.ToBase64String(message.Header.AuthenticationTag!));

        Assert.NotNull(message.Signature);
        Assert.Equal("MGUCMCoWR62YhnklwXEuj63nCz8qK8O4UOuR71bP3RiWfZHYQtkrrzV7ukj2Nseghpyt4gIxAKquEtCPigr+heUFSAMRDJ7YEbLgisSgUqkHQhfqOwG2YFNKySG/CR0SNlfisJNovQ==", Convert.ToBase64String(message.Signature));

        var writer = new ArrayBufferWriter<byte>();

        message.WriteTo(writer);

        Assert.Equal(messageBytes.AsSpan(), writer.WrittenSpan);
    }
}