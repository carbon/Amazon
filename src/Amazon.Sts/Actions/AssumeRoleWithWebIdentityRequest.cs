namespace Amazon.Sts
{
    public class AssumeRoleWithWebIdentityRequest : IStsRequest
    {
        public string Action => "AssumeRoleWithWebIdentity";

        public int DurationSeconds { get; set; }

        public string Policy { get; set; }

        public string ProviderId { get; set; }

        public string RoleArn { get; set; }

        public string RoleSessionName { get; set; }

        public string WebIdentityToken { get; set; }
    }
}