using System;

namespace Amazon.Kms
{
    public sealed class CreateAliasRequest : KmsRequest
    {
        public CreateAliasRequest(string targetKeyId, string aliasName)
        {
            TargetKeyId = targetKeyId ?? throw new ArgumentNullException(nameof(targetKeyId));
            AliasName   = aliasName ?? throw new ArgumentNullException(nameof(aliasName));
        }

        public string TargetKeyId { get; }

        public string AliasName { get; }
    }    
}