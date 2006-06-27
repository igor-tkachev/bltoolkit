using System;

namespace BLToolkit.Mapping
{
	[CLSCompliant(false)]
	public interface IMapDataDestination
	{
		Type GetFieldType(int index);
		int  GetOrdinal  (string name);
		void SetValue    (object o, int index,   object value);
		void SetValue    (object o, string name, object value);

		void SetNull     (object o, int index);

		[CLSCompliant(false)]
		void SetSByte    (object o, int index, SByte   value);
		void SetInt16    (object o, int index, Int16   value);
		void SetInt32    (object o, int index, Int32   value);
		void SetInt64    (object o, int index, Int64   value);

		void SetByte     (object o, int index, Byte    value);
		[CLSCompliant(false)]
		void SetUInt16   (object o, int index, UInt16  value);
		[CLSCompliant(false)]
		void SetUInt32   (object o, int index, UInt32  value);
		[CLSCompliant(false)]
		void SetUInt64   (object o, int index, UInt64  value);

		void SetBoolean  (object o, int index, Boolean value);
		void SetChar     (object o, int index, Char    value);
		void SetSingle   (object o, int index, Single  value);
		void SetDouble   (object o, int index, Double  value);
		void SetDecimal  (object o, int index, Decimal value);
		void SetGuid     (object o, int index, Guid    value);
	}
}
