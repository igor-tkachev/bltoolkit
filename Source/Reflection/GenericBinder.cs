using System;
using System.Globalization;
using System.Reflection;

namespace BLToolkit.Reflection
{
	/// <Summary>
	/// Selects a member from a list of candidates, and performs type conversion
	/// from actual argument type to formal argument type.
	/// </Summary>
	[Serializable]
	public class GenericBinder : Binder
	{
		private readonly bool _genericMethodDefinition;
		public GenericBinder(bool genericMethodDefinition)
		{
			_genericMethodDefinition = genericMethodDefinition;
		}

		#region System.Reflection.Binder methods

		public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args,
			ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
		{
			throw new NotImplementedException("GenericBinder.BindToMethod");
		}

		public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
		{
			throw new NotImplementedException("GenericBinder.BindToField");
		}

		public override MethodBase SelectMethod(
			BindingFlags        bindingAttr,
			MethodBase[]        matchMethods,
			Type[]              parameterTypes,
			ParameterModifier[] modifiers)
		{
			for (int i = 0; i < matchMethods.Length; ++i)
			{
				if (matchMethods[i].IsGenericMethodDefinition == _genericMethodDefinition)
				{
					ParameterInfo[] pis = matchMethods[i].GetParameters();

					bool match = (pis.Length == parameterTypes.Length);

					for (int j = 0; match && j < pis.Length; ++j)
					{
						if (pis[j].ParameterType == parameterTypes[j])
							continue;

						if (pis[j].ParameterType.IsGenericParameter)
							match = CheckGenericTypeConstraints(pis[j].ParameterType, parameterTypes[j]);
						else if (pis[j].ParameterType.IsGenericType && parameterTypes[j].IsGenericType)
							match = CompareGenericTypesRecursive(pis[j].ParameterType, parameterTypes[j]);
						else
							match = false;
					}

					if (match)
						return matchMethods[i];
				}
			}

			return null;
		}

		public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType,
			Type[] indexes, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException("GenericBinder.SelectProperty");
		}

		public override object ChangeType(object value, Type type, CultureInfo culture)
		{
			throw new NotImplementedException("GenericBinder.ChangeType");
		}

		public override void ReorderArgumentArray(ref object[] args, object state)
		{
			throw new NotImplementedException("GenericBinder.ReorderArgumentArray");
		}

		#endregion

		private static bool CheckGenericTypeConstraints(Type genType, Type parameterType)
		{
			Type[] constraints = genType.GetGenericParameterConstraints();

			for (int i = 0; i < constraints.Length; i++)
				if (!constraints[i].IsAssignableFrom(parameterType))
					return false;

			return true;
		}

		private static bool CompareGenericTypesRecursive(Type genType, Type specType)
		{
			Type[]  genArgs =  genType.GetGenericArguments();
			Type[] specArgs = specType.GetGenericArguments();

			bool match = (genArgs.Length == specArgs.Length);

			for (int i = 0; match && i < genArgs.Length; i++)
			{
				if (genArgs[i] == specArgs[i])
					continue;

				if (genArgs[i].IsGenericParameter)
					match = CheckGenericTypeConstraints(genArgs[i], specArgs[i]);
				else if (genArgs[i].IsGenericType && specArgs[i].IsGenericType)
					match = CompareGenericTypesRecursive(genArgs[i], specArgs[i]);
				else
					match = false;
			}

			return match;
		}

		private static GenericBinder _generic;
		public  static GenericBinder  Generic
		{
			get { return _generic ?? (_generic = new GenericBinder(true)); }
		}

		private static GenericBinder _nonGeneric;
		public  static GenericBinder  NonGeneric
		{
			get { return _nonGeneric ?? (_nonGeneric = new GenericBinder(false)); }
		}
	}
}
