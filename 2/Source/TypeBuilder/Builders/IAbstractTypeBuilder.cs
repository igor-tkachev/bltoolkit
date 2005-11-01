using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public interface IAbstractTypeBuilder
	{
		Type[] GetInterfaces();
		bool   IsCompatible (BuildContext context, IAbstractTypeBuilder typeBuilder);

		bool   IsApplied    (BuildContext context);
		int    GetPriority  (BuildContext context);
		void   Build        (BuildContext context);

		object TargetElement { get; set; }
	}
}
