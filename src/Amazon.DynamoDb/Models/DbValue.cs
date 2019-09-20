using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public readonly struct DbValue : IConvertible
	{
		public static readonly DbValue Empty = new DbValue(string.Empty, DbValueType.Unknown);
		public static readonly DbValue Null	 = new DbValue(string.Empty, DbValueType.NULL);
		public static readonly DbValue True = new DbValue(true);
		public static readonly DbValue False = new DbValue(false);

		private readonly DbValueType kind;
		private readonly object value;

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
            this.kind = DbValueType.M;
            this.value = map;
        }

		public DbValue(object value, DbValueType type)
		{
			this.value = value;
			this.kind = type;
		}

		public DbValue(object value)
		{
            var type = value.GetType();

			if (type.IsEnum)
			{
				this.kind = DbValueType.N;
				this.value = EnumConverter.Default.FromObject(value, null!).Value;
 
				return;
			}

			if (type.IsArray)
			{
				var elementType = type.GetElementType();

				switch (Type.GetTypeCode(elementType))
				{
					case TypeCode.Byte:			this.kind = DbValueType.B;		break;

					case TypeCode.String:		this.kind = DbValueType.SS;		break;

					case TypeCode.Decimal:
					case TypeCode.Double:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:		this.kind = DbValueType.NS;		break;

					default: throw new Exception("Invalid array element type:" + type.Name);
				}
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

						this.kind = DbValueType.S;

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
					case TypeCode.UInt64: this.kind = DbValueType.N; break;

					case TypeCode.DateTime:
                        // Convert dates to unixtime

                        this.value = new DateTimeOffset((DateTime)value).ToUnixTimeSeconds();
						this.kind = DbValueType.N;

						return;

					// Boolean
					case TypeCode.Boolean:
						this.value = (bool)value ? 1 : 0;
						this.kind = DbValueType.N;

						return;

					default: {
						if (type == typeof(DbValue)) {
							var dbValue = (DbValue)value;

							this.kind = dbValue.Kind;
							this.value = dbValue.Value;

						}
                        else if (type == typeof(AttributeCollection))
                        {
                           this.value = value;
                           this.kind = DbValueType.M;        
                        }
						else
						{
                            if (!DbValueConverterFactory.TryGet(type, out IDbValueConverter converter))
                            {
                                throw new Exception("Invalid value type. Was: " + type.Name);
                            }

                            var result = converter.FromObject(value, null!);

                            this.value = result.Value;
                            this.kind = result.Kind;                            
                        }

                        break;
					}
				}
			}
			

			this.value = value;
		}

		public DbValueType Kind => kind;

		/// <summary>
		/// int, long, string, string [ ]
		/// </summary>
		public object Value => value;

		internal object ToPrimitiveValue()
		{
			switch (kind)
			{
				case DbValueType.B : return ToBinary();
				case DbValueType.N : return ToInt64();	// TODO, return a double if there's a floating point
				case DbValueType.S : return ToString();
				default			   : throw new Exception($"Cannot convert {kind} to native value.");
			}
		}

		#region To Helpers

		public HashSet<string> ToStringSet()
		{
			if (!(kind == DbValueType.NS || kind == DbValueType.SS || kind == DbValueType.BS)) {
				throw new Exception("Cannot be converted to a set.");
			}

			if (value is IEnumerable<string> enumerable)
			{
				return new HashSet<string>(enumerable);		
			}

			var set = new HashSet<string>();

			foreach (var item in (IEnumerable)value)
			{
				set.Add(item.ToString());
			}

			return set;
		}

		public HashSet<T> ToSet<T>()
		{
			if (!(kind == DbValueType.NS || kind == DbValueType.SS || kind == DbValueType.BS))
			{
				throw new Exception($"The value type '{kind}' cannot be converted to a Int32 Set.");
			}

			// Avoid additional allocations where possible
			if (value is IEnumerable<T> enumerable)
			{
				return new HashSet<T>(enumerable);
			}

			var set = new HashSet<T>();

			foreach (var item in (IEnumerable)value)
			{
				set.Add((T)Convert.ChangeType(item, typeof(T)));
			}

			return set;
		}

		public T[] ToArray<T>()
		{
			var type = value.GetType();
		
			if (type.IsArray && type.GetElementType() == typeof(T))
			{
				return (T[])value;
			}

            IList collection = (IList)value;
            
			T[] array = new T[collection.Count];
			
			for (int i = 0; i < collection.Count; i++)
			{
				array[i] = (T)Convert.ChangeType(collection[i], typeof(T));
			}

			return array;
		}

		public bool ToBoolean()
		{
            if (kind == DbValueType.N)
            {
                return ToInt() == 1;
            }

            if (value is bool b)
            {
                return b;
            }

            throw new Exception($"Cannot convert '{value.GetType().Name}' to a Boolean");
		}

		public float ToSingle() => Convert.ToSingle(value);

		public short ToInt16() => Convert.ToInt16(value);

		public int ToInt() => Convert.ToInt32(value);

		public long ToInt64() => Convert.ToInt64(value);

		public double ToDouble() => Convert.ToDouble(value);

		public decimal ToDecimal() => Convert.ToDecimal(value);

		public byte[] ToBinary()
		{
            return value is byte[] data
                ? data
                : Convert.FromBase64String(value.ToString());
		}

		public override string ToString() => value.ToString();

		#endregion

		public JsonObject ToJson()
		{
			// {"N":"225"}
			// {"S":"Hello"}
			// {"SS": ["Keneau", "Alexis", "John"]}
			// {"NS": ["1", "2", "3"]}

			JsonNode node;

            if (kind == DbValueType.M)
            {
                node = ((AttributeCollection)value).ToJson();
            }
			else if (kind == DbValueType.B && value is byte[] data)
			{
				node = new JsonString(Convert.ToBase64String(data));
			}
			else if (kind == DbValueType.L)
			{
				var list = new JsonNodeList();

                foreach (var item in (IEnumerable<DbValue>)value)
                {
                    list.Add(item.ToJson());
                }

				node = list;
			}
			else if (value.GetType().IsArray)
			{
				var elementType = value.GetType().GetElementType();

				if (elementType == typeof(string))
				{
					node = new XImmutableArray<string>((string[])value);
				}
				else
				{
					var list = new List<string>();

					foreach (var item in (IEnumerable)value)
					{
						list.Add(item.ToString());
					}

					node = new XList<string>(list);
				}
			}
			else if (kind == DbValueType.BOOL)
			{
                var val = (bool)value;

                node = val ? JsonBoolean.True : JsonBoolean.False;
			}
			else
			{
				node = new JsonString(value.ToString());
			}

            return new JsonObject {
                { kind.ToQuickString(), node }
            };
		}

		public static DbValue FromValue(JsonNode value) => value.Type switch
		{
			JsonType.Binary  => new DbValue(((XBinary)value).Value,	    DbValueType.B),		// byte[]
			JsonType.Boolean => new DbValue(((JsonBoolean)value).Value,	DbValueType.BOOL),	// bool
			JsonType.Number  => new DbValue(value.ToString(),			DbValueType.N),
			JsonType.String  => new DbValue((string)value,			    DbValueType.S),
			_                => throw new Exception("Invalid value type:" + value.Type)
		};

		public static DbValue FromJson(JsonObject json)
		{
			// {"N":"225"}
			// {"S":"Hello"}
			// {"B":"dmFsdWU="}
			// {"SS": ["Keneau", "Alexis", "John"]}

			// { "L": [ { "N": "1" }, { "N":"2" } ]

			var property = json.First();

			return property.Key switch
			{
				"B"	   => new DbValue(property.Value.ToString(), DbValueType.B),
				"N"	   => new DbValue(property.Value.ToString(), DbValueType.N),
				"S"	   => new DbValue(property.Value.ToString(), DbValueType.S),
				"BOOL" => new DbValue((bool)property.Value,      DbValueType.BOOL),
				"BS"   => new DbValue(((JsonArray)property.Value).ToArrayOf<string>(), DbValueType.BS),
				"NS"   => new DbValue(((JsonArray)property.Value).ToArrayOf<string>(), DbValueType.NS),
				"SS"   => new DbValue(((JsonArray)property.Value).ToArrayOf<string>(), DbValueType.SS),
				"L"	   => new DbValue(GetListValues((JsonArray)property.Value).ToArray()),
                "M"    => new DbValue(AttributeCollection.FromJson((JsonObject)property.Value)),
				_      => throw new Exception("Invalid value type:" + property.Key),
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

		private static IEnumerable<DbValue> GetListValues(JsonArray array)
		{
			foreach (var item in array)
			{
				yield return FromJson((JsonObject)item);
			}
		}

		#endregion

		#region IConvertible

		TypeCode IConvertible.GetTypeCode() => throw new NotImplementedException();
		
        bool IConvertible.ToBoolean(IFormatProvider provider) => ToBoolean();

		byte IConvertible.ToByte(IFormatProvider provider) => (byte)ToInt();

        char IConvertible.ToChar(IFormatProvider provider) => throw new NotImplementedException();
		
		DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new NotImplementedException();
		
        decimal IConvertible.ToDecimal(IFormatProvider provider) => ToDecimal();

        double IConvertible.ToDouble(IFormatProvider provider) => ToDouble();

        short IConvertible.ToInt16(IFormatProvider provider) => ToInt16();

        int IConvertible.ToInt32(IFormatProvider provider) => ToInt();

        long IConvertible.ToInt64(IFormatProvider provider) => ToInt64();

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

        float IConvertible.ToSingle(IFormatProvider provider) => ToSingle();

        string IConvertible.ToString(IFormatProvider provider) => ToString();

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			switch (Type.GetTypeCode(conversionType))
			{
				case TypeCode.String : return ToString();
				case TypeCode.Int16	 : return ToInt16();
				case TypeCode.Int32	 : return ToInt();
				case TypeCode.Int64	 : return ToInt64();
				case TypeCode.Single : return ToSingle();
				case TypeCode.Double : return ToDouble();
				default				 : throw new Exception("No convertor for " + conversionType.GetType().Name);
			}
		}

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)ToInt();

		uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)ToInt64();

        ulong IConvertible.ToUInt64(IFormatProvider provider) => ulong.Parse(ToString());

		#endregion
	}
}