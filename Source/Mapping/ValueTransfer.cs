using System;
using System.Collections;
using System.Collections.Generic;

using BLToolkit.Common;

namespace BLToolkit.Mapping
{
	internal class ValueTransfer
	{
		public static Hashtable SameTypeTransferers = new Hashtable();

		public static Dictionary<KeyValuePair<Type,Type>,IValueTransferer> TypeTransferers =
			new Dictionary<KeyValuePair<Type,Type>, IValueTransferer>();

		static ValueTransfer()
		{
			SameTypeTransferers.Add(typeof(SByte),   new SByteToSByte());
			SameTypeTransferers.Add(typeof(Int16),   new Int16ToInt16());
			SameTypeTransferers.Add(typeof(Int32),   new Int32ToInt32());
			SameTypeTransferers.Add(typeof(Int64),   new Int64ToInt64());
			SameTypeTransferers.Add(typeof(Byte),    new ByteToByte());
			SameTypeTransferers.Add(typeof(UInt16),  new UInt16ToUInt16());
			SameTypeTransferers.Add(typeof(UInt32),  new UInt32ToUInt32());
			SameTypeTransferers.Add(typeof(UInt64),  new UInt64ToUInt64());
			SameTypeTransferers.Add(typeof(Boolean), new BooleanToBoolean());
			SameTypeTransferers.Add(typeof(Char),    new CharToChar());
			SameTypeTransferers.Add(typeof(Single),  new SingleToSingle());
			SameTypeTransferers.Add(typeof(Double),  new DoubleToDouble());
			SameTypeTransferers.Add(typeof(Decimal), new DecimalToDecimal());

			Add<SByte>  ();
			Add<Int16>  ();
			Add<Int32>  ();
			Add<Int64>  ();
			Add<Byte>   ();
			Add<UInt16> ();
			Add<UInt32> ();
			Add<UInt64> ();
			Add<Boolean>();
			Add<Char>   ();
			Add<Single> ();
			Add<Double> ();
			Add<Decimal>();
		}

		static void Add<S>()
		{
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(SByte)),   new ValueTransferer<S,SByte>  ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Int16)),   new ValueTransferer<S,Int16>  ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Int32)),   new ValueTransferer<S,Int32>  ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Int64)),   new ValueTransferer<S,Int64>  ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Byte)),    new ValueTransferer<S,Byte>   ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(UInt16)),  new ValueTransferer<S,UInt16> ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(UInt32)),  new ValueTransferer<S,UInt32> ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(UInt64)),  new ValueTransferer<S,UInt64> ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Boolean)), new ValueTransferer<S,Boolean>());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Char)),    new ValueTransferer<S,Char>   ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Single)),  new ValueTransferer<S,Single> ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Double)),  new ValueTransferer<S,Double> ());
			TypeTransferers.Add(new KeyValuePair<Type,Type>(typeof(S), typeof(Decimal)), new ValueTransferer<S,Decimal>());
		}

		#region Same Types

		class SByteToSByte : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSByte(destObject, destIndex, source.GetSByte(sourceObject, sourceIndex));
			}
		}

		class Int16ToInt16 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt16(destObject, destIndex, source.GetInt16(sourceObject, sourceIndex));
			}
		}

		class Int32ToInt32 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt32(destObject, destIndex, source.GetInt32(sourceObject, sourceIndex));
			}
		}

		class Int64ToInt64 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt64(destObject, destIndex, source.GetInt64(sourceObject, sourceIndex));
			}
		}

		class ByteToByte : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetByte(destObject, destIndex, source.GetByte(sourceObject, sourceIndex));
			}
		}

		class UInt16ToUInt16 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt16(destObject, destIndex, source.GetUInt16(sourceObject, sourceIndex));
			}
		}

		class UInt32ToUInt32 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt32(destObject, destIndex, source.GetUInt32(sourceObject, sourceIndex));
			}
		}

		class UInt64ToUInt64 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt64(destObject, destIndex, source.GetUInt64(sourceObject, sourceIndex));
			}
		}

		class BooleanToBoolean : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetBoolean(destObject, destIndex, source.GetBoolean(sourceObject, sourceIndex));
			}
		}

		class CharToChar : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetChar(destObject, destIndex, source.GetChar(sourceObject, sourceIndex));
			}
		}

		class SingleToSingle : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSingle(destObject, destIndex, source.GetSingle(sourceObject, sourceIndex));
			}
		}

		class DoubleToDouble : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDouble(destObject, destIndex, source.GetDouble(sourceObject, sourceIndex));
			}
		}

		class DecimalToDecimal : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDecimal(destObject, destIndex, source.GetDecimal(sourceObject, sourceIndex));
			}
		}

		#endregion

		#region Different Types

		class DecimalToInt32 : IValueTransferer
		{
			public void Transfer(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt32(destObject, destIndex, ConvertTo<Int32>.From(source.GetDecimal(sourceObject, sourceIndex)));
			}
		}

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

				throw new InvalidOperationException();
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

				if (t == typeof(SByte))   return Conv<SByte>  (delegate(IMapDataDestination d, object o, int i,SByte   v) { d.SetSByte  (o, i, v); });
				if (t == typeof(Int16))   return Conv<Int16>  (delegate(IMapDataDestination d, object o, int i,Int16   v) { d.SetInt16  (o, i, v); });
				if (t == typeof(Int32))   return Conv<Int32>  (delegate(IMapDataDestination d, object o, int i,Int32   v) { d.SetInt32  (o, i, v); });
				if (t == typeof(Int64))   return Conv<Int64>  (delegate(IMapDataDestination d, object o, int i,Int64   v) { d.SetInt64  (o, i, v); });
				if (t == typeof(Byte))    return Conv<Byte>   (delegate(IMapDataDestination d, object o, int i,Byte    v) { d.SetByte   (o, i, v); });
				if (t == typeof(UInt16))  return Conv<UInt16> (delegate(IMapDataDestination d, object o, int i,UInt16  v) { d.SetUInt16 (o, i, v); });
				if (t == typeof(UInt32))  return Conv<UInt32> (delegate(IMapDataDestination d, object o, int i,UInt32  v) { d.SetUInt32 (o, i, v); });
				if (t == typeof(UInt64))  return Conv<UInt64> (delegate(IMapDataDestination d, object o, int i,UInt64  v) { d.SetUInt64 (o, i, v); });

				if (t == typeof(Boolean)) return Conv<Boolean>(delegate(IMapDataDestination d, object o, int i,Boolean v) { d.SetBoolean(o, i, v); });
				if (t == typeof(Char))    return Conv<Char>   (delegate(IMapDataDestination d, object o, int i,Char    v) { d.SetChar   (o, i, v); });
				if (t == typeof(Single))  return Conv<Single> (delegate(IMapDataDestination d, object o, int i,Single  v) { d.SetSingle (o, i, v); });
				if (t == typeof(Double))  return Conv<Double> (delegate(IMapDataDestination d, object o, int i,Double  v) { d.SetDouble (o, i, v); });
				if (t == typeof(Decimal)) return Conv<Decimal>(delegate(IMapDataDestination d, object o, int i,Decimal v) { d.SetDecimal(o, i, v); });

				throw new InvalidOperationException();
			}
		}

		class ValueTransferer<S,D> : IValueTransferer
		{
			public void Transfer(
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
