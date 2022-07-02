#nullable disable

namespace Amazon.Ec2;

public sealed class UserData
{
    public UserData() { }

    public UserData(string data)
    {
        Data = data;
    }

    public string Data { get; set; }
}