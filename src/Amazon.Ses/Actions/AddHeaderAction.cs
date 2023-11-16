namespace Amazon.Ses;

public sealed class AddHeaderAction(string headerName, string headerValue)
{
    public string HeaderName { get; } = headerName;

    public string HeaderValue { get; } = headerValue;
}