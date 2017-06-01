namespace Amazon.Kms
{
    public class CreateAliasRequest : KmsRequest
    {
        public CreateAliasRequest() { }

        public CreateAliasRequest(string targetKeyId, string aliasName)
        {
            TargetKeyId = targetKeyId;
            AliasName   = aliasName;
        }

        public string TargetKeyId { get; set; }

        public string AliasName { get; set; }
    }    
}