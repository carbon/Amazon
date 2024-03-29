﻿using System.Text.Json.Serialization;

namespace Amazon.Ses;

[JsonConverter(typeof(JsonStringEnumConverter<SesNotificationType>))]
public enum SesNotificationType
{
    Bounce    = 1,
    Complaint = 2,
    Received  = 3,
    Delivery  = 4
}