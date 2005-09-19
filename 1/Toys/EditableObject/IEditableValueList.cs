using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	[MapAction(typeof(IEditable))]
#if VER2
	[MapType(typeof(byte),     typeof(EditableValue<byte>))]
	[MapType(typeof(char),     typeof(EditableValue<char>))]
	[MapType(typeof(ushort),   typeof(EditableValue<ushort>))]
	[MapType(typeof(uint),     typeof(EditableValue<uint>))]
	[MapType(typeof(ulong),    typeof(EditableValue<ulong>))]
	[MapType(typeof(bool),     typeof(EditableValue<bool>))]
	[MapType(typeof(sbyte),    typeof(EditableValue<sbyte>))]
	[MapType(typeof(short),    typeof(EditableValue<short>))]
	[MapType(typeof(int),      typeof(EditableValue<int>))]
	[MapType(typeof(long),     typeof(EditableValue<long>))]
	[MapType(typeof(float),    typeof(EditableValue<float>))]
	[MapType(typeof(double),   typeof(EditableValue<double>))]
	[MapType(typeof(string),   typeof(EditableValue<string>), "")]
	[MapType(typeof(DateTime), typeof(EditableValue<DateTime>))]
	[MapType(typeof(decimal),  typeof(EditableValue<decimal>))]
	[MapType(typeof(Guid),     typeof(EditableValue<Guid>))]
#else
	[MapType(typeof(byte),     typeof(EditableValue), (byte)0)]
	[MapType(typeof(char),     typeof(EditableValue), (char)0)]
	[MapType(typeof(ushort),   typeof(EditableValue), (ushort)0)]
	[MapType(typeof(uint),     typeof(EditableValue), (uint)0)]
	[MapType(typeof(ulong),    typeof(EditableValue), (ulong)0)]
	[MapType(typeof(bool),     typeof(EditableValue), false)]
	[MapType(typeof(sbyte),    typeof(EditableValue), (sbyte)0)]
	[MapType(typeof(short),    typeof(EditableValue), (short)0)]
	[MapType(typeof(int),      typeof(EditableValue), (int)0)]
	[MapType(typeof(long),     typeof(EditableValue), (long)0)]
	[MapType(typeof(float),    typeof(EditableValue), (float)0)]
	[MapType(typeof(double),   typeof(EditableValue), (double)0)]
	[MapType(typeof(string),   typeof(EditableValue), "")]
	[MapType(typeof(DateTime), typeof(EditableDateTime))]
	[MapType(typeof(decimal),  typeof(EditableDecimal))]
	[MapType(typeof(Guid),     typeof(EditableGuid))]
#endif
	public interface IEditableTypeList
	{
	}
}
