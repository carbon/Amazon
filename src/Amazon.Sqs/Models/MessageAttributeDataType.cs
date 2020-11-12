namespace Amazon.Sqs
{
    public enum MessageAttributeDataType
    {
        String = 1,
        Number = 2, // A number can have up to 38 digits of precision, and it can be between 10^-128 and 10^+126.
        Binary = 3
    }

    internal static class MessageAttributeDataTypeExtensions
    {
        public static string Canonicalize(this MessageAttributeDataType type) => type switch
        {
            MessageAttributeDataType.String => "String",
            MessageAttributeDataType.Number => "Number",
            MessageAttributeDataType.Binary => "Binary",
            _ => type.ToString()
        };
    }
}