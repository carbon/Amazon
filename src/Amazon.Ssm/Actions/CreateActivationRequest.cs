#nullable disable

namespace Amazon.Ssm
{
    public class CreateActivationRequest : ISsmRequest
    {
        public string DefaultInstanceName { get; set; }

        public string Description { get; set; }

        public string ExpirationDate { get; set; }
        
        public string IamRole { get; set; }

        public int RegistrationLimit { get; set; }
    }
}