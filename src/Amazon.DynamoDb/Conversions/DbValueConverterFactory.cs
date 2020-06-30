using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Carbon.Data;
using Carbon.Json;
using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

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
            Add<DbValue[]>(new DbValueArrayConverter());

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
            Add(new UidConverter());

            // Custom
            Add<JsonObject>(new JsonObjectConverter());
            Add<JsonArray>(new JsonArrayConverter());
        }

        public static IDbValueConverter Get(Type type)
        {
            var details = TypeDetails.Get(type);

            if (details.IsEnum) return new EnumConverter();

            if (!TryGet(details.NonNullType, out IDbValueConverter? converter))
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
        DbValue FromObject(object value, IMember meta = null!);

        object ToObject(DbValue item, IMember meta);
    }
}