using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public interface IConvertibleFromJson
    {
        void FillField(JsonProperty property);
    }
}
