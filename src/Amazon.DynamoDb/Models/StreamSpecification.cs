using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class StreamSpecification
    {
        public StreamSpecification(bool streamEnabled, StreamViewType streamViewType)
        {
            StreamEnabled = streamEnabled;
            StreamViewType = streamViewType;
        }

        public bool StreamEnabled { get; set; }
        public StreamViewType StreamViewType { get; set; }

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
