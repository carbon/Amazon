#nullable disable

using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Kms
{
    public class GrantConstraints
    {
        // Match all
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContextEquals { get; set; }

        // Match any
        [DataMember(EmitDefaultValue = false)]
        public JsonObject EncryptionContextSubset { get; set; }
    }
}