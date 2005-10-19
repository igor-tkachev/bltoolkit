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

		public virtual bool IsApplied(BuildContext context)
		{
			return false;
		}

		public virtual int GetPriority(BuildContext context)
		{
			return 0;
		}

		public virtual void Build(BuildContext context)
		{
			switch (context.Element)
			{
				case BuildElement.Type:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildType(context); break;
						case BuildStep.Build:        BuildType(context); break;
						case BuildStep.After:   AfterBuildType(context); break;
					}

					break;

				case BuildElement.AbstractGetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildAbstractGetter(context); break;
						case BuildStep.Build:        BuildAbstractGetter(context); break;
						case BuildStep.After:   AfterBuildAbstractGetter(context); break;
					}

					break;

				case BuildElement.AbstractSetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildAbstractSetter(context); break;
						case BuildStep.Build:        BuildAbstractSetter(context); break;
						case BuildStep.After:   AfterBuildAbstractSetter(context); break;
					}

					break;

				case BuildElement.AbstractMethod:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildAbstractMethod(context); break;
						case BuildStep.Build:        BuildAbstractMethod(context); break;
						case BuildStep.After:   AfterBuildAbstractMethod(context); break;
					}

					break;

				case BuildElement.VirtualGetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildVirtualGetter(context); break;
						case BuildStep.Build:        BuildVirtualGetter(context); break;
						case BuildStep.After:   AfterBuildVirtualGetter(context); break;
					}

					break;

				case BuildElement.VirtualSetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildVirtualSetter(context); break;
						case BuildStep.Build:        BuildVirtualSetter(context); break;
						case BuildStep.After:   AfterBuildVirtualSetter(context); break;
					}

					break;

				case BuildElement.VirtualMethod:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildVirtualMethod(context); break;
						case BuildStep.Build:        BuildVirtualMethod(context); break;
						case BuildStep.After:   AfterBuildVirtualMethod(context); break;
					}

					break;

				case BuildElement.InterfaceMethod:
					BuildInterfaceMethod(context);
					break;
			}
		}

		protected virtual void BeforeBuildType          (BuildContext context) {}
		protected virtual void       BuildType          (BuildContext context) {}
		protected virtual void  AfterBuildType          (BuildContext context) {}

		protected virtual void BeforeBuildAbstractGetter(BuildContext context) {}
		protected virtual void       BuildAbstractGetter(BuildContext context) {}
		protected virtual void  AfterBuildAbstractGetter(BuildContext context) {}

		protected virtual void BeforeBuildAbstractSetter(BuildContext context) {}
		protected virtual void       BuildAbstractSetter(BuildContext context) {}
		protected virtual void  AfterBuildAbstractSetter(BuildContext context) {}

		protected virtual void BeforeBuildAbstractMethod(BuildContext context) {}
		protected virtual void       BuildAbstractMethod(BuildContext context) {}
		protected virtual void  AfterBuildAbstractMethod(BuildContext context) {}

		protected virtual void BeforeBuildVirtualGetter (BuildContext context) {}
		protected virtual void       BuildVirtualGetter (BuildContext context) {}
		protected virtual void  AfterBuildVirtualGetter (BuildContext context) {}

		protected virtual void BeforeBuildVirtualSetter (BuildContext context) {}
		protected virtual void       BuildVirtualSetter (BuildContext context) {}
		protected virtual void  AfterBuildVirtualSetter (BuildContext context) {}

		protected virtual void BeforeBuildVirtualMethod (BuildContext context) {}
		protected virtual void       BuildVirtualMethod (BuildContext context) {}
		protected virtual void  AfterBuildVirtualMethod (BuildContext context) {}

		protected virtual void BuildInterfaceMethod     (BuildContext context) {}
	}
}
