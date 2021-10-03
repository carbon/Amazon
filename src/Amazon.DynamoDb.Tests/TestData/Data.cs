using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

namespace Amazon.DynamoDb.Models.Tests
{
    [Dataset("Things")]
    public class Thing
    {
        [Member("color"), Key]
        public Color Color { get; set; }
        
        [Member("color2")]
        public ABCEnum X { get; set; }

        [Member("color3")]
        public ABCEnum? Y { get; set; }
    }

    public enum Color : byte
    {
        Blue = 1,
        Red = 2,
        Orange = 3
    }

    public class Fruit
    {
        [Member("name"), Key]
        [IgnoreDataMember]
        public string Name { get; set; }

        [Member("calories")]
        public int Calories { get; set; }

        [Member("description")]
        public string Description { get; set; }

        public bool IsHealthy { get; set; }
    }


    public class Point
    {
        [Member("1"), Key]
        public float X { get; set; }

        [Member("2"), Key]
        public float Y { get; set; }

        [Member("3"), Key]
        public float Z { get; set; }
    }

    [Dataset("Machine")]
    public class Machine
    {
        [Member("id"), Key]
        public int Id { get; set; }

        [Member("ips")]
        public List<IPAddress> Ips { get; set; }
    }

    [Dataset("People")]
    public class Person
    {
        [Member("Name"), Key]
        public string Name { get; set; }

        [Member("Age")]
        public int Age { get; set; }
    }

    [Dataset("Media")]
    public class Media
    {
        [Member("Transformation"), Key]
        public string Transformation { get; set; }

        [Member("ErrorMessage")]
        public string ErrorMessage { get; set; }

        [Member("Hash")]
        public byte[] Hash { get; set; }

        [Member("Duration")]
        [TimePrecision(TimePrecision.Millisecond)]
        public TimeSpan Duration { get; set; }

        [Member("Created")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; set; }
    }

    [Dataset("Smorsborgs")]
    public class Smorsborg
    {
        [Member("Int32"), Key]
        public int Int32 { get; set; }

        [Member("Int64")]
        public long Int64 { get; set; }

        [Member("Boolean")]
        public Boolean Boolean { get; set; }

        [Member("Date")]
        public DateTime Date { get; set; }
    }

    public class Record
    {
        [Member(1), Key]
        public long Id { get; set; }

        [Member("Type")]
        public RecordType Type { get; set; }
    }


    public class ReadOnlyRecord
    {
        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public RecordType Type { get; }
    }

    public enum RecordType
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3
    }


    [Dataset("Entity")]
    public class Entity
    {
        [Member("id"), Key]
        public Uid Id { get; set; }
    }

    [Dataset("Hi")]
    public class Hi
    {
        [Member("a"), Key]
        public Nested A { get; set; }

        [Member("b")]
        public Nested B { get; set; }
    }

    [Dataset("Nested")]
    public class Nested
    {
        [Member("a"), Key]
        public long A { get; set; }

        [Member("b")]
        public string B { get; set; }
    }

    public struct Position
    {
        [Member("x"), Key]
        public float X { get; set; }

        [Member("y"), Key]
        public float Y { get; set; }

        [Member("z"), Key]
        public float? Z { get; set; }
    }

    [Dataset("BlobMetadata")]
    public class Meta
    {
        [Member(1), Key]
        public byte[] Id { get; set; }

        [Member(2)]
        public string Name { get; set; }

        [Member(3)]
        public Annotation[] Annotations { get; set; }
    }

    [Dataset("Annotations")]
    public class Annotation
    {
        // public AnnotationType Type { get; set; }

        [Member(1), Key]
        public long Id { get; set; }

        [Member(2)]
        public string Description { get; set; }

        [Member(3)]
        public Position Position { get; set; }

        [Member(4)]
        public float Score { get; set; }

        [Member(5)]
        public float Confidence { get; set; }

        [Member(6)]
        public float Topicality { get; set; }

    }
}
