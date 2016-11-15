using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Carbon.Data;
using Carbon.Json;
using Carbon.Data.Annotations;
using Carbon.Collections;

namespace Amazon.DynamoDb
{
    public sealed class DbValueConverterFactory
    {
        private static readonly Dictionary<Type, IDbValueConverter> converters = new Dictionary<Type, IDbValueConverter>();

        static DbValueConverterFactory()
        {
            Add<bool>(new BooleanConverter());
            Add<DateTime>(new DateTimeConverter());
            // Add<DateTimeOffset>(new DateTimeOffsetConverter());
            Add<decimal>(new DecimalConverter());
            Add<double>(new DoubleConverter());
            Add<float>(new SingleConverter());
            Add<Guid>(new GuidConverter());
            Add<Int32>(new Int32Converter());
            Add<Int64>(new Int64Converter());
            Add<IPAddress>(new IPAddressConverter());
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

            Add<Version>(new VersionConverter());
            Add<Uri>(new UriConverter());

            // Custom
            Add<JsonObject>(new JsonObjectConverter());
            Add<JsonArray>(new XArrayConverter());

            Add<Int32List>(new Int32ListDbConverter());
            Add<StringList>(new StringListDbConverter());
            Add<StringMap>(new StringMapDbConverter());
        }

        public static IDbValueConverter Get(Type type)
        {
            var details = TypeDetails.Get(type);

            IDbValueConverter converter;

            if (details.IsEnum) return new EnumConverter();

            if (!TryGet(details.NonNullType, out converter))
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
      

        public static void Add<T>(IDbValueConverter converter)
        {
            converters.Add(typeof(T), converter);
        }
    }

    public interface IDbValueConverter
    {
        DbValue FromObject(object value, IMember meta);

        object ToObject(DbValue item, IMember meta);
    }

    internal class EnumConverter : IDbValueConverter
    {
        public static EnumConverter Default = new EnumConverter();

        // ulong?

        public DbValue FromObject(object value, IMember member)
            => new DbValue(Convert.ToInt32(value));

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

    internal class StringArrayConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue((string[])value);

        public object ToObject(DbValue item, IMember member)
            => item.ToStringSet().ToArray();
    }

    internal class GuidConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(((Guid)value).ToByteArray());

        public object ToObject(DbValue item, IMember member)
            => new Guid(item.ToBinary());
    }

    internal class ArrayConverter<T> : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue((T[])value);

        public object ToObject(DbValue item, IMember member)
            => item.ToArray<T>();
    }

    internal class StringHashSetConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(((HashSet<string>)value).ToArray());

        public object ToObject(DbValue item, IMember member)
            => item.ToStringSet();
    }

    internal class HashSetConverter<T> : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(((HashSet<T>)value).ToArray());

        public object ToObject(DbValue item, IMember member)
            => item.ToSet<T>();
    }

    internal class BinaryConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var data = (byte[])value;

            if (data.Length == 0) return DbValue.Empty;

            return new DbValue(data, DbValueType.B);
        }

        public object ToObject(DbValue item, IMember member)
            => item.ToBinary();
    }

    internal class UriConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(value.ToString());

        public object ToObject(DbValue item, IMember member)
            => new Uri(item.ToString());
    }

    internal class VersionConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue(value.ToString());

        public object ToObject(DbValue item, IMember member)
            => Version.Parse(item.ToString());
    }

    // Serialize IP addresses as bytes

    internal class IPAddressConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var ip = (IPAddress)value;

            return new DbValue(ip.GetAddressBytes());
        }

        public object ToObject(DbValue item, IMember member)
        {
            switch (item.Kind)
            {
                case DbValueType.S: return IPAddress.Parse(item.ToString());
                case DbValueType.B: return new IPAddress(item.ToBinary());
                default: throw new Exception("Unexpected DB type:" + item.Kind.ToString());
            }
        }
    }

    internal class StringConverter : IDbValueConverter
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

    internal class Int32Converter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue((Int32)value);
        }

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToInt();
        }
    }

    internal class SingleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue((Single)value);
        }

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToSingle();
        }
    }

    internal class DecimalConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue((Decimal)value);
        }

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToDecimal();
        }
    }

    internal class DoubleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue((double)value);

        public object ToObject(DbValue item, IMember member)
        {
            return item.ToDouble();
        }
    }

    internal class BooleanConverter : IDbValueConverter
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

    internal class Int64Converter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
            => new DbValue((Int64)value);

        public object ToObject(DbValue item, IMember member)
            => item.ToInt64();
    }

    internal class DateTimeConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var date = (DateTime)value;

            return new DbValue(new XDate(date).ToUnixTime());
        }

        public object ToObject(DbValue item, IMember member)
        {
            return XDate.FromUnixTime(item.ToInt()).ToUtcDateTime();
        }
    }

    internal class TimeSpanConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var time = (TimeSpan)value;
            var precision = (TimePrecision)(member.Precision ?? 3);

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

    internal class JsonObjectConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue(value.ToString());
        }

        public object ToObject(DbValue item, IMember member)
        {
            var text = item.ToString();

            return JsonObject.Parse(text);
        }
    }

    internal class XArrayConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue(value.ToString());
        }

        public object ToObject(DbValue item, IMember member)
        {
            var text = item.ToString();

            return JsonArray.Parse(text);
        }
    }

    internal class Int32ListDbConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var list = value as Int32List;

            if (list == null || list.Count == 0) return DbValue.Empty;

            return new DbValue(list.ToBytes());
        }

        public object ToObject(DbValue item, IMember member)
        {
            // Gracefully migrate from number sets to a custom binary serializer that respects order
            switch (item.Kind)
            {
                case DbValueType.NS : return new Int32List(item.ToArray<int>());
                case DbValueType.B  : return Int32List.FromBytes(item.ToBinary());
                default             : return new Int32List();
            }
        }
    }
    

    internal class StringListDbConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var list = value as StringList;

            if (list == null || list.Count == 0) return DbValue.Empty;

            return new DbValue(list.Select(item => new DbValue(item)));
        }

        public object ToObject(DbValue item, IMember member)
        {
            // Gracefully migrates from StringSets and custom binary provider to a List

            switch (item.Kind)
            {
                case DbValueType.SS : return new StringList(item.ToStringSet());
                case DbValueType.B  : return StringList.FromBytes(item.ToBinary());
                case DbValueType.L  : return new StringList(item.ToArray<string>());
                default             : return new StringList();
            }
        }
    }

    internal class ObjectConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var list = value as StringList;

            if (list == null || list.Count == 0) return DbValue.Empty;

            return new DbValue(list.Select(item => new DbValue(item)));
        }

        public object ToObject(DbValue item, IMember member)
        {
            // Gracefully migrates from StringSets and custom binary provider to a List

            switch (item.Kind)
            {
                case DbValueType.SS : return new StringList(item.ToStringSet());
                case DbValueType.B  : return StringList.FromBytes(item.ToBinary());
                case DbValueType.L  : return new StringList(item.ToArray<string>());
                default             : return new StringList();
            }
        }
    }

    internal class StringMapDbConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var list = value as StringMap;

            if (list == null || list.Count == 0) return DbValue.Empty;

            return new DbValue(list.ToBytes());
        }

        public object ToObject(DbValue item, IMember member)
        {
            // TODO: Use a nested map

            switch (item.Kind)
            {
                case DbValueType.B: return StringMap.FromBytes(item.ToBinary());
                default: return new StringMap();
            }
        }
    }
}