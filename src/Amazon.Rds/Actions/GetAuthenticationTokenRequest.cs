namespace Amazon.Rds;

public sealed class GetAuthenticationTokenRequest
{
    public GetAuthenticationTokenRequest(string hostname, int port, string userName)
        : this(hostname, port, userName, TimeSpan.FromMinutes(15)) { }

    public GetAuthenticationTokenRequest(string hostname, int port, string userName, TimeSpan expires)
    {
        ArgumentException.ThrowIfNullOrEmpty(hostname);
        ArgumentException.ThrowIfNullOrEmpty(userName);

        if (expires <= TimeSpan.Zero || expires.Ticks % TimeSpan.TicksPerSecond != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expires), expires, "Must be a positive, whole number of seconds");
        }

        HostName = hostname;
        Port = port;
        UserName = userName;
        Expires = expires;
    }

    public string HostName { get; }

    public int Port { get; }

    public string UserName { get; }

    public TimeSpan Expires { get; }
}