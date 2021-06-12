namespace Amazon.Translate
{
    public sealed class TranslationClient : AwsClient
    {
        private const string target = "AWSShineFrontendService_20170701";

        public TranslationClient(IAwsCredential credential)
            : base(AwsService.Translate, AwsRegion.USEast1, credential) { }


        // + Operation
        // TranslateText

    }
}
