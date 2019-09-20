#nullable disable

namespace Amazon.Ses
{
    public class SesRecipient
    {
        public SesRecipient() { }

        public SesRecipient(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; set; }
    }
}