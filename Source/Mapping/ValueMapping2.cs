using System;
using System.Collections.Generic;

using BLToolkit.Common;

namespace BLToolkit.Mapping
{
	public static class ValueMapping
	{
		private static Dictionary<KeyValuePair<Type,Type>,IValueMapper> _mappers =
			new Dictionary<KeyValuePair<Type,Type>, IValueMapper>();

		#region Default Mapper

		class DefaultValueMapper : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				dest.SetValue(destObject, destIndex, source.GetValue(sourceObject, sourceIndex));
			}
		}

		private static IValueMapper _defaultMapper = new DefaultValueMapper();
		[CLSCompliant(false)]
		public  static IValueMapper  DefaultMapper
		{
			get { return _defaultMapper;  }
			set { _defaultMapper = value; }
		}

		#endregion

		#region GetMapper

		private static object _sync = new object();

		[CLSCompliant(false)]
		public static IValueMapper GetMapper(Type t1, Type t2)
		{
			lock (_sync)
			{
				if (t1 == null) t1 = typeof(object);
				if (t2 == null) t2 = typeof(object);

				KeyValuePair<Type,Type> key = new KeyValuePair<Type,Type>(t1, t2);

				IValueMapper t;

				if (_mappers.TryGetValue(key, out t))
					return t;

				Type type = typeof(GetSetDataChecker<,>).MakeGenericType(t1, t2);

				if (((IGetSetDataChecker)Activator.CreateInstance(type)).Check() == false)
				{
					t = _defaultMapper;
				}
				else
				{
					type = t1 == t2?
						typeof(ValueMapper<>). MakeGenericType(t1):
						typeof(ValueMapper<,>).MakeGenericType(t1, t2);

					t = (IValueMapper)Activator.CreateInstance(type);
				}

				_mappers.Add(key, t);

				return t;
			}
		}

		#endregion

		#region Mappers

		static class GetData<T>
		{
			public delegate T GetMethod(IMapDataSource s, object o, int index);

			private static GetMethod Conv<T1>(GetData<T1>.GetMethod op)
			{
				return (GetMethod)(object)op;
			}

			public  static GetMethod Get = GetGetter();
			private static GetMethod GetGetter()
			{
				Type t = typeof(T);

				if (t == typeof(SByte))   return Conv<SByte>  (delegate(IMapDataSource s, object o, int i) { return s.GetSByte  (o, i); });
				if (t == typeof(Int16))   return Conv<Int16>  (delegate(IMapDataSource s, object o, int i) { return s.GetInt16  (o, i); });
				if (t == typeof(Int32))   return Conv<Int32>  (delegate(IMapDataSource s, object o, int i) { return s.GetInt32  (o, i); });
				if (t == typeof(Int64))   return Conv<Int64>  (delegate(IMapDataSource s, object o, int i) { return s.GetInt64  (o, i); });
				if (t == typeof(Byte))    return Conv<Byte>   (delegate(IMapDataSource s, object o, int i) { return s.GetByte   (o, i); });
				if (t == typeof(UInt16))  return Conv<UInt16> (delegate(IMapDataSource s, object o, int i) { return s.GetUInt16 (o, i); });
				if (t == typeof(UInt32))  return Conv<UInt32> (delegate(IMapDataSource s, object o, int i) { return s.GetUInt32 (o, i); });
				if (t == typeof(UInt64))  return Conv<UInt64> (delegate(IMapDataSource s, object o, int i) { return s.GetUInt64 (o, i); });

				if (t == typeof(Boolean)) return Conv<Boolean>(delegate(IMapDataSource s, object o, int i) { return s.GetBoolean(o, i); });
				if (t == typeof(Char))    return Conv<Char>   (delegate(IMapDataSource s, object o, int i) { return s.GetChar   (o, i); });
				if (t == typeof(Single))  return Conv<Single> (delegate(IMapDataSource s, object o, int i) { return s.GetSingle (o, i); });
				if (t == typeof(Double))  return Conv<Double> (delegate(IMapDataSource s, object o, int i) { return s.GetDouble (o, i); });
				if (t == typeof(Decimal)) return Conv<Decimal>(delegate(IMapDataSource s, object o, int i) { return s.GetDecimal(o, i); });

				return null;
			}
		}

		static class SetData<T>
		{
			public delegate void SetMethod(IMapDataDestination d, object o, int index, T value);

			private static SetMethod Conv<T1>(SetData<T1>.SetMethod op)
			{
				return (SetMethod)(object)op;
			}

			public  static SetMethod Set = GetSetter();
			private static SetMethod GetSetter()
			{
				Type t = typeof(T);

				if (t == typeof(SByte))   return Conv<SByte>  (delegate(IMapDataDestination d, object o, int i, SByte   v) { d.SetSByte  (o, i, v); });
				if (t == typeof(Int16))   return Conv<Int16>  (delegate(IMapDataDestination d, object o, int i, Int16   v) { d.SetInt16  (o, i, v); });
				if (t == typeof(Int32))   return Conv<Int32>  (delegate(IMapDataDestination d, object o, int i, Int32   v) { d.SetInt32  (o, i, v); });
				if (t == typeof(Int64))   return Conv<Int64>  (delegate(IMapDataDestination d, object o, int i, Int64   v) { d.SetInt64  (o, i, v); });
				if (t == typeof(Byte))    return Conv<Byte>   (delegate(IMapDataDestination d, object o, int i, Byte    v) { d.SetByte   (o, i, v); });
				if (t == typeof(UInt16))  return Conv<UInt16> (delegate(IMapDataDestination d, object o, int i, UInt16  v) { d.SetUInt16 (o, i, v); });
				if (t == typeof(UInt32))  return Conv<UInt32> (delegate(IMapDataDestination d, object o, int i, UInt32  v) { d.SetUInt32 (o, i, v); });
				if (t == typeof(UInt64))  return Conv<UInt64> (delegate(IMapDataDestination d, object o, int i, UInt64  v) { d.SetUInt64 (o, i, v); });

				if (t == typeof(Boolean)) return Conv<Boolean>(delegate(IMapDataDestination d, object o, int i, Boolean v) { d.SetBoolean(o, i, v); });
				if (t == typeof(Char))    return Conv<Char>   (delegate(IMapDataDestination d, object o, int i, Char    v) { d.SetChar   (o, i, v); });
				if (t == typeof(Single))  return Conv<Single> (delegate(IMapDataDestination d, object o, int i, Single  v) { d.SetSingle (o, i, v); });
				if (t == typeof(Double))  return Conv<Double> (delegate(IMapDataDestination d, object o, int i, Double  v) { d.SetDouble (o, i, v); });
				if (t == typeof(Decimal)) return Conv<Decimal>(delegate(IMapDataDestination d, object o, int i, Decimal v) { d.SetDecimal(o, i, v); });

				return null;
			}
		}

		interface IGetSetDataChecker
		{
			bool Check();
		}

		class GetSetDataChecker<S,D> : IGetSetDataChecker
		{
			public bool Check()
			{
				return
					GetData<S>.Get != null && SetData<S>.Set != null &&
					GetData<D>.Get != null && SetData<D>.Set != null;
			}
		}

		class ValueMapper<T> : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					SetData<T>.Set(
						dest, destObject, destIndex,
						GetData<T>.Get(source, sourceObject, sourceIndex));
			}
		}

		class ValueMapper<S,D> : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					SetData<D>.Set(
						dest, destObject, destIndex,
						ConvertTo<D>.From(GetData<S>.Get(source, sourceObject, sourceIndex)));
			}
		}

		#endregion
	}
}
