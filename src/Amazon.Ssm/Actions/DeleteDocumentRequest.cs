using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class DeleteDocumentRequest : ISsmRequest
    {
        public DeleteDocumentRequest() { }

        public DeleteDocumentRequest(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}