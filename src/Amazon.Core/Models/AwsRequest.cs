using System.Collections;
using System.Globalization;

namespace Amazon;

public class AwsRequest : IEnumerable<KeyValuePair<string, string>>
{
    public AwsRequest()
    {
        Parameters = new Dictionary<string, string>();
    }

    public AwsRequest(KeyValuePair<string, string>[] parameters)
    {
        Parameters = new Dictionary<string, string>(parameters.Length);

        foreach (var parameter in parameters)
        {
            Parameters.Add(parameter.Key, parameter.Value);
        }
    }

    public Dictionary<string, string> Parameters { get; }

    public void Add(string name, string value)
    {
        Parameters.Add(name, value);
    }

    public void Add(string name, int value)
    {
        Parameters.Add(name, value.ToString(CultureInfo.InvariantCulture));
    }

    // IEnumerable

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => Parameters.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();
}
