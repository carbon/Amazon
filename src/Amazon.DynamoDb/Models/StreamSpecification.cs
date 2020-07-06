using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class StreamSpecification : IConvertibleFromJson
    {
        public StreamSpecification() { }
        public StreamSpecification(bool streamEnabled, StreamViewType streamViewType)
        {
            StreamEnabled = streamEnabled;
            StreamViewType = streamViewType;
        }

        public bool StreamEnabled { get; set; }
        public StreamViewType StreamViewType { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("StreamEnabled")) StreamEnabled = property.Value.GetBoolean();
            else if (property.NameEquals("StreamViewType")) StreamViewType = property.Value.GetEnum<StreamViewType>();
        }

        public JsonObject ToJson()
        {
            return new JsonObject()
            {
                { "StreamEnabled", StreamEnabled },
                { "StreamViewType", StreamViewType.ToQuickString() }
            };
        }
    }
}
