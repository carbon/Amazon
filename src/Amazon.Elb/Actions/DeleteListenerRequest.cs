using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DeleteListenerRequest : IElbRequest
    {
        public string Action => "DeleteListener";
        
        [Required]
        public string ListenerArn { get; set; }
    }
}
