namespace Amazon.DynamoDb
{
    public class CountResult
    {
        public CountResult() { }

        public CountResult(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}

/*
{
    "Count":17
}
*/
