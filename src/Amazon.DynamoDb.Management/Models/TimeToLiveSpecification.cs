﻿namespace Amazon.DynamoDb.Models;

public sealed class TimeToLiveSpecification
{
#nullable disable
    public TimeToLiveSpecification() { }
#nullable enable

    public TimeToLiveSpecification(string attributeName, bool enabled)
    {
        AttributeName = attributeName;
        Enabled = enabled;
    }

    public string AttributeName { get; init; }

    public bool Enabled { get; init; }
}
