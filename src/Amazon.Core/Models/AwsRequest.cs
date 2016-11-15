using System.Collections;
using System.Collections.Generic;

namespace Amazon
{
    public class AwsRequest : IEnumerable
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

        #region IEnumerator

        IEnumerator IEnumerable.GetEnumerator()
            => Parameters.GetEnumerator();

        #endregion
    }
}