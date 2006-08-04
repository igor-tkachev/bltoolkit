using System;

using Convert=BLToolkit.Common.Convert;

namespace BLToolkit.Mapping.ValueMappingInternal
{
	#region SByte

	internal sealed class I8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					source.GetSByte(sourceObject, sourceIndex));
		}
	}

	internal sealed class I16ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSByte(destObject, destIndex,
					Convert.ToSByte(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Int16

	internal sealed class I8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					source.GetInt16(sourceObject, sourceIndex));
		}
	}

	internal sealed class I32ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt16(destObject, destIndex,
					Convert.ToInt16(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Int32

	internal sealed class I8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					source.GetInt32(sourceObject, sourceIndex));
		}
	}

	internal sealed class I64ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt32(destObject, destIndex,
					Convert.ToInt32(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Int64

	internal sealed class I8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					source.GetInt64(sourceObject, sourceIndex));
		}
	}


	internal sealed class U8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTToI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetInt64(destObject, destIndex,
					Convert.ToInt64(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}

	#endregion 


	#region Byte

	internal sealed class I8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					source.GetByte(sourceObject, sourceIndex));
		}
	}

	internal sealed class U16ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}
#if FW2

	internal sealed class NI8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetByte(destObject, destIndex,
					Convert.ToByte(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}
	#endregion 

	#region UInt16

	internal sealed class I8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					source.GetUInt16(sourceObject, sourceIndex));
		}
	}

	internal sealed class U32ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}
#if FW2

	internal sealed class NI8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt16(destObject, destIndex,
					Convert.ToUInt16(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region UInt32

	internal sealed class I8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					source.GetUInt32(sourceObject, sourceIndex));
		}
	}

	internal sealed class U64ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}
#if FW2

	internal sealed class NI8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt32(destObject, destIndex,
					Convert.ToUInt32(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region UInt64

	internal sealed class I8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					source.GetUInt64(sourceObject, sourceIndex));
		}
	}


	internal sealed class BToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}
#if FW2

	internal sealed class NI8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetUInt64(destObject, destIndex,
					Convert.ToUInt64(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 


	#region Boolean

	internal sealed class I8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					source.GetBoolean(sourceObject, sourceIndex));
		}
	}

	internal sealed class CToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetBoolean(destObject, destIndex,
					Convert.ToBoolean(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Char

	internal sealed class I8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					source.GetChar(sourceObject, sourceIndex));
		}
	}

	internal sealed class R4ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetChar(destObject, destIndex,
					Convert.ToChar(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Single

	internal sealed class I8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					source.GetSingle(sourceObject, sourceIndex));
		}
	}

	internal sealed class R8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}
#if FW2

	internal sealed class NI8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSingle(destObject, destIndex,
					Convert.ToSingle(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Double

	internal sealed class I8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					source.GetDouble(sourceObject, sourceIndex));
		}
	}

	internal sealed class DToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}

#endif
	internal sealed class dbSToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTToR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDouble(destObject, destIndex,
					Convert.ToDouble(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Decimal

	internal sealed class I8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					source.GetDecimal(sourceObject, sourceIndex));
		}
	}

#if FW2
	internal sealed class NI8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDecimal(destObject, destIndex,
					Convert.ToDecimal(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 


	#region Guid

	internal sealed class GToG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetGuid(destObject, destIndex,
					source.GetGuid(sourceObject, sourceIndex));
		}
	}

#if FW2
	internal sealed class NGToG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetGuid(destObject, destIndex,
					Convert.ToGuid(
						source.GetNullableGuid(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetGuid(destObject, destIndex,
					Convert.ToGuid(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbGToG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetGuid(destObject, destIndex,
					Convert.ToGuid(
						source.GetSqlGuid(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region DateTime

	internal sealed class I64ToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					source.GetDateTime(sourceObject, sourceIndex));
		}
	}

#if FW2
	internal sealed class NI64ToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTToDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetDateTime(destObject, destIndex,
					Convert.ToDateTime(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}

	#endregion 
}