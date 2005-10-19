using System;

namespace BLToolkit.TypeBuilder
{
	public interface IAbstractTypeBuilder : ITypeBuilder
	{
		Type[] GetInterfaces();

		bool   IsApplied    (BuildContext context);
		int    GetPriority  (BuildContext context);
		void   Build        (BuildContext context);

		object TargetElement { get; set; }
	}
}
