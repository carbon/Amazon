#nullable disable

using Carbon.Json;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class TimeToLiveSpecification
    {
        public TimeToLiveSpecification() { }
        public TimeToLiveSpecification(string attributeName, bool enabled)
        {
            AttributeName = attributeName;
            Enabled = enabled;
        }

        public string AttributeName { get; set; }
        public bool Enabled { get; set; }
    }
}
