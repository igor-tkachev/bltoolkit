using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public class AbstractTypeBuilderBase : TypeBuilderBase, IAbstractTypeBuilder
	{
		public const int NormalPriority = 0;

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
			return NormalPriority;
		}

		public virtual void Build(BuildContext context)
		{
			Context = context;

			switch (context.Element)
			{
				case BuildElement.Type:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildType(); break;
						case BuildStep.Build:        BuildType(); break;
						case BuildStep.After:   AfterBuildType(); break;
					}

					break;

				case BuildElement.AbstractGetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildAbstractGetter(); break;
						case BuildStep.Build:        BuildAbstractGetter(); break;
						case BuildStep.After:   AfterBuildAbstractGetter(); break;
					}

					break;

				case BuildElement.AbstractSetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildAbstractSetter(); break;
						case BuildStep.Build:        BuildAbstractSetter(); break;
						case BuildStep.After:   AfterBuildAbstractSetter(); break;
					}

					break;

				case BuildElement.AbstractMethod:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildAbstractMethod(); break;
						case BuildStep.Build:        BuildAbstractMethod(); break;
						case BuildStep.After:   AfterBuildAbstractMethod(); break;
					}

					break;

				case BuildElement.VirtualGetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildVirtualGetter(); break;
						case BuildStep.Build:        BuildVirtualGetter(); break;
						case BuildStep.After:   AfterBuildVirtualGetter(); break;
					}

					break;

				case BuildElement.VirtualSetter:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildVirtualSetter(); break;
						case BuildStep.Build:        BuildVirtualSetter(); break;
						case BuildStep.After:   AfterBuildVirtualSetter(); break;
					}

					break;

				case BuildElement.VirtualMethod:
					switch (context.Step)
					{
						case BuildStep.Before: BeforeBuildVirtualMethod(); break;
						case BuildStep.Build:        BuildVirtualMethod(); break;
						case BuildStep.After:   AfterBuildVirtualMethod(); break;
					}

					break;

				case BuildElement.InterfaceMethod:
					BuildInterfaceMethod();
					break;
			}
		}

		protected virtual void BeforeBuildType          () {}
		protected virtual void       BuildType          () {}
		protected virtual void  AfterBuildType          () {}

		protected virtual void BeforeBuildAbstractGetter() {}
		protected virtual void       BuildAbstractGetter() {}
		protected virtual void  AfterBuildAbstractGetter() {}

		protected virtual void BeforeBuildAbstractSetter() {}
		protected virtual void       BuildAbstractSetter() {}
		protected virtual void  AfterBuildAbstractSetter() {}

		protected virtual void BeforeBuildAbstractMethod() {}
		protected virtual void       BuildAbstractMethod() {}
		protected virtual void  AfterBuildAbstractMethod() {}

		protected virtual void BeforeBuildVirtualGetter () {}
		protected virtual void       BuildVirtualGetter () {}
		protected virtual void  AfterBuildVirtualGetter () {}

		protected virtual void BeforeBuildVirtualSetter () {}
		protected virtual void       BuildVirtualSetter () {}
		protected virtual void  AfterBuildVirtualSetter () {}

		protected virtual void BeforeBuildVirtualMethod () {}
		protected virtual void       BuildVirtualMethod () {}
		protected virtual void  AfterBuildVirtualMethod () {}

		protected virtual void BuildInterfaceMethod     () {}
	}
}
