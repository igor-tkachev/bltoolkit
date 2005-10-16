using System;

namespace BLToolkit.TypeBuilder
{
	public class AbstractTypeBuilderBase : TypeBuilderBase, IAbstractTypeBuilder
	{
		public virtual Type[] GetInterfaces()
		{
			return null;
		}

		public virtual int GetPriority(BuildOperation operation)
		{
			switch (operation)
			{
				case BuildOperation.BeforeBuild: return GetBeforeBuildPriority();
				case BuildOperation.AfterBuild:  return GetAfterBuildPriority();
			}

			return 0;
		}

		protected virtual int GetBeforeBuildPriority() { return 0; }
		protected virtual int GetAfterBuildPriority () { return 0; }

		public virtual void Build(BuildContext context)
		{
			switch (context.BuildOperation)
			{
				case BuildOperation.BeforeBuild:          BeforeBuild         (context); break;
				case BuildOperation.BuildInterfaceMethod: BuildInterfaceMethod(context); break;
				case BuildOperation.AfterBuild:           AfterBuild          (context); break;
			}
		}

		protected virtual void BeforeBuild         (BuildContext context) {}
		protected virtual void BuildInterfaceMethod(BuildContext context) {}
		protected virtual void AfterBuild          (BuildContext context) {}
	}
}
