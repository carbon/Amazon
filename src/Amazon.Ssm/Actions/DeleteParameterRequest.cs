using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class DeleteParameterRequest : ISsmRequest
    {
        public DeleteParameterRequest() { }

        public DeleteParameterRequest(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}