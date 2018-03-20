namespace Amazon.DynamoDb
{
    public enum ReturnValues
    {
        /// <summary>
        /// If this parameter is not provided or is NONE, nothing is returned. 
        /// </summary>
        NONE = 0,

        /// <summary>
        /// ALL_OLD is specified, and UpdateItem overwrote an attribute name-value pair, the content of the old item is returned. 
        /// </summary>
        ALL_OLD = 1,

        UPDATED_OLD = 2,

        /// <summary>
        /// All the attributes of the new version of the item are returned. 
        /// </summary>
        ALL_NEW = 3,

        /// <summary>
        /// The new versions of only the updated attributes are returned.
        /// </summary>
        UPDATED_NEW = 4
    }
}