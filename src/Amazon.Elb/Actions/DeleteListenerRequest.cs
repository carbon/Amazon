using System;

namespace Amazon.Elb
{
    public sealed class DeleteListenerRequest : IElbRequest
    {
        public DeleteListenerRequest(string listenerArn)
        {
            this.ListenerArn = listenerArn ?? throw new ArgumentNullException(nameof(listenerArn));
        }

        public string Action => "DeleteListener";
        
        public string ListenerArn { get; }
    }
}
