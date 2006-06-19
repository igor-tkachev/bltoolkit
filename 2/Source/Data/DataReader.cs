using System;
using System.Data;
using BLToolkit.Common;

namespace BLToolkit.Data
{
	public static class DataReader<T>
	{
		public delegate T GetMethod(IDataReader dr, int index);

		public static GetMethod Conv<T1>(DataReader<T1>.GetMethod m)
		{
			return (GetMethod)(object)m;
		}

		public static GetMethod Get = SelectMethod(typeof(T));
		public static GetMethod SelectMethod(Type type)
		{
			// Base data types
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					return Conv<Boolean> (delegate(IDataReader dr, int index) { return         dr.GetBoolean(index);  });
				case TypeCode.Byte:
					return Conv<Byte>    (delegate(IDataReader dr, int index) { return         dr.GetByte(index);     });
				case TypeCode.Char:
					return Conv<Char>    (delegate(IDataReader dr, int index) { return         dr.GetChar(index);     });
				case TypeCode.DateTime:
					return Conv<DateTime>(delegate(IDataReader dr, int index) { return         dr.GetDateTime(index); });
				case TypeCode.Decimal:
					return Conv<Decimal> (delegate(IDataReader dr, int index) { return         dr.GetDecimal(index);  });
				case TypeCode.Double:
					return Conv<Double>  (delegate(IDataReader dr, int index) { return         dr.GetDouble(index);   });
				case TypeCode.Int16:
					return Conv<Int16>   (delegate(IDataReader dr, int index) { return         dr.GetInt16(index);    });
				case TypeCode.Int32:
					return Conv<Int32>   (delegate(IDataReader dr, int index) { return         dr.GetInt32(index);    });
				case TypeCode.Int64:
					return Conv<Int64>   (delegate(IDataReader dr, int index) { return         dr.GetInt64(index);    });
				case TypeCode.SByte:
					return Conv<SByte>   (delegate(IDataReader dr, int index) { return  (SByte)dr.GetByte(index);     });
				case TypeCode.Single:
					return Conv<Single>  (delegate(IDataReader dr, int index) { return         dr.GetFloat(index);    });
				case TypeCode.String:
					return Conv<String>  (delegate(IDataReader dr, int index) { return         dr.IsDBNull(index) ? null : dr.GetString(index);   });
				case TypeCode.UInt16:
					return Conv<UInt16>  (delegate(IDataReader dr, int index) { return (UInt16)dr.GetInt16(index);    });
				case TypeCode.UInt32:
					return Conv<UInt32>  (delegate(IDataReader dr, int index) { return (UInt32)dr.GetInt32(index);    });
				case TypeCode.UInt64:
					return Conv<UInt64>  (delegate(IDataReader dr, int index) { return (UInt64)dr.GetInt64(index);    });
			}

			if (type == typeof(Guid))
					return Conv<Guid>    (delegate(IDataReader dr, int index) { return         dr.GetGuid(index);     });

			// Nullable types
			if ((type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)))
			{
				switch (Type.GetTypeCode(type.GetGenericArguments()[0]))
				{
					case TypeCode.Boolean:
						return Conv<Boolean?> (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Boolean?)null  :         dr.GetBoolean(index);  });
					case TypeCode.Byte:
						return Conv<Byte?>    (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Byte?)null     :         dr.GetByte(index);     });
					case TypeCode.Char:
						return Conv<Char?>    (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Char?)null     :         dr.GetChar(index);     });
					case TypeCode.DateTime:
						return Conv<DateTime?>(delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (DateTime?)null :         dr.GetDateTime(index); });
					case TypeCode.Decimal:
						return Conv<Decimal?> (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Decimal?)null  :         dr.GetDecimal(index);  });
					case TypeCode.Double:
						return Conv<Double?>  (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Double?)null   :         dr.GetDouble(index);   });
					case TypeCode.Int16:
						return Conv<Int16?>   (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Int16?)null    :         dr.GetInt16(index);    });
					case TypeCode.Int32:
						return Conv<Int32?>   (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Int32?)null    :         dr.GetInt32(index);    });
					case TypeCode.Int64:
						return Conv<Int64?>   (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Int64?)null    :         dr.GetInt64(index);    });
					case TypeCode.SByte:
						return Conv<SByte?>   (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (SByte?)null    :  (SByte)dr.GetByte(index);     });
					case TypeCode.Single:
						return Conv<Single?>  (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (float?)null    :         dr.GetFloat(index);    });
					case TypeCode.UInt16:
						return Conv<UInt16?>  (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (UInt16?)null   : (UInt16)dr.GetInt16(index);    });
					case TypeCode.UInt32:
						return Conv<UInt32?>  (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (UInt32?)null   : (UInt32)dr.GetInt32(index);    });
					case TypeCode.UInt64:
						return Conv<UInt64?>  (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (UInt64?)null   : (UInt64)dr.GetInt64(index);    });
				}

				if (type == typeof(Guid?))
						return Conv<Guid?>    (delegate(IDataReader dr, int index)
							{ return dr.IsDBNull(index) ? (Guid?)null     :         dr.GetGuid(index);     });
			}

			return DefaultMethod;
		}

		public static T DefaultMethod(IDataReader dr, int index)
		{
			return ConvertTo<T>.From(dr.GetValue(index));
		}
	}
}
