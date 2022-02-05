namespace Amazon.Ses;

public sealed class AddHeaderAction
{
    public AddHeaderAction(string headerName, string headerValue)
    {
        HeaderName = headerName;
        HeaderValue = headerValue;
    }

    public string HeaderName { get; }

    public string HeaderValue { get; }
}
