using System;

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public interface ITypeBuilder
	{
		Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder);
	}
}
