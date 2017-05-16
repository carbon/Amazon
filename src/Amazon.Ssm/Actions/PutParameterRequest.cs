namespace Amazon.Ssm
{
    public class PutParameterRequest : ISsmRequest
    {
        public string Description { get; set; }

        public string KeyId { get; set; }

        public string Name { get; set; }

        public bool Overwrite { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}