using System.Runtime.Serialization;

namespace Amazon.Ssm
{
    public class ListDocumentsRequest : ISsmRequest
    {
        public DocumentFilter[] DocumentFilterList { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }

    public class DocumentFilter
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}