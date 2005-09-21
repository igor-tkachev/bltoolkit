using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.DataAccess
{
	class MethodGenerator
	{
		MethodInfo      _methodInfo;
		MethodBuilder   _methodBuilder;

		MapGenerator    _gen;
		ParameterInfo[] _parameters;
		ArrayList       _paramList = new ArrayList();
		LocalBuilder    _locManager;
		LocalBuilder    _locReturn;
		LocalBuilder    _locObjType;
		bool            _createManager = true;
		Type            _objectType;
		Type            _baseType = typeof(DataAccessorBase);

		const BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public void Generate(MethodInfo methodInfo, MethodBuilder methodBuilder)
		{
			_methodInfo    = methodInfo;
			_methodBuilder = methodBuilder;

			_gen           = new MapGenerator(methodBuilder.GetILGenerator());
			_parameters    = methodInfo.GetParameters();
			_locManager    = _gen.DeclareLocal(typeof(DbManager));
			_locReturn     = _gen.DeclareLocal(methodInfo.ReturnType);
			_locObjType    = _gen.DeclareLocal(typeof(Type));

			ProcessParameters();
			CreateDbManager();
			SetObjectType();

			// Define execute type.
			//
			Type returnType = methodInfo.ReturnType;

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
#if VER2
				if (returnType.IsGenericType)
					_objectType = returnType.GetGenericArguments()[0];
#endif
				if (_objectType == null)
					_objectType = returnType.GetElementType();

				if (_objectType == null)
					throw new RsdnDataAccessException(string.Format(
						"Can not determine object type for method '{0}.{1}'",
						_methodInfo.DeclaringType.Name,
						_methodInfo.Name));

				ExecuteList();
			}
			else if (returnType.IsValueType || returnType == typeof(string))
			{
				ExecuteNonQuery();
			}
			else
			{
				if (_objectType == null)
					_objectType = returnType;

				ExecuteObject();
			}

			Finally();
			Return();
		}

		void ExecuteList()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSpocName();
			CallSetCommand();

			_gen
				.ldloc(_locReturn)
				.ldloc(_locObjType)
				.callvirt(typeof(DbManager), "ExecuteList", typeof(IList), typeof(Type))
				.pop
				.EndGen()
				;
		}

		void ExecuteDataTable()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSpocName();
			CallSetCommand();

			_gen
				.ldloc(_locReturn)
				.callvirt(typeof(DbManager), "ExecuteDataTable", typeof(DataTable))
				.pop
				.EndGen()
				;
		}

		void ExecuteDataSet()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSpocName();
			CallSetCommand();

			if (_methodInfo.ReturnType == typeof(DataSet))
			{
				_gen
					.ldloc(_locReturn)
					.callvirt(typeof(DbManager), "ExecuteDataSet", typeof(DataSet))
					.pop
					.EndGen()
					;
			}
			else
			{
				_gen
					.pop
					.ldloc(_locReturn)
					;

				Label l1 = _gen.DefineLabel();
				Label l2 = _gen.DefineLabel();

				_gen
					.callvirt(typeof(DataSet), "get_Tables")
					.callvirt(typeof(InternalDataCollectionBase), "get_Count")
					.ldc_i4_0
					.ble_s(l1)
					.ldloc(_locManager)
					.ldloc(_locReturn)
					.ldloc(_locReturn)
					.callvirt(typeof(DataSet), "get_Tables")
					.ldc_i4_0
					.callvirt(typeof(DataTableCollection), "get_Item", typeof(int))
					.callvirt(typeof(DataTable), "get_TableName")
					.callvirt(typeof(DbManager), "ExecuteDataSet", typeof(DataSet), typeof(string))
					.pop
					.br_s(l2)
					.MarkLabel(l1)
					.ldloc(_locManager)
					.ldloc(_locReturn)
					.callvirt(typeof(DbManager), "ExecuteDataSet", typeof(DataSet))
					.pop
					.MarkLabel(l2)
					;
			}
		}

		void ExecuteObject()
		{
			InitObjectType();
			GetSpocName();
			CallSetCommand();

			_gen
				.ldloc(_locObjType)
				.callvirt(typeof(DbManager), "ExecuteObject", typeof(Type))
				.castclass(_objectType)
				.stloc(_locReturn)
				;
		}

		void ExecuteNonQuery()
		{
			InitObjectType();
			GetSpocName();
			CallSetCommand();

			MethodInfo   mi      = typeof(DbManager).GetMethod("ExecuteNonQuery");
			LocalBuilder locExec = _gen.DeclareLocal(mi.ReturnType);

			_gen
				.callvirt(mi)
				.stloc(locExec)
				;

			if (mi.ReturnType == _methodInfo.ReturnType ||
				mi.ReturnType.IsSubclassOf(_methodInfo.ReturnType))
			{
				_gen
					.ldloc(locExec)
					.stloc(_locReturn)
					;
			}
		}

		void SetObjectType()
		{
			object[] attrs = _methodInfo.GetCustomAttributes(typeof(ObjectTypeAttribute), true);

			if (attrs.Length == 0)
				attrs = _methodInfo.DeclaringType.GetCustomAttributes(typeof(ObjectTypeAttribute), true);

			if (attrs.Length != 0)
				_objectType = ((ObjectTypeAttribute)attrs[0]).ObjectType;
		}

		bool IsInterfaceOf(Type type, Type interfaceType)
		{
			Type[] types = type.GetInterfaces();

			foreach (Type t in types)
				if (t == interfaceType)
					return true;

			return false;
		}

		void CreateReturnTypeInstance()
		{
			ConstructorInfo ci = DataAccessFactory.GetDefaultConstructor(_methodInfo.ReturnType);

			_gen
				.newobj(ci)
				.stloc(_locReturn)
				;
		}

		void ProcessParameters()
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

		void CreateDbManager()
		{
			if (_createManager)
			{
				_gen
					.ldarg_0
					.callvirt(_baseType, "GetDbManager", _bindingFlags)
					.stloc(_locManager)
					.BeginExceptionBlock
					.EndGen()
					;
			}
			else
			{
				for (int i = 0; i < _parameters.Length; i++)
				{
					Type pType = _parameters[i].ParameterType;

					if (pType == typeof(DbManager) || pType.IsSubclassOf(typeof(DbManager)))
					{
						_gen
							.ldarg_s((byte)(_parameters[i].Position + 1))
							.stloc(_locManager)
							;

						break;
					}
				}
			}
		}

		void Finally()
		{
			if (_createManager)
			{
				Label fin = _gen.DefineLabel();

				_gen
					.BeginFinallyBlock
					.ldloc(_locManager)
					.brfalse_s(fin)
					.ldloc(_locManager)
					.callvirt(typeof(IDisposable).GetMethod("Dispose"))
					.MarkLabel(fin)
					.EndExceptionBlock
					.EndGen()
					;
			}
		}

		void Return()
		{
			if (_methodInfo.ReturnType != typeof(void))
				_gen.ldloc(_locReturn);

			_gen.ret();
		}

		void InitObjectType()
		{
			if (_objectType == null)
			{
				_gen
					.ldnull
					.stloc(_locObjType)
					;
			}
			else
			{
				_gen
					.LoadType(_objectType)
					.stloc(_locObjType)
					;
			}
		}

		void GetSpocName()
		{
			object[] attrs = _methodInfo.GetCustomAttributes(typeof(SprocNameAttribute), true);

			if (attrs.Length == 0)
			{
				attrs = _methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true);

				string actionName = attrs.Length == 0 ?
					_methodInfo.Name : ((ActionNameAttribute)attrs[0]).Name;

				// Call GetSpName.
				//
				_gen
					.ldloc(_locManager)
					.ldarg_0
					.ldloc(_locObjType)
					.ldstr(actionName)
					.callvirt(_baseType, "GetSpName", _bindingFlags, typeof(Type), typeof(string))
					;
			}
			else
			{
				_gen
					.ldloc(_locManager)
					.ldstr(((SprocNameAttribute)attrs[0]).Name)
					;
			}
		}

		void CallSetCommand()
		{
			// Parameters.
			//
			LocalBuilder locParams = _gen.DeclareLocal(typeof(object[]));

			_gen
				.ldc_i4(_paramList.Count)
				.newarr(typeof(object))
				.stloc(locParams)
				;

			object[] attrs =
				_methodInfo.DeclaringType.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			bool discoverParams = attrs.Length == 0 ?
				false : ((DiscoverParametersAttribute)attrs[0]).Discover;

			attrs = _methodInfo.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			if (attrs.Length != 0)
				discoverParams = ((DiscoverParametersAttribute)attrs[0]).Discover;

			for (int i = 0; i < _paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)_paramList[i];

				_gen
					.ldloc(locParams)
					.ldc_i4(i)
					;

				if (discoverParams)
				{
					_gen
						.ldarg_s((byte)(pi.Position + 1))
						.BoxIfValueType(pi.ParameterType)
						;
				}
				else
				{
					attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

					string paramName = attrs.Length == 0 ?
						pi.Name : ((ParamNameAttribute)attrs[0]).Name;

					if (paramName[0] != '@')
						paramName = '@' + paramName;

					_gen
						.ldloc_0
						.ldstr(paramName)
						.ldarg_s((byte)(pi.Position + 1))
						.BoxIfValueType(pi.ParameterType)
						.callvirt(typeof(DbManager), "Parameter", typeof(string), typeof(object))
						;
				}

				_gen
					.stelem_ref
					.EndGen()
					;
			}

			// Call SetSpCommand.
			//
			_gen
				.ldloc(locParams)
				.callvirt(typeof(DbManager), "SetSpCommand", _bindingFlags, typeof(string), typeof(object[]))
				;
		}
	}
}
