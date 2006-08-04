using System;

using Convert=BLToolkit.Common.Convert;

namespace BLToolkit.Mapping.ValueMappingInternal
{
	#region SqlString

	internal sealed class I8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					source.GetSqlString(sourceObject, sourceIndex));
		}
	}


	internal sealed class dbU8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbS : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlString(destObject, destIndex,
					Convert.ToSqlString(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 


	#region SqlByte

	internal sealed class I8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					source.GetSqlByte(sourceObject, sourceIndex));
		}
	}

	internal sealed class dbI16TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbU8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlByte(destObject, destIndex,
					Convert.ToSqlByte(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region SqlInt16

	internal sealed class I8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					source.GetSqlInt16(sourceObject, sourceIndex));
		}
	}

	internal sealed class dbI32TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbI16 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt16(destObject, destIndex,
					Convert.ToSqlInt16(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region SqlInt32

	internal sealed class I8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					source.GetSqlInt32(sourceObject, sourceIndex));
		}
	}

	internal sealed class dbI64TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbI32 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt32(destObject, destIndex,
					Convert.ToSqlInt32(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region SqlInt64

	internal sealed class I8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					source.GetSqlInt64(sourceObject, sourceIndex));
		}
	}


	internal sealed class dbR4TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTTodbI64 : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlInt64(destObject, destIndex,
					Convert.ToSqlInt64(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}

	#endregion 


	#region SqlSingle

	internal sealed class I8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					source.GetSqlSingle(sourceObject, sourceIndex));
		}
	}

	internal sealed class dbR8TodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbR4 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlSingle(destObject, destIndex,
					Convert.ToSqlSingle(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region SqlDouble

	internal sealed class I8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NI8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					source.GetSqlDouble(sourceObject, sourceIndex));
		}
	}

	internal sealed class dbDTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTTodbR8 : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDouble(destObject, destIndex,
					Convert.ToSqlDouble(
						source.GetSqlDateTime(sourceObject, sourceIndex)));
		}
	}

	#endregion 

	#region SqlDecimal

	internal sealed class I8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					source.GetSqlDecimal(sourceObject, sourceIndex));
		}
	}

	internal sealed class dbMTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbD : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDecimal(destObject, destIndex,
					Convert.ToSqlDecimal(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 

	#region SqlMoney

	internal sealed class I8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					source.GetSqlMoney(sourceObject, sourceIndex));
		}
	}


	internal sealed class dbBTodbM : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlMoney(destObject, destIndex,
					Convert.ToSqlMoney(
						source.GetSqlBoolean(sourceObject, sourceIndex)));
		}
	}


	#endregion 


	#region SqlBoolean

	internal sealed class I8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I16TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I32TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class I64TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class U8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U16TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U32TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class U64TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class BTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class CTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R4TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetDecimal(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableSByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI16TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI32TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NI64TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NU8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU16TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableUInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU32TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableUInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NU64TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableUInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class NBTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableBoolean(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NCTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableChar(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR4TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetNullableDecimal(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbU8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlByte(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI16TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlInt16(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI32TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlInt32(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbI64TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR4TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlSingle(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbR8TodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbDTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlDecimal(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbMTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					Convert.ToSqlBoolean(
						source.GetSqlMoney(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbBTodbB : IValueMapper
	{
		public void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlBoolean(destObject, destIndex,
					source.GetSqlBoolean(sourceObject, sourceIndex));
		}
	}

	#endregion 

	#region SqlGuid

	internal sealed class GTodbG : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlGuid(destObject, destIndex,
					Convert.ToSqlGuid(
						source.GetGuid(sourceObject, sourceIndex)));
		}
	}

#if FW2
	internal sealed class NGTodbG : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlGuid(destObject, destIndex,
					Convert.ToSqlGuid(
						source.GetNullableGuid(sourceObject, sourceIndex)));
		}
	}
#endif

	internal sealed class dbSTodbG : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlGuid(destObject, destIndex,
					Convert.ToSqlGuid(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}

	internal sealed class dbGTodbG : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlGuid(destObject, destIndex,
					source.GetSqlGuid(sourceObject, sourceIndex));
		}
	}

	#endregion

	#region SqlDateTime

	internal sealed class I64TodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class R8TodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class DTTodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetDateTime(sourceObject, sourceIndex)));
		}
	}


#if FW2
	internal sealed class NI64TodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetNullableInt64(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NR8TodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetNullableDouble(sourceObject, sourceIndex)));
		}
	}

	internal sealed class NDTTodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetNullableDateTime(sourceObject, sourceIndex)));
		}
	}


#endif
	internal sealed class dbSTodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetSqlString(sourceObject, sourceIndex)));
		}
	}



	internal sealed class dbI64TodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetSqlInt64(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbR8TodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					Convert.ToSqlDateTime(
						source.GetSqlDouble(sourceObject, sourceIndex)));
		}
	}


	internal sealed class dbDTTodbDT : IValueMapper
	{
		public void Map(
			IMapDataSource source, object sourceObject, int sourceIndex,
			IMapDataDestination dest, object destObject, int destIndex)
		{
			if (source.IsNull(sourceObject, sourceIndex))
				dest.SetNull(destObject, destIndex);
			else
				dest.SetSqlDateTime(destObject, destIndex,
					source.GetSqlDateTime(sourceObject, sourceIndex));
		}
	}

	#endregion
}
