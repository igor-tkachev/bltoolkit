using System;

namespace BLToolkit.TypeBuilder
{
	public class AbstractTypeBuilderBase : TypeBuilderBase, IAbstractTypeBuilder
	{
		public virtual Type[] GetInterfaces()
		{
			return null;
		}

		private object _targetElement;
		public  object  TargetElement
		{
			get { return _targetElement;  }
			set { _targetElement = value; }
		}

		public virtual int GetPriority(BuildOperation operation)
		{
			switch (operation)
			{
				case BuildOperation.BeginBuild:               return BeginBuildPriority;
				case BuildOperation.EndBuild:                 return EndBuildPriority;

				case BuildOperation.BeginBuildAbstractGetter: return BeginBuildAbstractGetterPriority;
				case BuildOperation.BuildAbstractGetter:      return BuildAbstractGetterPriority;
				case BuildOperation.EndBuildAbstractGetter:   return EndBuildAbstractGetterPriority;

				case BuildOperation.BeginBuildAbstractSetter: return BeginBuildAbstractSetterPriority;
				case BuildOperation.BuildAbstractSetter:      return BuildAbstractSetterPriority;
				case BuildOperation.EndBuildAbstractSetter:   return EndBuildAbstractSetterPriority;

				case BuildOperation.BeginBuildAbstractMethod: return BeginBuildAbstractMethodPriority;
				case BuildOperation.BuildAbstractMethod:      return BuildAbstractMethodPriority;
				case BuildOperation.EndBuildAbstractMethod:   return EndBuildAbstractMethodPriority;
			}

			return 0;
		}

		protected virtual int BeginBuildPriority               { get { return 0; } }
		protected virtual int EndBuildPriority                 { get { return 0; } }

		protected virtual int BeginBuildAbstractGetterPriority { get { return 0; } }
		protected virtual int BuildAbstractGetterPriority      { get { return 0; } }
		protected virtual int EndBuildAbstractGetterPriority   { get { return 0; } }

		protected virtual int BeginBuildAbstractSetterPriority { get { return 0; } }
		protected virtual int BuildAbstractSetterPriority      { get { return 0; } }
		protected virtual int EndBuildAbstractSetterPriority   { get { return 0; } }

		protected virtual int BeginBuildAbstractMethodPriority { get { return 0; } }
		protected virtual int BuildAbstractMethodPriority      { get { return 0; } }
		protected virtual int EndBuildAbstractMethodPriority   { get { return 0; } }

		public virtual void Build(BuildContext context)
		{
			switch (context.BuildOperation)
			{
				case BuildOperation.BeginBuild:               BeginBuild(context);               break;
				case BuildOperation.EndBuild:                 EndBuild(context);                 break;

				case BuildOperation.BeginBuildAbstractGetter: BeginBuildAbstractGetter(context); break;
				case BuildOperation.BuildAbstractGetter:      BuildAbstractGetter(context);      break;
				case BuildOperation.EndBuildAbstractGetter:   EndBuildAbstractGetter(context);   break;

				case BuildOperation.BeginBuildAbstractSetter: BeginBuildAbstractSetter(context); break;
				case BuildOperation.BuildAbstractSetter:      BuildAbstractSetter(context);      break;
				case BuildOperation.EndBuildAbstractSetter:   EndBuildAbstractSetter(context);   break;

				case BuildOperation.BeginBuildAbstractMethod: BeginBuildAbstractMethod(context); break;
				case BuildOperation.BuildAbstractMethod:      BuildAbstractMethod(context);      break;
				case BuildOperation.EndBuildAbstractMethod:   EndBuildAbstractMethod(context);   break;

				case BuildOperation.BuildInterfaceMethod:     BuildInterfaceMethod(context);     break;
			}
		}

		protected virtual void BeginBuild              (BuildContext context) {}
		protected virtual void EndBuild                (BuildContext context) {}

		protected virtual void BeginBuildAbstractGetter(BuildContext context) {}
		protected virtual void BuildAbstractGetter     (BuildContext context) {}
		protected virtual void EndBuildAbstractGetter  (BuildContext context) {}

		protected virtual void BeginBuildAbstractSetter(BuildContext context) {}
		protected virtual void BuildAbstractSetter     (BuildContext context) {}
		protected virtual void EndBuildAbstractSetter  (BuildContext context) {}

		protected virtual void BeginBuildAbstractMethod(BuildContext context) {}
		protected virtual void BuildAbstractMethod     (BuildContext context) {}
		protected virtual void EndBuildAbstractMethod  (BuildContext context) {}

		protected virtual void BuildInterfaceMethod    (BuildContext context) {}
	}
}
