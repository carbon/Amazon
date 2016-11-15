namespace Amazon.Lambda
{
    public class InvokeResult
    {
        public InvokeResult(string responseText)
        {
            ResponseText = responseText;
        }

        public string ResponseText { get; }
    }
}