namespace Amazon.Sqs
{
	public class SqsError
	{
		public string Type { get; set; }
		public string Code { get; set; }
		public string Message { get; set; }
		public string Detail { get; set; }
	}
}