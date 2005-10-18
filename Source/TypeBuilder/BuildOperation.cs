using System;

namespace BLToolkit.TypeBuilder
{
	public enum BuildOperation
	{
		BeginBuild,
		EndBuild,

		BeginBuildAbstractGetter,
		BuildAbstractGetter,
		EndBuildAbstractGetter,

		BeginBuildAbstractSetter,
		BuildAbstractSetter,
		EndBuildAbstractSetter,

		BeginBuildAbstractMethod,
		BuildAbstractMethod,
		EndBuildAbstractMethod,

		BuildInterfaceMethod
	}
}
