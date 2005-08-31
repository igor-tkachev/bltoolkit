using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	[MapAction(typeof(IValidatable))]
#if VER2
	[MapType(typeof(byte),     typeof(ValidatableValue<byte>))]
	[MapType(typeof(char),     typeof(ValidatableValue<char>))]
	[MapType(typeof(ushort),   typeof(ValidatableValue<ushort>))]
	[MapType(typeof(uint),     typeof(ValidatableValue<uint>))]
	[MapType(typeof(ulong),    typeof(ValidatableValue<ulong>))]
	[MapType(typeof(bool),     typeof(ValidatableValue<bool>))]
	[MapType(typeof(sbyte),    typeof(ValidatableValue<sbyte>))]
	[MapType(typeof(short),    typeof(ValidatableValue<short>))]
	[MapType(typeof(int),      typeof(ValidatableValue<int>))]
	[MapType(typeof(long),     typeof(ValidatableValue<long>))]
	[MapType(typeof(float),    typeof(ValidatableValue<float>))]
	[MapType(typeof(double),   typeof(ValidatableValue<double>))]
	[MapType(typeof(string),   typeof(ValidatableValue<string>), "")]
	[MapType(typeof(DateTime), typeof(ValidatableValue<DateTime>))]
	[MapType(typeof(decimal),  typeof(ValidatableValue<decimal>))]
	[MapType(typeof(Guid),     typeof(ValidatableValue<Guid>))]
#else
	[MapType(typeof(byte),     typeof(ValidatableValue), (byte)0)]
	[MapType(typeof(char),     typeof(ValidatableValue), (char)0)]
	[MapType(typeof(ushort),   typeof(ValidatableValue), (ushort)0)]
	[MapType(typeof(uint),     typeof(ValidatableValue), (uint)0)]
	[MapType(typeof(ulong),    typeof(ValidatableValue), (ulong)0)]
	[MapType(typeof(bool),     typeof(ValidatableValue), false)]
	[MapType(typeof(sbyte),    typeof(ValidatableValue), (sbyte)0)]
	[MapType(typeof(short),    typeof(ValidatableValue), (short)0)]
	[MapType(typeof(int),      typeof(ValidatableValue), (int)0)]
	[MapType(typeof(long),     typeof(ValidatableValue), (long)0)]
	[MapType(typeof(float),    typeof(ValidatableValue), (float)0)]
	[MapType(typeof(double),   typeof(ValidatableValue), (double)0)]
	[MapType(typeof(string),   typeof(ValidatableValue), "")]
	[MapType(typeof(DateTime), typeof(ValidatableDateTime))]
	[MapType(typeof(Decimal),  typeof(ValidatableDecimal))]
	[MapType(typeof(Guid),     typeof(ValidatableGuid))]
#endif
	[Obsolete]
	public interface IValidatableEntity
	{
	}
}
