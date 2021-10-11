#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class DeleteParameterRequest : ISsmRequest
{
    public DeleteParameterRequest() { }

    public DeleteParameterRequest(string name)
    {
        Name = name;
    }

    [Required]
    public string Name { get; set; }
}
