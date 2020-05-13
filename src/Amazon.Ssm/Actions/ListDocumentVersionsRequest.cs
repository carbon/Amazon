#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class ListDocumentVersionsRequest : ISsmRequest
    {
        public ListDocumentVersionsRequest() { }

        public ListDocumentVersionsRequest(string name)
        {
            Name = name;
        }

        public int? MaxResults { get; set; }

        [Required]
        public string Name { get; set; }

        public string NextToken { get; set; }
    }
}