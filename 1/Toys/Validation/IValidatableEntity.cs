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
	[MapType(typeof(byte),     typeof(ValidatableValue))]
	[MapType(typeof(char),     typeof(ValidatableValue))]
	[MapType(typeof(ushort),   typeof(ValidatableValue))]
	[MapType(typeof(uint),     typeof(ValidatableValue))]
	[MapType(typeof(ulong),    typeof(ValidatableValue))]
	[MapType(typeof(bool),     typeof(ValidatableValue))]
	[MapType(typeof(sbyte),    typeof(ValidatableValue))]
	[MapType(typeof(short),    typeof(ValidatableValue))]
	[MapType(typeof(int),      typeof(ValidatableValue))]
	[MapType(typeof(long),     typeof(ValidatableValue))]
	[MapType(typeof(float),    typeof(ValidatableValue))]
	[MapType(typeof(double),   typeof(ValidatableValue))]
	[MapType(typeof(string),   typeof(ValidatableValue), "")]
	[MapType(typeof(DateTime), typeof(ValidatableValue))]
	[MapType(typeof(decimal),  typeof(ValidatableValue))]
	[MapType(typeof(Guid),     typeof(ValidatableValue))]
#endif
	public interface IValidatableEntity
	{
	}
}
