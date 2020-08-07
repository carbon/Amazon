#nullable disable

namespace Amazon.DynamoDb
{
    public sealed class QueryResult
    {
        public ConsumedCapacity ConsumedCapacity { get; set; }

        public AttributeCollection[] Items { get; set; }

        public AttributeCollection LastEvaluatedKey { get; set; }

        public int Count { get; set; }
    }
}

/* 
{ 
	"Count": 343,
	"ConsumedCapacity": {
		"TableName": "Thread",
		"CapacityUnits": 1
	},
	"Items": [
		{ 
			"hitCount": {"N":"225"},
			"date": {"S":"2011-05-31T00:00:00Z"},
			"siteId": {"N":"221051"}
		},
		{
			"hitCount": {"N":"120"},
			"date": {"S":"2011-06-01T00:00:00Z"},
			"siteId": {"N":"221051"}
		},
		{
			"hitCount": {"N":"6680"},
			"date": {"S":"2011-06-02T00:00:00Z"},
			"siteId": {"N":"221051"}
		}
	],
	"LastEvaluatedKey": {
		"string" :
			{
				"B": "blob",
				"BS": [
					"blob"
				],
				"N": "string",
				"NS": [
					"string"
				],
				"S": "string",
				"SS": [
					"string"
				]
			}
	}
}
*/
