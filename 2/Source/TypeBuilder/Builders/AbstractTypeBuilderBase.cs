using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BLToolkit.TypeBuilder.Builders
{
	public class AbstractTypeBuilderBase : IAbstractTypeBuilder
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

		private BuildContext _context;
		public  BuildContext  Context
		{
			get { return _context;  }
			set { _context = value; }
		}

		public virtual bool IsCompatible(BuildContext context, IAbstractTypeBuilder typeBuilder)
		{
			return true;
		}

		protected bool IsRelative(IAbstractTypeBuilder typeBuilder)
		{
			if (typeBuilder == null) throw new ArgumentNullException("typeBuilder");

			return GetType().IsInstanceOfType(typeBuilder) || typeBuilder.GetType().IsInstanceOfType(this);
		}

		public virtual bool IsApplied(BuildContext context)
		{
			return false;
		}

		public virtual int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.NormalBuilderPriority;
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		public virtual void Build(BuildContext context)
		{
			if (context == null) throw new ArgumentNullException("context");

			Context = context;

			switch (context.BuildElement)
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

		#region Helpers

		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		protected bool CallLazyInstanceInsurer(FieldBuilder field)
		{
			if (field == null) throw new ArgumentNullException("field");

			MethodBuilderHelper ensurer = Context.GetFieldInstanceEnsurer(field.Name);

			if (ensurer != null)
			{
				Context.MethodBuilder.Emitter
					.ldarg_0
					.call    (ensurer);
			}

			return ensurer != null;
		}

		[SuppressMessage("Microsoft.Performance", "CA1818:DoNotConcatenateStringsInsideLoops")]
		protected virtual string GetFieldName(PropertyInfo propertyInfo)
		{
			string name = propertyInfo.Name;

			if (char.IsUpper(name[0]) && name.Length > 1 && char.IsLower(name[1]))
				name = char.ToLower(name[0], CultureInfo.CurrentCulture) + name.Substring(1, name.Length - 1);

			name = "_" + name;

			foreach (ParameterInfo p in propertyInfo.GetIndexParameters())
				name += "." + p.ParameterType.FullName;//.Replace(".", "_").Replace("+", "_");

			return name;
		}

		protected string GetFieldName()
		{
			return GetFieldName(Context.CurrentProperty);
		}

		protected FieldBuilder GetPropertyInfoField(PropertyInfo property)
		{
			string       fieldName = GetFieldName(property) + "_$propertyInfo";
			FieldBuilder field     = Context.GetField(fieldName);

			if (field == null)
			{
				field = Context.CreatePrivateStaticField(fieldName, typeof(PropertyInfo));

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				ParameterInfo[] index      = property.GetIndexParameters();
				LocalBuilder    parameters = null;

				if (index.Length > 0)
				{
					parameters = (LocalBuilder)Context.Items["$BLToolkit.ParameterLocalBuilder."];

					if (parameters == null)
					{
						parameters = emit.DeclareLocal(typeof(Type[]));

						Context.Items["$BLToolkit.ParameterLocalBuilder."] = parameters;
					}
				}

				emit
					.LoadType (Context.Type)
					.ldstr    (property.Name)
					.LoadType (property.PropertyType)
					;

				if (index.Length == 0)
				{
					emit
						.ldsfld (typeof(Type).GetField("EmptyTypes"))
						;
				}
				else
				{
					emit
						.ldc_i4 (index.Length) 
						.newarr (typeof(Type))
						.stloc  (parameters)
						;

					for (int i = 0; i < index.Length; i++)
						emit
							.ldloc      (parameters)
							.ldc_i4     (i) 
							.LoadType   (index[i].ParameterType)
							.stelem_ref
							.end()
							;

					emit.ldloc(parameters);
				}

				emit
					.call   (typeof(TypeHelper).GetMethod("GetPropertyInfo"))
					.stsfld (field)
					;
			}

			return field;
		}

		protected FieldBuilder GetPropertyInfoField()
		{
			return GetPropertyInfoField(Context.CurrentProperty);
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		protected FieldBuilder GetParameterField()
		{
			string       fieldName = GetFieldName() + "_$parameters";
			FieldBuilder field     = Context.GetField(fieldName);

			if (field == null)
			{
				field = Context.CreatePrivateStaticField(fieldName, typeof(object[]));

				FieldBuilder piField = GetPropertyInfoField();
				EmitHelper   emit    = Context.TypeBuilder.TypeInitializer.Emitter;

				emit
					.ldsfld (piField)
					.call   (typeof(TypeHelper).GetMethod("GetPropertyParameters"))
					.stsfld (field)
					;
			}

			return field;
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		protected FieldBuilder GetTypeAccessorField()
		{
			string       fieldName = "_" + GetFieldType().Name + "_$typeAccessor";
			FieldBuilder field     = Context.GetField(fieldName);

			if (field == null)
			{
				field = Context.CreatePrivateStaticField(fieldName, typeof(TypeAccessor));

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				emit
					.LoadType (GetFieldType())
					.call     (typeof(TypeAccessor), "GetAccessor", typeof(Type))
					.stsfld   (field)
					;
			}

			return field;
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		protected virtual Type GetFieldType()
		{
			PropertyInfo    pi    = Context.CurrentProperty;
			ParameterInfo[] index = pi.GetIndexParameters();

			switch (index.Length)
			{
				case 0: return pi.PropertyType;
				case 1: return typeof(Hashtable);
				default:
					throw new InvalidOperationException();
			}
		}

		#endregion
	}
}
