using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class CreateListenerRequest : IElbRequest
    {
        public string Action => "CreateListener";

        public string LoadBalancerArn { get; set; }

        public List<Certificate> Certificates { get; set; }

        public List<Action> Actions { get; } = new List<Action>();

        [Range(1, 65535)]
        public ushort Port { get; set; }

        [Required]
        public string Protocal { get; set; }

        public string SslPolicy { get; set; }


        // Certificates
        // Default actions
    }
}

/*
https://elasticloadbalancing.amazonaws.com/?Action=CreateListener
&LoadBalancerArn=arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188
&Protocol=HTTPS
&Port=443
&Certificates.member.1.CertificateArn=arn:aws:iam::123456789012:server-certificate/my-server-cert
&SslPolicy=ELBSecurityPolicy-2016-08
&DefaultActions.member.1.Type=forward
&DefaultActions.member.1.TargetGroupArn=arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067
&Version=2015-12-01
&AUTHPARAMS
*/