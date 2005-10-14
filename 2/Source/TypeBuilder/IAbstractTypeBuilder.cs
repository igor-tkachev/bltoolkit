using System;

namespace BLToolkit.TypeBuilder
{
	public interface IAbstractTypeBuilder : ITypeBuilder
	{
		Type[] GetInterfaces();
		void BeforeBuild(TypeBuilderContext context);
		void AfterBuild (TypeBuilderContext context);
	}
}
