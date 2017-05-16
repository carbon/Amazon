namespace Amazon.Ssm
{
    public class GetParametersRequest : ISsmRequest
    {
        public string[] Names { get; set; }

        public bool? WithDecryption { get; set; }
    }
}