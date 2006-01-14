using System;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Reflection.Emit;

using BLToolkit.TypeBuilder.Builders;
using BLToolkit.Data;
using BLToolkit.Reflection.Emit;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.Data.DataProvider;

namespace BLToolkit.DataAccess
{
	public class DataAccessorBuilder : AbstractTypeBuilderBase
	{
		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.DataAccessorPriority;
		}

		public override bool IsApplied(BuildContext context)
		{
			return context.IsBuildStep && context.IsAbstractMethod;
		}

		const BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		Type            _baseType = typeof(DataAccessor);
		Type            _objectType;
		ParameterInfo[] _parameters;
		ArrayList       _paramList;
		bool            _createManager;
		LocalBuilder    _locManager;
		LocalBuilder    _locObjType;

		protected override void BuildAbstractMethod()
		{
			_paramList     = new ArrayList();
			_createManager = true;
			_objectType    = null;
			_parameters    = Context.CurrentMethod.GetParameters();
			_locManager    = Context.MethodBuilder.Emitter.DeclareLocal(typeof(DbManager));
			_locObjType    = Context.MethodBuilder.Emitter.DeclareLocal(typeof(Type));

			ProcessParameters();
			CreateDbManager();
			SetObjectType();

			// Define execute method type.
			//
			Type returnType = Context.CurrentMethod.ReturnType;

			if (returnType == typeof(DataSet) || returnType.IsSubclassOf(typeof(DataSet)))
			{
				ExecuteDataSet();
			}
			else if (returnType == typeof(DataTable) || returnType.IsSubclassOf(typeof(DataTable)))
			{
				ExecuteDataTable();
			}
			else if (IsInterfaceOf(returnType, typeof(IList)))
			{
#if FW2
				if (returnType.IsGenericType)
					_objectType = returnType.GetGenericArguments()[0];
#endif
				if (_objectType == null)
					_objectType = returnType.GetElementType();

				if (_objectType == null)
					throw new TypeBuilderException(string.Format(
						"Can not determine object type for method '{0}.{1}'",
						Context.CurrentMethod.DeclaringType.Name,
						Context.CurrentMethod.Name));

				ExecuteList();
			}
			else if (returnType == typeof(void))
			{
				ExecuteNonQuery();
			}
			else if (returnType.IsValueType || returnType == typeof(string))
			{
				ExecuteScalar();
			}
			else
			{
				if (_objectType == null)
					_objectType = returnType;

				ExecuteObject();
			}

			Finally();
		}

		private void ProcessParameters()
		{
			for (int i = 0; i < _parameters.Length; i++)
			{
				Type pType = _parameters[i].ParameterType;

				if (pType.IsValueType || pType == typeof(string))
				{
					_paramList.Add(_parameters[i]);
				}
				else if (pType == typeof(DbManager) || pType.IsSubclassOf(typeof(DbManager)))
				{
					_createManager = false;
				}
			}
		}

		private void CreateDbManager()
		{
			if (_createManager)
			{
				Context.MethodBuilder.Emitter
					.ldarg_0
					.callvirt             (_baseType, "GetDbManager", _bindingFlags)
					.stloc                (_locManager)
					.BeginExceptionBlock()
					;
			}
			else
			{
				for (int i = 0; i < _parameters.Length; i++)
				{
					Type pType = _parameters[i].ParameterType;

					if (pType == typeof(DbManager) || pType.IsSubclassOf(typeof(DbManager)))
					{
						Context.MethodBuilder.Emitter
							.ldarg_s ((byte)(_parameters[i].Position + 1))
							.stloc   (_locManager)
							;

						break;
					}
				}
			}
		}

		private void SetObjectType()
		{
			MethodInfo mi = Context.CurrentMethod;

			object[] attrs = mi.GetCustomAttributes(typeof(ObjectTypeAttribute), true);

			if (attrs.Length == 0)
				attrs = mi.DeclaringType.GetCustomAttributes(typeof(ObjectTypeAttribute), true);

			if (attrs.Length != 0)
				_objectType = ((ObjectTypeAttribute)attrs[0]).ObjectType;
		}

		private void ExecuteDataSet()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (Context.CurrentMethod.ReturnType == typeof(DataSet))
			{
				emit
					.ldloc    (Context.ReturnValue)
					.callvirt (typeof(DbManager), "ExecuteDataSet", typeof(DataSet))
					.pop
					.end()
					;
			}
			else
			{
				emit
					.pop
					.ldloc (Context.ReturnValue)
					;

				Label l1 = emit.DefineLabel();
				Label l2 = emit.DefineLabel();

				emit
					.callvirt  (typeof(DataSet).GetProperty("Tables").GetGetMethod())
					.callvirt  (typeof(InternalDataCollectionBase).GetProperty("Count").GetGetMethod())
					.ldc_i4_0
					.ble_s(l1)
					.ldloc     (_locManager)
					.ldloc     (Context.ReturnValue)
					.ldloc     (Context.ReturnValue)
					.callvirt  (typeof(DataSet).GetProperty("Tables").GetGetMethod())
					.ldc_i4_0
					.callvirt  (typeof(DataTableCollection), "get_Item", typeof(int))
					.callvirt  (typeof(DataTable).GetProperty("TableName").GetGetMethod())
					.callvirt  (typeof(DbManager), "ExecuteDataSet", typeof(DataSet), typeof(string))
					.pop
					.br_s      (l2)
					.MarkLabel (l1)
					.ldloc     (_locManager)
					.ldloc     (Context.ReturnValue)
					.callvirt  (typeof(DbManager), "ExecuteDataSet", typeof(DataSet))
					.pop
					.MarkLabel (l2)
					;
			}
		}

		private void ExecuteDataTable()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			Context.MethodBuilder.Emitter
				.ldloc    (Context.ReturnValue)
				.callvirt (typeof(DbManager), "ExecuteDataTable", typeof(DataTable))
				.pop
				.end()
				;
		}

		private void ExecuteList()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			Context.MethodBuilder.Emitter
				.ldloc    (Context.ReturnValue)
				.ldloc    (_locObjType)
				.callvirt (typeof(DbManager), "ExecuteList", typeof(IList), typeof(Type))
				.pop
				.end()
				;
		}

		public void ExecuteNonQuery()
		{
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			MethodInfo   mi      = typeof(DbManager).GetMethod("ExecuteNonQuery");
			LocalBuilder locExec = Context.MethodBuilder.Emitter.DeclareLocal(mi.ReturnType);

			Context.MethodBuilder.Emitter
				.callvirt (mi)
				.stloc    (locExec)
				;

			if (Context.ReturnValue != null)
			{
				Context.MethodBuilder.Emitter
					.ldloc (locExec)
					.stloc (Context.ReturnValue)
					;
			}
		}

		public void ExecuteScalar()
		{
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			Context.MethodBuilder.Emitter
				.callvirtNoGenerics (typeof(DbManager), "ExecuteScalar")
				.stloc              (Context.ReturnValue)
				;
		}

		public void ExecuteObject()
		{
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			Context.MethodBuilder.Emitter
				.ldloc     (_locObjType)
				.callvirt  (typeof(DbManager), "ExecuteObject", typeof(Type))
				.castclass (_objectType)
				.stloc     (Context.ReturnValue)
				;
		}

		private void Finally()
		{
			if (_createManager)
			{
				Label fin = Context.MethodBuilder.Emitter.DefineLabel();

				Context.MethodBuilder.Emitter
					.BeginFinallyBlock()
					.ldloc               (_locManager)
					.brfalse_s           (fin)
					.ldloc               (_locManager)
					.callvirt            (typeof(IDisposable).GetMethod("Dispose"))
					.MarkLabel           (fin)
					.EndExceptionBlock()
					;
			}
		}

		private void CreateReturnTypeInstance()
		{
			ConstructorInfo ci = TypeHelper.GetDefaultConstructor(Context.CurrentMethod.ReturnType);

			Context.MethodBuilder.Emitter
				.newobj (ci)
				.stloc  (Context.ReturnValue)
				;
		}

		private void InitObjectType()
		{
			if (_objectType == null)
			{
				Context.MethodBuilder.Emitter
					.ldnull
					.stloc  (_locObjType)
					;
			}
			else
			{
				Context.MethodBuilder.Emitter
					.LoadType (_objectType)
					.stloc    (_locObjType)
					;
			}
		}

		private void GetSprocName()
		{
			object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(SprocNameAttribute), true);

			if (attrs.Length == 0)
			{
				attrs = Context.CurrentMethod.GetCustomAttributes(typeof(ActionNameAttribute), true);

				string actionName = attrs.Length == 0 ?
					Context.CurrentMethod.Name : ((ActionNameAttribute)attrs[0]).Name;

				// Call GetSpName.
				//
				Context.MethodBuilder.Emitter
					.ldloc    (_locManager)
					.ldarg_0
					.ldloc    (_locObjType)
					.ldstr    (actionName)
					.callvirt (_baseType, "GetSpName", _bindingFlags, typeof(Type), typeof(string))
					;
			}
			else
			{
				Context.MethodBuilder.Emitter
					.ldloc (_locManager)
					.ldstr (((SprocNameAttribute)attrs[0]).Name)
					;
			}
		}

		private void CallSetCommand()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			// Parameters.
			//
			LocalBuilder locParams = emit.DeclareLocal(typeof(object[]));

			emit
				.ldc_i4 (_paramList.Count)
				.newarr (typeof(object))
				.stloc  (locParams)
				;

			object[] attrs =
				Context.CurrentMethod.DeclaringType.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			bool discoverParams = attrs.Length == 0?
				false: ((DiscoverParametersAttribute)attrs[0]).Discover;

			attrs = Context.CurrentMethod.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			if (attrs.Length != 0)
				discoverParams = ((DiscoverParametersAttribute)attrs[0]).Discover;

			for (int i = 0; i < _paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)_paramList[i];

				emit
					.ldloc  (locParams)
					.ldc_i4 (i)
					;

				if (discoverParams)
				{
					emit
						.ldarg_s        ((byte)(pi.Position + 1))
						.boxIfValueType (pi.ParameterType)
						;
				}
				else
				{
					attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

					string paramName = attrs.Length == 0 ?
						pi.Name : ((ParamNameAttribute)attrs[0]).Name;

					emit
						.ldloc (_locManager);

					if (paramName[0] != '@')
					{
						//paramName = '@' + paramName;
						emit
							.ldloc    (_locManager)
							.callvirt (typeof(DbManager).GetProperty("DataProvider").GetGetMethod())
							.ldstr    (paramName)
							.callvirt (typeof(IDataProvider), "GetParameterName", typeof(string))
							;
					}
					else
					{
						emit
							.ldstr (paramName);
					}

					emit
						.ldarg_s        ((byte)(pi.Position + 1))
						.boxIfValueType (pi.ParameterType)
						.callvirt       (typeof(DbManager), "Parameter", typeof(string), typeof(object))
						;
				}

				emit
					.stelem_ref
					.end()
					;
			}

			// Call SetSpCommand.
			//
			emit
				.ldloc    (locParams)
				.callvirt (typeof(DbManager), "SetSpCommand", _bindingFlags, typeof(string), typeof(object[]))
				;
		}

		private bool IsInterfaceOf(Type type, Type interfaceType)
		{
			Type[] types = type.GetInterfaces();

			foreach (Type t in types)
				if (t == interfaceType)
					return true;

			return false;
		}
	}
}
