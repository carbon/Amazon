#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public sealed class ModifyListenerRequest : IElbRequest
    {
        public string Action => "ModifyListener";

        public Certificate[] Certificates { get; set; }

        public Action[] DefaultActions { get; set; }
        
        [Required]
        public string ListenerArn { get; set; }

        public int? Port { get; set; }

        public string Protocol { get; set; }

        public string SslPolicy { get; set; }
    }
}