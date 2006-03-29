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
		ArrayList       _refParamList;
		bool            _createManager;
		LocalBuilder    _locManager;
		LocalBuilder    _locObjType;
		ArrayList       _outputParameters;

		protected override void BuildAbstractMethod()
		{
			// Any class variable must be initialized before use
			// as the same instance of the class is utilized to build abstract methods.
			//
			_paramList        = new ArrayList();
			_refParamList     = new ArrayList();
			_createManager    = true;
			_objectType       = null;
			_parameters       = Context.CurrentMethod.GetParameters();
			_locManager       = Context.MethodBuilder.Emitter.DeclareLocal(typeof(DbManager));
			_locObjType       = Context.MethodBuilder.Emitter.DeclareLocal(typeof(Type));
			_outputParameters = null;

			ProcessParameters();
			CreateDbManager();
			SetObjectType();

			// Define execute method type.
			//
			Type returnType = Context.CurrentMethod.ReturnType;

			if (returnType == typeof(IDataReader))
			{
				ExecuteReader();
			}
			else if (returnType == typeof(DataSet) || returnType.IsSubclassOf(typeof(DataSet)))
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
			else if (returnType.IsValueType || returnType == typeof(string) || returnType == typeof(byte[]))
			{
				ExecuteScalar();
			}
			else
			{
				if (_objectType == null)
					_objectType = returnType;

				ExecuteObject();
			}

			GetOutRefParameters();
			Finally();
		}

		private void ProcessParameters()
		{
			for (int i = 0; i < _parameters.Length; i++)
			{
				Type pType = _parameters[i].ParameterType;

				if (pType.IsByRef)
					pType = pType.GetElementType();

				if (pType.IsValueType || pType == typeof(string) || pType == typeof(byte[]))
				{
					_paramList.Add(_parameters[i]);
				}
				else if (pType == typeof(DbManager) || pType.IsSubclassOf(typeof(DbManager)))
				{
					_createManager = false;
				}
				else
				{
					_refParamList.Add(_parameters[i]);
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

		private void ExecuteReader()
		{
			InitObjectType();
			GetSprocName();
			CallSetCommand();

			MethodInfo mi = typeof(DbManager).GetMethod("ExecuteReader", Type.EmptyTypes);

			Context.MethodBuilder.Emitter
				.callvirt(mi)
				.stloc(Context.ReturnValue)
				;
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
				.CastFromObject     (Context.CurrentMethod.ReturnType)
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
				Label        fin = Context.MethodBuilder.Emitter.DefineLabel();
				PropertyInfo pi  = typeof(DataAccessor)
					.GetProperty("DisposeDbManager", BindingFlags.NonPublic | BindingFlags.Instance);

				Context.MethodBuilder.Emitter
					.BeginFinallyBlock()
					.ldloc               (_locManager)
					.brfalse_s           (fin)
					.ldarg_0
					.callvirt            (pi.GetGetMethod(true))
					.brfalse_s           (fin)
					.ldloc               (_locManager)
					.callvirt            (typeof(IDisposable), "Dispose")
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

			// Get DiscoverParametersAttribute.
			//
			object[] attrs =
				Context.CurrentMethod.DeclaringType.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			bool discoverParams = attrs.Length == 0?
				false: ((DiscoverParametersAttribute)attrs[0]).Discover;

			attrs = Context.CurrentMethod.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			if (attrs.Length != 0)
				discoverParams = ((DiscoverParametersAttribute)attrs[0]).Discover;

			LocalBuilder locParams = discoverParams?
				BuildParametersWithDiscoverParameters():
				BuildParameters();

			// Call SetSpCommand.
			//
			emit
				.ldloc    (locParams)
				.callvirt (typeof(DbManager), "SetSpCommand", _bindingFlags, typeof(string), typeof(object[]))
				;
		}

		private LocalBuilder BuildParameters()
		{
			return _refParamList.Count > 0?
				BuildRefParameters():
				BuildSimpleParameters();
		}

		private LocalBuilder BuildSimpleParameters()
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

			for (int i = 0; i < _paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)_paramList[i];

				emit
					.ldloc  (locParams)
					.ldc_i4 (i)
					;

				BuildParameter(pi);

				emit
					.stelem_ref
					.end()
					;
			}

			return locParams;
		}

		private LocalBuilder BuildRefParameters()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			// Parameters.
			//
			LocalBuilder locParams = emit.DeclareLocal(typeof(object[]));

			emit
				.ldc_i4 (_parameters.Length)
				.newarr (typeof(object))
				.stloc  (locParams)
				;

			for (int i = 0; i < _parameters.Length; i++)
			{
				ParameterInfo pi = _parameters[i];

				emit
					.ldloc  (locParams)
					.ldc_i4 (i)
					;

				if (_paramList.Contains(pi))
				{
					BuildParameter(pi);
				}
				else if (_refParamList.Contains(pi))
				{
					Type type =
						pi.ParameterType == typeof(DataRow) || pi.ParameterType.IsSubclassOf(typeof(DataRow))?
							typeof(DataRow): typeof(object);

					emit
						.ldloc    (_locManager)
						.ldarg    (pi)
						.ldnull
						.callvirt (typeof(DbManager), "CreateParameters", type, typeof(IDbDataParameter[]))
						;
				}
				else
				{
					emit
						.ldnull
						.end()
						;
				}

				emit
					.stelem_ref
					.end()
					;
			}

			LocalBuilder retParams = emit.DeclareLocal(typeof(IDbDataParameter[]));

			emit
				.ldarg_0
				.ldloc    (locParams)
				.callvirt (_baseType, "PrepareParameters", _bindingFlags, typeof(object[]))
				.stloc    (retParams)
				;

			return retParams;
		}

		private void BuildParameter(ParameterInfo pi)
		{
			EmitHelper emit  = Context.MethodBuilder.Emitter;
			object[]   attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

			string paramName = attrs.Length == 0?
				pi.Name: ((ParamNameAttribute)attrs[0]).Name;

			emit
				.ldloc(_locManager);

			if (paramName[0] != '@')
			{
				emit
					.ldloc    (_locManager)
					.callvirt (typeof(DbManager).GetProperty("DataProvider").GetGetMethod())
					.ldstr    (paramName)
					.ldc_i4   ((int)ConvertType.NameToParameter)
					.callvirt (typeof(IDataProvider), "Convert", typeof(string), typeof(ConvertType))
					;
			}
			else
			{
				emit
					.ldstr(paramName);
			}

			if (pi.ParameterType.IsByRef)
			{
				if (_outputParameters == null)
					_outputParameters = new ArrayList();

				_outputParameters.Add(pi);

				Type   type       = pi.ParameterType.GetElementType();
				string methodName = pi.IsOut? "OutputParameter": "InputOutputParameter";

				emit
					.ldarg(pi)
					;

				if (type.IsValueType && type.IsPrimitive == false)
					emit.ldobj(type);
				else
					emit.ldind(type);

				emit
					.boxIfValueType (type)
					.callvirt       (typeof(DbManager), methodName, typeof(string), typeof(object))
					;
			}
			else
			{
				emit
					.ldarg          (pi)
					.boxIfValueType (pi.ParameterType)
					.callvirt       (typeof(DbManager), "Parameter", typeof(string), typeof(object))
					;
			}
		}

		private LocalBuilder BuildParametersWithDiscoverParameters()
		{
			EmitHelper   emit      = Context.MethodBuilder.Emitter;
			LocalBuilder locParams = emit.DeclareLocal(typeof(object[]));

			emit
				.ldc_i4 (_paramList.Count)
				.newarr (typeof(object))
				.stloc  (locParams)
				;

			for (int i = 0; i < _paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)_paramList[i];

				emit
					.ldloc          (locParams)
					.ldc_i4         (i)
					.ldarg          (pi)
					.boxIfValueType (pi.ParameterType)
					.stelem_ref
					.end()
					;
			}

			return locParams;
		}

		private void GetOutRefParameters()
		{
			if (_outputParameters != null)
			{
				EmitHelper emit = Context.MethodBuilder.Emitter;

				foreach (ParameterInfo pi in _outputParameters)
				{
					Type type = pi.ParameterType.GetElementType();

					emit
						.ldarg(pi)
						;

					object[] attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

					string paramName = attrs.Length == 0 ?
						pi.Name : ((ParamNameAttribute)attrs[0]).Name;

					emit
						.ldloc (_locManager);

					if (paramName[0] != '@')
					{
						emit
							.ldloc    (_locManager)
							.callvirt (typeof(DbManager).GetProperty("DataProvider").GetGetMethod())
							.ldstr    (paramName)
							.ldc_i4   ((int)ConvertType.NameToParameter)
							.callvirt (typeof(IDataProvider), "Convert", typeof(string), typeof(ConvertType))
							;
					}
					else
					{
						emit
							.ldstr (paramName);
					}

					emit
						.callvirt       (typeof(DbManager), "Parameter", typeof(string))
						.callvirt       (typeof(IDataParameter).GetProperty("Value").GetGetMethod())
						.LoadType       (type)
						.call           (typeof(Convert), "ChangeType", typeof(object), typeof(Type))
						.CastFromObject (type)
						;

					if (type.IsValueType && type.IsPrimitive == false)
						emit.stobj(type);
					else
						emit.stind(type);
				}
			}
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
