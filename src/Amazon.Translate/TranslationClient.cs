namespace Amazon.Translate;

public sealed class TranslationClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Translate, region, credential)
{
    private const string target = "AWSShineFrontendService_20170701";


    // + Operation
    // TranslateText
}