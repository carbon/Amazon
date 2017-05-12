namespace Amazon.Elb
{
    public class RemoveTagsRequest : IElbRequest
    {
        public string Action => "RemoveTags";
        
        public string[] ResourceArns { get; set; }

        public string[] TagKeys { get; set; }
    }
}