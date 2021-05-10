using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.JsonConverters;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(DbValueConverter))]
    public readonly struct DbValue : IConvertible
	{
		public static readonly DbValue Empty = new (string.Empty, DbValueType.Unknown);
		public static readonly DbValue Null	 = new (string.Empty, DbValueType.NULL);
		public static readonly DbValue True  = new (true);
		public static readonly DbValue False = new (false);

		private readonly DbValueType _kind;
		private readonly object _value;

		public DbValue(string value)	 : this(value, DbValueType.S) { }
	                                     
		public DbValue(bool value)	     : this(value, DbValueType.BOOL) { }
		public DbValue(byte value)		 : this(value, DbValueType.N) { }
		public DbValue(decimal value)	 : this(value, DbValueType.N) { }
		public DbValue(double value)	 : this(value, DbValueType.N) { }
		public DbValue(short value)		 : this(value, DbValueType.N) { }
		public DbValue(int value)		 : this(value, DbValueType.N) { }
		public DbValue(long value)		 : this(value, DbValueType.N) { }
		public DbValue(float value)	     : this(value, DbValueType.N) { }
		public DbValue(ushort value)	 : this(value, DbValueType.N) { }
		public DbValue(uint value)	     : this(value, DbValueType.N) { }
		public DbValue(ulong value)	     : this(value, DbValueType.N) { }
		public DbValue(byte[] value)	 : this(value, DbValueType.B) { }
                                         
		public DbValue(short[] values)	 : this(values, DbValueType.NS) { }
		public DbValue(int[] values)	 : this(values, DbValueType.NS) { }
		public DbValue(long[] values)	 : this(values, DbValueType.NS) { }
		public DbValue(float[] values)	 : this(values, DbValueType.NS) { }
		public DbValue(double[] values)	 : this(values, DbValueType.NS) { }
		public DbValue(string[] values)	 : this(values, DbValueType.SS) { }
		public DbValue(byte[][] values)	 : this(values, DbValueType.BS) { }

		public DbValue(DbValue[] values)            : this(values, DbValueType.L) { }
        public DbValue(IEnumerable<DbValue> values) : this(values, DbValueType.L) { }

        public DbValue(ISet<string>[] values) : this(values, DbValueType.SS) { }
		public DbValue(ISet<Int32>[] values)  : this(values, DbValueType.NS) { }

        public DbValue(AttributeCollection map)
        {
            _kind = DbValueType.M;
            _value = map;
        }

		public DbValue(object value, DbValueType type)
		{
			_value = value;
			_kind = type;
		}

		public DbValue(object value)
		{
            var type = value.GetType();

			if (type.IsEnum)
			{
				_kind = DbValueType.N;
				_value = EnumConverter.Default.FromObject(value, null!).Value;
 
				return;
			}

			if (type.IsArray)
			{
				var elementType = type.GetElementType();

                _kind = (Type.GetTypeCode(elementType)) switch
                {
                    TypeCode.Byte => DbValueType.B,
                    TypeCode.String => DbValueType.SS, // StringSet
                    TypeCode.Decimal or 
					TypeCode.Double or 
					TypeCode.Int16 or 
					TypeCode.Int32 or 
					TypeCode.Int64 or
					TypeCode.Single or 
					TypeCode.UInt16 or 
					TypeCode.UInt32 or
					TypeCode.UInt64 => DbValueType.NS,
                    _ => throw new Exception("Invalid array element type:" + type.Name),
                };
            }
			else
			{
				switch (Type.GetTypeCode(type))
				{
					// String
					case TypeCode.String:
                        if ((string)value == string.Empty)
                        {
                            throw new ArgumentException(paramName: nameof(value), message: "Must not be empty");
                        }

						_kind = DbValueType.S;

						break;

					// Numbers
					case TypeCode.Decimal:
					case TypeCode.Double:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64: _kind = DbValueType.N; break;

					case TypeCode.DateTime:
                        // Convert dates to unixtime

                        _value = new DateTimeOffset((DateTime)value).ToUnixTimeSeconds();
						_kind = DbValueType.N;

						return;

					// Boolean
					case TypeCode.Boolean:
						_value = (bool)value ? 1 : 0;
						_kind = DbValueType.N;

						return;

					default: {
						if (type == typeof(DbValue))
						{
							var dbValue = (DbValue)value;

							_kind = dbValue.Kind;
							_value = dbValue.Value;

						}
                        else if (type == typeof(AttributeCollection))
                        {
                           _value = value;
                           _kind = DbValueType.M;        
                        }
						else
						{
                            if (!DbValueConverterFactory.TryGet(type, out IDbValueConverter? converter))
                            {
                                throw new Exception("Invalid value type. Was: " + type.Name);
                            }

                            var result = converter.FromObject(value, null!);

                            _value = result.Value;
                            _kind = result.Kind;                            
                        }

                        break;
					}
				}
			}
			

			_value = value;
		}

		public readonly DbValueType Kind => _kind;

		/// <summary>
		/// int, long, string, string [ ]
		/// </summary>
		public readonly object Value => _value;

		internal readonly object ToPrimitiveValue()
		{
			return _kind switch {
				DbValueType.B => ToBinary(),
				DbValueType.N => ToInt64(),// TODO, return a double if there's a floating point
				DbValueType.S => ToString(),
				_			  => throw new Exception($"Cannot convert {_kind} to native value."),
			};
		}

		#region To Helpers

		public readonly HashSet<string> ToStringSet()
		{
			if (!(_kind is DbValueType.NS or DbValueType.SS or DbValueType.BS)) 
			{
				throw new Exception("Cannot be converted to a set.");
			}

			if (_value is IEnumerable<string> enumerable)
			{
				return new HashSet<string>(enumerable);		
			}

			var set = new HashSet<string>();

			foreach (var item in (IEnumerable)_value)
			{
				set.Add(item!.ToString()!);
			}

			return set;
		}

		public readonly HashSet<T> ToSet<T>()
		{
			if (!(_kind is DbValueType.NS or DbValueType.SS or DbValueType.BS))
			{
				throw new Exception($"The value type '{_kind}' cannot be converted to a Int32 Set.");
			}

			// Avoid additional allocations where possible
			if (_value is IEnumerable<T> enumerable)
			{
				return new HashSet<T>(enumerable);
			}

			var set = new HashSet<T>();

			foreach (var item in (IEnumerable)_value)
			{
				set.Add((T)Convert.ChangeType(item, typeof(T))!);
			}

			return set;
		}

		public readonly T[] ToArray<T>()
		{
			var type = _value.GetType();
		
			if (type.IsArray && type.GetElementType() == typeof(T))
			{
				return (T[])_value;
			}

            IList collection = (IList)_value;
            
			var array = new T[collection.Count];
			
			for (int i = 0; i < collection.Count; i++)
			{
				array[i] = (T)Convert.ChangeType(collection[i], typeof(T))!;
			}

			return array;
		}

		public readonly bool ToBoolean()
		{
            if (_kind == DbValueType.N)
            {
                return ToInt() == 1;
            }

            if (_value is bool b)
            {
                return b;
            }

            throw new Exception($"Cannot convert '{_value.GetType().Name}' to a Boolean");
		}

		public readonly float ToSingle() => Convert.ToSingle(_value);

		public readonly short ToInt16() => Convert.ToInt16(_value);

		public readonly int ToInt() => Convert.ToInt32(_value);

		public readonly long ToInt64() => Convert.ToInt64(_value);

		public readonly double ToDouble() => Convert.ToDouble(_value);

		public readonly decimal ToDecimal() => Convert.ToDecimal(_value);

		public readonly uint ToUInt32() => Convert.ToUInt32(_value);

		public readonly byte[] ToBinary()
		{
            return _value is byte[] data
                ? data
                : Convert.FromBase64String(_value.ToString()!);
		}

		public override string ToString() => _value.ToString()!;

		#endregion

		public static DbValue FromJsonElement(in JsonElement json)
		{
			var enumerator = json.EnumerateObject();

			enumerator.MoveNext();

			JsonProperty property = enumerator.Current;

			return property.Name switch
			{
				"B"    => new DbValue(property.Value.GetString()!,     DbValueType.B),
				"N"    => new DbValue(property.Value.GetString()!,     DbValueType.N),
				"S"    => new DbValue(property.Value.GetString()!,     DbValueType.S),
				"BOOL" => new DbValue(property.Value.GetBoolean(),     DbValueType.BOOL),
				"BS"   => new DbValue(property.Value.GetStringArray(), DbValueType.BS),
				"NS"   => new DbValue(property.Value.GetStringArray(), DbValueType.NS),
				"SS"   => new DbValue(property.Value.GetStringArray(), DbValueType.SS),
				"L"    => new DbValue(GetListValues(property.Value)),
				"M"    => new DbValue(AttributeCollection.FromJsonElement(property.Value)),
				_      => throw new Exception("Invalid value type:" + property.Name),
			};
		}

		#region Casting

		public static explicit operator string(DbValue value) => value.ToString();

		public static explicit operator Int16(DbValue value) => value.ToInt16();

		public static explicit operator Int32(DbValue value) => value.ToInt();

		public static explicit operator Int64(DbValue value) => value.ToInt64();

		public static explicit operator Single(DbValue value) => value.ToSingle();

		public static explicit operator Double(DbValue value) => value.ToDouble();

		public static explicit operator byte[](DbValue value) => value.ToBinary();

		#endregion

		#region Helpers

		private static DbValue[] GetListValues(in JsonElement array)
		{
			var items = new DbValue[array.GetArrayLength()];

			int i = 0;

			foreach (var item in array.EnumerateArray())
			{
				items[i] = FromJsonElement(item);

				i++;
			}

			return items;
		}

		#endregion

		#region IConvertible

		readonly TypeCode IConvertible.GetTypeCode() => throw new NotImplementedException();

		readonly bool IConvertible.ToBoolean(IFormatProvider? provider) => ToBoolean();

		readonly byte IConvertible.ToByte(IFormatProvider? provider) => (byte)ToInt();

		readonly char IConvertible.ToChar(IFormatProvider? provider) => throw new NotImplementedException();

		readonly DateTime IConvertible.ToDateTime(IFormatProvider? provider) => throw new NotImplementedException();

		readonly decimal IConvertible.ToDecimal(IFormatProvider? provider) => ToDecimal();

		readonly double IConvertible.ToDouble(IFormatProvider? provider) => ToDouble();

		readonly short IConvertible.ToInt16(IFormatProvider? provider) => ToInt16();

		readonly int IConvertible.ToInt32(IFormatProvider? provider) => ToInt();

		readonly long IConvertible.ToInt64(IFormatProvider? provider) => ToInt64();

		readonly sbyte IConvertible.ToSByte(IFormatProvider? provider) => (sbyte)ToInt16();

		readonly float IConvertible.ToSingle(IFormatProvider? provider) => ToSingle();

		readonly string IConvertible.ToString(IFormatProvider? provider) => ToString();

		readonly ushort IConvertible.ToUInt16(IFormatProvider? provider) => (ushort)ToInt();

		readonly uint IConvertible.ToUInt32(IFormatProvider? provider) => (uint)ToInt64();

		readonly ulong IConvertible.ToUInt64(IFormatProvider? provider) => ulong.Parse(ToString(), CultureInfo.InvariantCulture);

		readonly object IConvertible.ToType(Type conversionType, IFormatProvider? provider) => (Type.GetTypeCode(conversionType)) switch
		{
			TypeCode.String => ToString(),
			TypeCode.Int16  => ToInt16(),
			TypeCode.Int32  => ToInt(),
			TypeCode.Int64  => ToInt64(),
			TypeCode.Single => ToSingle(),
			TypeCode.Double => ToDouble(),
			_				=> throw new Exception("No converter for " + conversionType.GetType().Name),
		};
		
		#endregion
	}
}