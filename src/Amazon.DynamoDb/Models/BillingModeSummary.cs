using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class BillingModeSummary : IConvertibleFromJson
    {
        public BillingMode BillingMode { get; set; }
        public DateTimeOffset LastUpdateToPayPerRequestDateTime { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("BillingMode"))
            {
                BillingMode = property.Value.GetEnum<BillingMode>();
            }
            else if (property.NameEquals("LastUpdateToPayPerRequestDateTime"))
            {
                LastUpdateToPayPerRequestDateTime = property.Value.GetDynamoDateTimeOffset();
            }
        }
    }
}
