using System.Collections;
using System.Globalization;

namespace Amazon;

public class AwsRequest : IEnumerable<KeyValuePair<string, string>>
{
    public AwsRequest()
    {
        Parameters = new List<KeyValuePair<string, string>>(4);
    }

    public AwsRequest(ReadOnlySpan<KeyValuePair<string, string>> parameters)
    {
        Parameters = [.. parameters];
    }

    public List<KeyValuePair<string, string>> Parameters { get; }

    public void Add(string name, string value)
    {
        Parameters.Add(new (name, value));
    }

    public void Add(KeyValuePair<string, string> item)
    {
        Parameters.Add(item);
    }

    public void Add(string name, int value)
    {
        Parameters.Add(new (name, value.ToString(CultureInfo.InvariantCulture)));
    }

    public void AddRange(ReadOnlySpan<KeyValuePair<string, string>> values)
    {
        Parameters.AddRange(values);
    }
   
    // IEnumerable

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => Parameters.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();
}
