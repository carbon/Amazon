using System.Collections;
using System.Collections.Generic;

namespace Amazon
{
    public class AwsRequest : IEnumerable<KeyValuePair<string, string>>
    {
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public void Add(string name, string value)
        {
            Parameters.Add(name, value);
        }

        public void Add(string name, int value)
        {
            Parameters.Add(name, value.ToString());
        }

        #region IEnumerable

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();

        #endregion
    }
}