using System;

using Carbon.Data.Annotations;

namespace Amazon.DynamoDb
{
    [Dataset("Entities")]
    public class Entity
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("type")]
        public EntityType Type { get; set; }

        [Member("name")]
        public string Name { get; set; }

        [Member("displayName")]
        public string DisplayName { get; set; }

        [Member("slug"), Unique]
        public string Slug { get; set; }

        [Member("domain")]
        public string Domain { get; set; }

        [Member("ownerId")]
        public long? OwnerId { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("locked")]
        public DateTime? Locked { get; set; }
    }

    public enum EntityType
    {
        Organization = 1,
        Person = 2
    }
}