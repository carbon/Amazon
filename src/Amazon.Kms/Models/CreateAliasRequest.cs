namespace Amazon.Kms
{
    public class CreateAliasRequest : KmsRequest
    {
        public string TargetKeyId { get; set; }

        public string AliasName { get; set; }
    }    
}

/*
{
  "TargetKeyId": "1234abcd-12ab-34cd-56ef-1234567890ab",
  "AliasName": "alias/ExampleAlias"
}
*/
