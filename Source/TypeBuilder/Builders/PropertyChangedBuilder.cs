using System;
using System.Reflection;
using System.Reflection.Emit;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public class PropertyChangedBuilder : AbstractTypeBuilderBase
	{
		public PropertyChangedBuilder()
			: this(Common.Configuration.NotifyOnEqualSet, true, true)
		{
		}

		public PropertyChangedBuilder(bool notifyOnEqualSet, bool useReferenceEquals, bool skipSetterOnNoChange)
		{
			_notifyOnEqualSet     = notifyOnEqualSet;
			_useReferenceEquals   = useReferenceEquals;
			_skipSetterOnNoChange = skipSetterOnNoChange;
		}

		private readonly bool _notifyOnEqualSet;
		private readonly bool _useReferenceEquals;
		private readonly bool _skipSetterOnNoChange;

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.IsSetter && (context.IsBeforeStep || context.IsAfterStep);
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

			MethodInfo op_InequalityMethod =
				Context.CurrentProperty.PropertyType.GetMethod("op_Inequality",
					new Type[]
						{
							Context.CurrentProperty.PropertyType,
							Context.CurrentProperty.PropertyType
						});

			if (op_InequalityMethod == null)
			{
				if (Context.CurrentProperty.PropertyType.IsValueType)
				{
					if (TypeHelper.IsNullableType(Context.CurrentProperty.PropertyType))
					{
						// Handled nullable types

						var currentValue      = emit.DeclareLocal(Context.CurrentProperty.PropertyType);
						var newValue          = emit.DeclareLocal(Context.CurrentProperty.PropertyType);
						var notEqualLabel     = emit.DefineLabel();
						var comparedLabel     = emit.DefineLabel();
						var hasValueGetMethod = Context.CurrentProperty.PropertyType.GetProperty("HasValue").GetGetMethod();

						emit
							.ldarg_0
							.callvirt(Context.CurrentProperty.GetGetMethod(true))
							.stloc(currentValue)
							.ldarg_1
							.stloc(newValue)
							.ldloca(currentValue)
							.call(Context.CurrentProperty.PropertyType, "GetValueOrDefault")
							.ldloca(newValue)
							.call(Context.CurrentProperty.PropertyType, "GetValueOrDefault");

						var nullableUnderlyingType = TypeHelper.GetUnderlyingType(Context.CurrentProperty.PropertyType);

						op_InequalityMethod = nullableUnderlyingType.GetMethod("op_Inequality",
						                                                       new Type[]
							                                                       {
								                                                       nullableUnderlyingType,
								                                                       nullableUnderlyingType
							                                                       });

						if (op_InequalityMethod != null)
						{
							emit
								.call(op_InequalityMethod)
							    .brtrue_s(notEqualLabel);
						}
						else
							emit.bne_un_s(notEqualLabel);

						emit
							.ldloca(currentValue)
						    .call(hasValueGetMethod)
						    .ldloca(newValue)
						    .call(hasValueGetMethod)
						    .ceq
						    .ldc_bool(true)
						    .ceq
						    .br(comparedLabel)
						    .MarkLabel(notEqualLabel)
						    .ldc_bool(false)
						    .MarkLabel(comparedLabel)
						    .end();
					}
					else if (!Context.CurrentProperty.PropertyType.IsPrimitive)
					{
						// Handle structs without op_Inequality.
						var currentValue = emit.DeclareLocal(Context.CurrentProperty.PropertyType);

						emit
							.ldarg_0
							.callvirt(Context.CurrentProperty.GetGetMethod(true))
							.stloc(currentValue)
							.ldloca(currentValue)
							.ldarg_1
							.box(Context.CurrentProperty.PropertyType)
							.constrained(Context.CurrentProperty.PropertyType)
							.callvirt(typeof(object), "Equals", new [] {typeof(object)})
							.end();
					}
					else
					{
						// Primitive value type comparison
						emit
							.ldarg_0
							.callvirt(Context.CurrentProperty.GetGetMethod(true))
							.ldarg_1
							.ceq
							.end();
					}
				}
				else if (!_useReferenceEquals)
				{
					// Do not use ReferenceEquals comparison for objects
					emit
						.ldarg_0
						.callvirt(Context.CurrentProperty.GetGetMethod(true))
						.ldarg_1
						.ceq
						.end();
				}
				else
				{
					// ReferenceEquals comparison for objects
					emit
						.ldarg_0
						.callvirt(Context.CurrentProperty.GetGetMethod(true))
						.ldarg_1
						.call(typeof(object), "ReferenceEquals", typeof(object), typeof(object))
						.end();
				}
			}
			else
			{
				// Items compared have op_Inequality operator (!=)
				emit
					.ldarg_0
					.callvirt(Context.CurrentProperty.GetGetMethod(true))
					.ldarg_1
					.call(op_InequalityMethod)
					.ldc_i4_0
					.ceq
					.end();
			}

			if (_skipSetterOnNoChange)
				emit.brtrue(_afterNotificationLabel);
			else
				emit.stloc(_isSameValueBuilder);
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
