using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class StreamSpecification
    {
        public StreamSpecification() { }
        public StreamSpecification(bool streamEnabled, StreamViewType streamViewType)
        {
            StreamEnabled = streamEnabled;
            StreamViewType = streamViewType;
        }

        public bool StreamEnabled { get; set; }
        public StreamViewType StreamViewType { get; set; }
    }
}
