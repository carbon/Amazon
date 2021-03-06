﻿namespace Amazon.DynamoDb.Models
{
    public sealed class ArchivalSummary
    {
        public string? ArchivalBackupArn { get; set; }

        public string? ArchivalDateTime { get; set; }

        public string? ArchivalReason { get; set; }
    }
}