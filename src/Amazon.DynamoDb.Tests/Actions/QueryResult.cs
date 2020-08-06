using System.Text.Json;

using Carbon.Json;

using Xunit;

namespace Amazon.DynamoDb.Tests
{
    public class QueryResultTests
	{
		[Fact]
		public void Item1()
		{
			var text = @"{""Count"":343,""Items"":[{""hitCount"":{""N"":""225""},""date"":{""S"":""2011-05-31T00:00:00Z""},""siteId"":{""N"":""221051""}},{""hitCount"":{""N"":""120""},""date"":{""S"":""2011-06-01T00:00:00Z""},""siteId"":{""N"":""221051""}},{""hitCount"":{""N"":""6680""},""date"":{""S"":""2011-06-02T00:00:00Z""},""siteId"":{""N"":""221051""}}]}";


			var result = System.Text.Json.JsonSerializer.Deserialize<QueryResult>(text);

			Assert.Equal(343, result.Count);

			Assert.Equal("225", result.Items[0]["hitCount"].Value);
			Assert.Equal("221051", result.Items[1]["siteId"].Value);
		}

		[Fact]
		public void QueryResultTest2()
		{
			var text = @"{""ConsumedCapacityUnits"":0.5,""Count"":7,""Items"":[{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""lXfCkUWvcZjaz2F9mZqP8LSp+ooaY0Cxm2vinpRVGUs=""},""number"":{""N"":""7""},""eTag"":{""S"":""\""2cb550e8b4bef93d1b7a3e4949a36a36\""""},""size"":{""N"":""4365569""},""offset"":{""N"":""31457280""}},{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""JAchovgasTUWxLnMHSypgcsGelsNYYY/GlWPn+Zh9aI=""},""number"":{""N"":""6""},""eTag"":{""S"":""\""a613b615d28717969ace06a05124065f\""""},""size"":{""N"":""5242880""},""offset"":{""N"":""26214400""}},{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""js1OTW7nUgltISP94s/fYh7H2iugOWEv0zw01jrC91o=""},""number"":{""N"":""5""},""eTag"":{""S"":""\""321840a3d07eee7f7990e2db1054578e\""""},""size"":{""N"":""5242880""},""offset"":{""N"":""20971520""}},{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""z5AodiDrkl4MbFxHMfxBJ2HFoOoOoQwQUTvQZ0RV+54=""},""number"":{""N"":""4""},""eTag"":{""S"":""\""b37d16ddf9429b484b71497bdce62783\""""},""size"":{""N"":""5242880""},""offset"":{""N"":""15728640""}},{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""3JLHnJo7Zl1otD/MStNt6OK+HWrfmJ3zXGvh9dvCoyY=""},""number"":{""N"":""3""},""eTag"":{""S"":""\""0b3183dcd48588d3f86bf8991b87fe45\""""},""size"":{""N"":""5242880""},""offset"":{""N"":""10485760""}},{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""PJUHzcapT3xJ90czN00RFcjq2D+5ZkPgrjGlqGuLlZs=""},""number"":{""N"":""2""},""eTag"":{""S"":""\""a9f09adc6b586881d09fda5bfc6d0627\""""},""size"":{""N"":""5242880""},""offset"":{""N"":""5242880""}},{""uploadId"":{""S"":""9897b7d1db0c41beaafe8ebfea9a2d54""},""sha256"":{""B"":""xUFiYOQ8VuwjarNT8PWsHgE2Fop/M4ATUNs4YRICUiY=""},""number"":{""N"":""1""},""eTag"":{""S"":""\""b6f856367615c29da66087712f03f2cf\""""},""size"":{""N"":""5242880""},""offset"":{""N"":""0""}}]}";

			var result = System.Text.Json.JsonSerializer.Deserialize<QueryResult>(text);

			Assert.Equal(7, result.Items.Length);

			Assert.Equal("lXfCkUWvcZjaz2F9mZqP8LSp+ooaY0Cxm2vinpRVGUs=", result.Items[0]["sha256"].ToString());
            Assert.Equal("JAchovgasTUWxLnMHSypgcsGelsNYYY/GlWPn+Zh9aI=", result.Items[1]["sha256"].ToString());


        }
    }

}
