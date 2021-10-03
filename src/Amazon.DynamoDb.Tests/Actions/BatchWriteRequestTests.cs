namespace Amazon.DynamoDb
{
    public class BatchWriteRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var batches = new TableRequests[]
            {
                new TableRequests("A", new List<ItemRequest> {
                    new PutRequest(new AttributeCollection { { "id", 1 } }),
                    new DeleteRequest(new AttributeCollection { { "id", 2 } }),
                }),

                new TableRequests("B", new List<ItemRequest> {
                    new DeleteRequest(new AttributeCollection { { "id", 1 } }),
                    new DeleteRequest(new AttributeCollection { { "id", 2 } }),
                })
            };


            var q = new Dictionary<string, object>(batches.Length);

            foreach (var batch in batches)
            {
                q.Add(batch.TableName, batch.SerializeList());
            }

            var r = new {
                RequestItems = q
            };

            Assert.Equal(@"{""RequestItems"":{""A"":[{""PutRequest"":{""Item"":{""id"":{""N"":""1""}}}},{""DeleteRequest"":{""Key"":{""id"":{""N"":""2""}}}}],""B"":[{""DeleteRequest"":{""Key"":{""id"":{""N"":""1""}}}},{""DeleteRequest"":{""Key"":{""id"":{""N"":""2""}}}}]}}", r.ToSystemTextJson());
        }
    }
}