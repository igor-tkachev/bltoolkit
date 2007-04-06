using System;
using System.Reflection;
using System.Reflection.Emit;
using BLToolkit.Common;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public class PropertyChangedBuilder : AbstractTypeBuilderBase
	{
		public PropertyChangedBuilder()
			:this(Configuration.NotifyOnEqualSet, true, true)
		{
		}

		public PropertyChangedBuilder(bool notifyOnEqualSet, bool useReferenceEquals, bool skipSetterOnNoChange)
		{
			_notifyOnEqualSet     = notifyOnEqualSet;
			_useReferenceEquals   = useReferenceEquals;
			_skipSetterOnNoChange = skipSetterOnNoChange;
		}

		private bool _notifyOnEqualSet;
		private bool _useReferenceEquals;
		private bool _skipSetterOnNoChange;

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.IsBeforeStep || context.IsAfterStep || context.IsSetter;
		}

		protected override void BeforeBuildAbstractSetter()
		{
			if (!_notifyOnEqualSet && Context.CurrentProperty.CanRead)
				GenerateIsSameValueComparison();
		}

		protected override void BeforeBuildVirtualSetter()
		{
			if (!_notifyOnEqualSet && Context.CurrentProperty.CanRead)
				GenerateIsSameValueComparison();
		}

		protected override void AfterBuildAbstractSetter()
		{
			BuildSetter();
		}

		protected override void AfterBuildVirtualSetter()
		{
			BuildSetter();
		}


		public override bool IsCompatible(BuildContext context, IAbstractTypeBuilder typeBuilder)
		{
			if (typeBuilder is PropertyChangedBuilder)
				return false;

			return base.IsCompatible(context, typeBuilder);
		}

		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.Priority.PropChange;
		}

		private LocalBuilder _isSameValueBuilder;
		private Label        _afterNotificationLabel;

		private void GenerateIsSameValueComparison()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (_skipSetterOnNoChange)
				_afterNotificationLabel = emit.DefineLabel();
			else 
				_isSameValueBuilder = emit.DeclareLocal(typeof(bool));

			if (!Context.CurrentProperty.PropertyType.IsValueType)
			{
				emit
					.ldarg_0
					.callvirt(Context.CurrentProperty.GetGetMethod(true))
					.ldarg_1.end();

				if (!Context.CurrentProperty.PropertyType.Equals(typeof(string)))
				{
					if (_useReferenceEquals)
						emit.call(typeof(object), "ReferenceEquals", typeof(object), typeof(object));
					else
						emit.call(typeof(object), "Equals", typeof(object), typeof(object));
				}
				else
					emit.call(typeof(string), "Equals", typeof(string), typeof(string));

				if (_skipSetterOnNoChange)
					emit.brtrue(_afterNotificationLabel);
				else 
					emit.stloc(_isSameValueBuilder);
			}
			else
			{
				emit
					.ldarg_0
					.callvirt(Context.CurrentProperty.GetGetMethod(true))
					.ldarg_1
					.ceq.end();

				if (_skipSetterOnNoChange)
					emit.brtrue(_afterNotificationLabel);
				else
					emit.stloc(_isSameValueBuilder);
			}
		}

		private void BuildSetter()
		{
			InterfaceMapping im   = Context.Type.GetInterfaceMap(typeof(IPropertyChanged));
			MethodInfo       mi   = im.TargetMethods[0];
			FieldBuilder     ifb  = GetPropertyInfoField();
			EmitHelper       emit = Context.MethodBuilder.Emitter;

			if (!_notifyOnEqualSet && Context.CurrentProperty.CanRead && !_skipSetterOnNoChange)
			{
				_afterNotificationLabel = emit.DefineLabel();
				emit
					.ldloc (_isSameValueBuilder)
					.brtrue(_afterNotificationLabel);
			}

			if (mi.IsPublic)
			{
				emit
					.ldarg_0
					.ldsfld   (ifb)
					.callvirt (mi)
					;
			}
			else
			{
				emit
					.ldarg_0
					.castclass (typeof(IPropertyChanged))
					.ldsfld    (ifb)
					.callvirt  (im.InterfaceMethods[0])
					;
			}

			if (!_notifyOnEqualSet && Context.CurrentProperty.CanRead)
				emit.MarkLabel(_afterNotificationLabel);
		}
	}
}
