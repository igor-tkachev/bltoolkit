using System;

namespace BLToolkit.Mapping
{
	[CLSCompliant(false)]
	public interface IMapDataSource
	{
		int     Count { get; }

		Type    GetFieldType(int index);
		string  GetName     (int index);
		object  GetValue    (object o, int index);
		object  GetValue    (object o, string name);

		bool    IsNull      (object o, int index);

		[CLSCompliant(false)]
		SByte   GetSByte    (object o, int index);
		Int16   GetInt16    (object o, int index);
		Int32   GetInt32    (object o, int index);
		Int64   GetInt64    (object o, int index);

		Byte    GetByte     (object o, int index);
		[CLSCompliant(false)]
		UInt16  GetUInt16   (object o, int index);
		[CLSCompliant(false)]
		UInt32  GetUInt32   (object o, int index);
		[CLSCompliant(false)]
		UInt64  GetUInt64   (object o, int index);

		Boolean GetBoolean  (object o, int index);
		Char    GetChar     (object o, int index);
		Single  GetSingle   (object o, int index);
		Double  GetDouble   (object o, int index);
		Decimal GetDecimal  (object o, int index);
	}
}
