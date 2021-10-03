using System.Text.Json;

namespace Amazon.Kms.Tests
{
    public class ListGrantsResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = @"{
    ""Grants"": [
        {
            ""KeyId"": ""arn:aws:kms:us-west-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab"",
            ""CreationDate"": 1572216195.0,
            ""GrantId"": ""abcde1237f76e4ba7987489ac329fbfba6ad343d6f7075dbd1ef191f0120514"",
            ""Constraints"": {
                ""EncryptionContextSubset"": {
                    ""Department"": ""IT""
                }
            },
            ""RetiringPrincipal"": ""arn:aws:iam::111122223333:role/adminRole"",
            ""Name"": """",
            ""IssuingAccount"": ""arn:aws:iam::111122223333:root"",
            ""GranteePrincipal"": ""arn:aws:iam::111122223333:user/exampleUser"",
            ""Operations"": [
                ""Decrypt""
            ]
        }
    ]
}";

            var result = JsonSerializer.Deserialize<ListGrantsResponse>(text);

            Assert.Single(result.Grants);

            Grant grant = result.Grants[0];

            Assert.Equal(1572216195.0, grant.CreationDate);

            Assert.Equal("arn:aws:kms:us-west-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab", grant.KeyId);
            Assert.Equal("abcde1237f76e4ba7987489ac329fbfba6ad343d6f7075dbd1ef191f0120514", grant.GrantId);
            Assert.Equal("IT", grant.Constraints.EncryptionContextSubset["Department"]);
            Assert.Equal(new[] { "Decrypt" }, grant.Operations);
        }


        [Fact]
        public void Parse2()
        {
            var text = @"{
  ""Grants"": [
    {
      ""CreationDate"": 1.477431461E9,
      ""GrantId"": ""91ad875e49b04a9d1f3bdeb84d821f9db6ea95e1098813f6d47f0c65fbe2a172"",
      ""GranteePrincipal"": ""acm.us-east-2.amazonaws.com"",
      ""IssuingAccount"": ""arn:aws:iam::111122223333:root"",
      ""KeyId"": ""arn:aws:kms:us-east-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab"",
      ""Name"": """",
      ""Operations"": [
        ""Encrypt"",
        ""ReEncryptFrom"",
        ""ReEncryptTo""
      ],
      ""RetiringPrincipal"": ""acm.us-east-2.amazonaws.com""
    },
    {
      ""CreationDate"": 1.477431461E9,
      ""GrantId"": ""a5d67d3e207a8fc1f4928749ee3e52eb0440493a8b9cf05bbfad91655b056200"",
      ""GranteePrincipal"": ""acm.us-east-2.amazonaws.com"",
      ""IssuingAccount"": ""arn:aws:iam::111122223333:root"",
      ""KeyId"": ""arn:aws:kms:us-east-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab"",
      ""Name"": """",
      ""Operations"": [
        ""ReEncryptFrom"",
        ""ReEncryptTo""
      ],
      ""RetiringPrincipal"": ""acm.us-east-2.amazonaws.com""
    },
    {
      ""CreationDate"": 1.477431461E9,
      ""GrantId"": ""c541aaf05d90cb78846a73b346fc43e65be28b7163129488c738e0c9e0628f4f"",
      ""GranteePrincipal"": ""acm.us-east-2.amazonaws.com"",
      ""IssuingAccount"": ""arn:aws:iam::111122223333:root"",
      ""KeyId"": ""arn:aws:kms:us-east-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab"",
      ""Name"": """",
      ""Operations"": [
        ""Encrypt"",
        ""ReEncryptFrom"",
        ""ReEncryptTo""
      ],
      ""RetiringPrincipal"": ""acm.us-east-2.amazonaws.com""
    },
    {
      ""CreationDate"": 1.477431461E9,
      ""GrantId"": ""dd2052c67b4c76ee45caf1dc6a1e2d24e8dc744a51b36ae2f067dc540ce0105c"",
      ""GranteePrincipal"": ""acm.us-east-2.amazonaws.com"",
      ""IssuingAccount"": ""arn:aws:iam::111122223333:root"",
      ""KeyId"": ""arn:aws:kms:us-east-2:111122223333:key/1234abcd-12ab-34cd-56ef-1234567890ab"",
      ""Name"": ""name"",
      ""Operations"": [
        ""Encrypt"",
        ""ReEncryptFrom"",
        ""ReEncryptTo""
      ],
      ""RetiringPrincipal"": ""acm.us-east-2.amazonaws.com""
    }
  ],
  ""Truncated"": false
}";

            var result = JsonSerializer.Deserialize<ListGrantsResponse>(text);

            Assert.Equal("name", result.Grants[3].Name);
            Assert.Equal("Encrypt,ReEncryptFrom,ReEncryptTo", string.Join(",", result.Grants[3].Operations));
            Assert.Equal("91ad875e49b04a9d1f3bdeb84d821f9db6ea95e1098813f6d47f0c65fbe2a172", result.Grants[0].GrantId);
            Assert.Equal(4, result.Grants.Count);
        }
    }
}