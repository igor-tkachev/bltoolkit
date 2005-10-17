using System;

namespace BLToolkit.TypeBuilder
{
	public interface IAbstractTypeBuilder : ITypeBuilder
	{
		Type[] GetInterfaces();
		int    GetPriority(BuildOperation operation);
		void   Build      (BuildContext context);

		object TargetElement { get; set; }
	}
}
