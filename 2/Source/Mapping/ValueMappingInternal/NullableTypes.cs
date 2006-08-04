using System;

using Convert=BLToolkit.Common.Convert;

namespace BLToolkit.Mapping.ValueMappingInternal
{
	#region SByte

	internal sealed class I8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					source.GetNullableSByte(sourceObject, sourceIndex));
		}
	}

	internal sealed class NI16ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNI8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSByte(destObject, destIndex,
					Convert.ToNullableSByte(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Int16

	internal sealed class I8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					source.GetNullableInt16(sourceObject, sourceIndex));
		}
	}

	internal sealed class NI32ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt16(destObject, destIndex,
					Convert.ToNullableInt16(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Int32

	internal sealed class I8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					source.GetNullableInt32(sourceObject, sourceIndex));
		}
	}

	internal sealed class NI64ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt32(destObject, destIndex,
					Convert.ToNullableInt32(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Int64

	internal sealed class I8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					source.GetNullableInt64(sourceObject, sourceIndex));
		}
	}


	internal sealed class NU8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbSToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTToNI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableInt64(destObject, destIndex,
					Convert.ToNullableInt64(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}


	#endregion 


	#region Byte

	internal sealed class I8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					source.GetNullableByte(sourceObject, sourceIndex));
		}
	}

	internal sealed class NU16ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableByte(destObject, destIndex,
					Convert.ToNullableByte(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region UInt16

	internal sealed class I8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					source.GetNullableUInt16(sourceObject, sourceIndex));
		}
	}

	internal sealed class NU32ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNU16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt16(destObject, destIndex,
					Convert.ToNullableUInt16(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region UInt32

	internal sealed class I8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					source.GetNullableUInt32(sourceObject, sourceIndex));
		}
	}

	internal sealed class NU64ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNU32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt32(destObject, destIndex,
					Convert.ToNullableUInt32(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region UInt64

	internal sealed class I8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					source.GetNullableUInt64(sourceObject, sourceIndex));
		}
	}


	internal sealed class NBToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNU64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableUInt64(destObject, destIndex,
					Convert.ToNullableUInt64(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 


	#region Boolean

	internal sealed class I8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					source.GetNullableBoolean(sourceObject, sourceIndex));
		}
	}

	internal sealed class NCToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableBoolean(destObject, destIndex,
					Convert.ToNullableBoolean(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Char

	internal sealed class I8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					source.GetNullableChar(sourceObject, sourceIndex));
		}
	}

	internal sealed class NR4ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNC : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableChar(destObject, destIndex,
					Convert.ToNullableChar(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Single

	internal sealed class I8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					source.GetNullableSingle(sourceObject, sourceIndex));
		}
	}

	internal sealed class NR8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbSToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableSingle(destObject, destIndex,
					Convert.ToNullableSingle(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region Double

	internal sealed class I8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					source.GetNullableDouble(sourceObject, sourceIndex));
		}
	}

	internal sealed class NDToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbSToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTToNR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDouble(destObject, destIndex,
					Convert.ToNullableDouble(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Decimal

	internal sealed class I8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}



	internal sealed class NI8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					source.GetNullableDecimal(sourceObject, sourceIndex));
		}
	}



	internal sealed class dbSToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBToND : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDecimal(destObject, destIndex,
					Convert.ToNullableDecimal(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region Guid

	internal sealed class NGToNG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableGuid(destObject, destIndex,
					source.GetNullableGuid(sourceObject, sourceIndex));
		}
	}

	internal sealed class GToNG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableGuid(destObject, destIndex,
					Convert.ToNullableGuid(
						source.GetGuid(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbSToNG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableGuid(destObject, destIndex,
					Convert.ToNullableGuid(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbGToNG : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableGuid(destObject, destIndex,
					Convert.ToNullableGuid(
						source.GetSqlGuid(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region DateTime

	internal sealed class I64ToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8ToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					source.GetNullableDateTime(sourceObject, sourceIndex));
		}
	}

	internal sealed class NI64ToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8ToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbSToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64ToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8ToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTToNDT : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetNullableDateTime(destObject, destIndex,
					Convert.ToNullableDateTime(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}

	#endregion 
}