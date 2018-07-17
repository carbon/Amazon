using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Carbon.Data;
using Carbon.Json;
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb
{
    public sealed class DbValueConverterFactory
    {
        private static readonly Dictionary<Type, IDbValueConverter> converters = new Dictionary<Type, IDbValueConverter>();

        static DbValueConverterFactory()
        {
            Add<bool>(new BooleanConverter());
            Add<DateTime>(new DateTimeConverter());
            Add<DateTimeOffset>(new DateTimeOffsetConverter());
            Add<decimal>(new DecimalConverter());
            Add<double>(new DoubleConverter());
            Add<float>(new SingleConverter());
            Add(new GuidConverter());
            Add<Int32>(new Int32Converter());
            Add<Int64>(new Int64Converter());
            Add<string>(new StringConverter());
            Add<TimeSpan>(new TimeSpanConverter());

            Add<byte[]>(new BinaryConverter());
            Add<string[]>(new StringArrayConverter());
            Add<Int16[]>(new ArrayConverter<Int16>());
            Add<Int32[]>(new ArrayConverter<Int32>());
            Add<Int64[]>(new ArrayConverter<Int64>());
            Add<float[]>(new ArrayConverter<float>());
            Add<double[]>(new ArrayConverter<double>());

            Add<HashSet<string>>(new StringHashSetConverter());
            Add<HashSet<Int16>>(new HashSetConverter<Int16>());
            Add<HashSet<Int32>>(new HashSetConverter<Int32>());
            Add<HashSet<Int64>>(new HashSetConverter<Int64>());
            Add<HashSet<float>>(new HashSetConverter<float>());
            Add<HashSet<double>>(new HashSetConverter<double>());

            // Add<UInt16>(new UInt16Converter());
            // Add<UInt32>(new UInt32Converter());
            // Add<UInt64>(new UInt64Converter());

            Add(new VersionConverter());
            Add(new UriConverter());
            Add(new IPAddressConverter());

            // Custom
            Add<JsonObject>(new JsonObjectConverter());
            Add<JsonArray>(new JsonArrayConverter());
        }

        public static IDbValueConverter Get(Type type)
        {
            var details = TypeDetails.Get(type);

            if (details.IsEnum) return new EnumConverter();

            if (!TryGet(details.NonNullType, out IDbValueConverter converter))
            {
                throw new ConversionException($"No converter found for '{type.Name}'.");
            }

            return converter;
        }

        public static bool TryGet(Type type, out IDbValueConverter converter)
        {
            var details = TypeDetails.Get(type);

            if (details.IsEnum)
            {
                converter = new EnumConverter();

                return true;
            }

            return converters.TryGetValue(details.NonNullType, out converter);
        }

        public static void Add<T>(DbTypeConverter<T> converter)
        {
            converters.Add(typeof(T), converter);
        }

        internal static void Add<T>(IDbValueConverter converter)
        {
            converters.Add(typeof(T), converter);
        }
    }

    public interface IDbValueConverter
    {
        DbValue FromObject(object value, IMember meta = null);

        object ToObject(DbValue item, IMember meta);
    }

    internal sealed class EnumConverter : IDbValueConverter
    {
        public static readonly EnumConverter Default = new EnumConverter();

        // ulong?

        public DbValue FromObject(object value, IMember member) => 
            new DbValue(Convert.ToInt32(value));

        public object ToObject(DbValue item, IMember member)
        {
            // Parse strings to allow graceful migration to integers

            if (item.Kind == DbValueType.S)
            {
                return Enum.Parse(member.Type, item.ToString());
            }

            return Enum.ToObject(member.Type, item.ToInt());
        }
    }

    internal sealed class StringArrayConverter : DbTypeConverter<string[]>
    {
        // item.ToStringSet().ToArray();

        public override string[] Parse(DbValue dbValue) => dbValue.ToArray<string>();

        public override DbValue ToDbValue(string[] value) => new DbValue(value);
    }

    internal sealed class GuidConverter : DbTypeConverter<Guid>
    {
        public override Guid Parse(DbValue item) => new Guid(item.ToBinary());

        public override DbValue ToDbValue(Guid value) => new DbValue(value.ToByteArray());
    }

    internal sealed class ArrayConverter<T> : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue((T[])value);

        public object ToObject(DbValue item, IMember member) => item.ToArray<T>();
    }

    internal sealed class StringHashSetConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(((HashSet<string>)value).ToArray());

        public object ToObject(DbValue item, IMember member) => item.ToStringSet();
    }

    internal sealed class HashSetConverter<T> : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(((HashSet<T>)value).ToArray());

        public object ToObject(DbValue item, IMember member) => item.ToSet<T>();
    }

    internal sealed class BinaryConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var data = (byte[])value;

            if (data.Length == 0) return DbValue.Empty;

            return new DbValue(data, DbValueType.B);
        }

        public object ToObject(DbValue item, IMember member) => item.ToBinary();
    }

    internal sealed class UriConverter : DbTypeConverter<Uri>
    {
        public override Uri Parse(DbValue item) => 
            new Uri(item.ToString());

        public override DbValue ToDbValue(Uri value) => new DbValue(value.ToString());
    }

    internal sealed class VersionConverter : DbTypeConverter<Version>
    {
        public override Version Parse(DbValue item) => Version.Parse(item.ToString());

        public override DbValue ToDbValue(Version value) => new DbValue(value.ToString());
    }
    
    internal sealed class IPAddressConverter : DbTypeConverter<IPAddress>
    {
        public override IPAddress Parse(DbValue item)
        {
            switch (item.Kind)
            {
                case DbValueType.S : return IPAddress.Parse(item.ToString());
                case DbValueType.B : return new IPAddress(item.ToBinary());
                default            : throw new ConversionException($"Cannot DB type: {item.Kind} to IPAddress");
            }
        }

        // Serialize IP addresses as bytes
        public override DbValue ToDbValue(IPAddress value) =>
            new DbValue(value.GetAddressBytes());
    }

    internal sealed class StringConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var text = (string)value;

            if (text.Length == 0) return DbValue.Empty;

            return new DbValue(text);
        }

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToString();
        }
    }

    internal sealed class Int32Converter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) =>
            new DbValue((int)value);

        public object ToObject(DbValue item, IMember member) => item.ToInt();
    }

    internal sealed class SingleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue((Single)value);
        }

        public object ToObject(DbValue item, IMember member) => item.ToSingle();
    }

    internal sealed class DecimalConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue((decimal)value);
        }

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToDecimal();
        }
    }

    internal sealed class DoubleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) =>
            new DbValue((double)value);

        public object ToObject(DbValue item, IMember member) =>
            item.ToDouble();
    }

    internal sealed class BooleanConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue((bool)value);
        }

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToBoolean();
        }
    }

    internal sealed class Int64Converter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue((long)value);

        public object ToObject(DbValue item, IMember member)
            => item.ToInt64();
    }

    internal sealed class DateTimeConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var date = new DateTimeOffset((DateTime)value);

            if (member?.Precision == 4)
            {
                return new DbValue(date.ToUnixTimeMilliseconds());
            }
            
            return new DbValue(date.ToUnixTimeSeconds());
        }

        public object ToObject(DbValue item, IMember member)
        {
            if (member?.Precision == 4)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(item.ToInt64()).UtcDateTime;
            }

            return DateTimeOffset.FromUnixTimeSeconds(item.ToInt64()).UtcDateTime;
        }
    }
    
    internal sealed class DateTimeOffsetConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var date = (DateTimeOffset)value;

            var precision = (TimePrecision)(member?.Precision ?? 0);

            switch (precision)
            {
                case TimePrecision.Millisecond : return new DbValue(date.ToUnixTimeMilliseconds());
                default                        : return new DbValue(date.ToUnixTimeSeconds());
            }
        }

        public object ToObject(DbValue item, IMember member)
        {
            var precision = (TimePrecision)(member?.Precision ?? 0);

            switch (precision)
            {
                case TimePrecision.Millisecond : return DateTimeOffset.FromUnixTimeMilliseconds(item.ToInt64());
                default                        : return DateTimeOffset.FromUnixTimeSeconds(item.ToInt64());
            }
        }
    }

    internal sealed class TimeSpanConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var time = (TimeSpan)value;
            var precision = (TimePrecision)(member?.Precision ?? 3);

            switch (precision)
            {
                case TimePrecision.Second: return new DbValue((int)time.TotalSeconds);
                default: return new DbValue((int)time.TotalMilliseconds);
            }
        }

        public object ToObject(DbValue item, IMember member)
        {
            var precision = (TimePrecision)(member.Precision ?? 3);

            switch (precision)
            {
                case TimePrecision.Second: return TimeSpan.FromSeconds(item.ToInt());
                default: return TimeSpan.FromMilliseconds(item.ToInt());
            }
        }
    }

    internal sealed class JsonObjectConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue(value.ToString());

        public object ToObject(DbValue item, IMember member) => JsonObject.Parse(item.ToString());
    }

    internal sealed class JsonArrayConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue(value.ToString());

        public object ToObject(DbValue item, IMember member)
        {
            var text = item.ToString();

            return JsonArray.Parse(text);
        }
    }
}