using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;

using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.DataAccess
{
	public class DataAccessorBuilder : AbstractTypeBuilderBase
	{
		private struct MapOutputParametersValue
		{
			public readonly string        ReturnValueMember;
			public readonly ParameterInfo ParameterInfo;

			public MapOutputParametersValue(string returnValueMember, ParameterInfo parameterInfo)
			{
				ReturnValueMember = returnValueMember;
				ParameterInfo     = parameterInfo;
			}
		}

		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.Priority.DataAccessor;
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context.IsBuildStep)
			{
				if (context.IsAbstractMethod)
				{
					// Give up if there is any builder that builds the method body.
					//
					if (builders.Count > 1)
						foreach (IAbstractTypeBuilder builder in builders)
							if (builder != this && builder.IsApplied(context, builders))
								return false;

					return true;
				}

				// Treat an abstract getter/setter as a regular method
				// when the property has [NoInstance] attribute 
				//
				if (context.IsAbstractGetter || context.IsAbstractSetter)
					return context.CurrentProperty.IsDefined(typeof(NoInstanceAttribute), true);
			}

			return false;
		}

		private Dictionary<Type, Type> _actualTypes;
		private Dictionary<Type, Type>  ActualTypes
		{
			get
			{
				if (_actualTypes == null)
				{
					_actualTypes = new Dictionary<Type,Type>();

					object[] attrs = Context.Type.GetAttributes(typeof(ActualTypeAttribute));

					foreach (ActualTypeAttribute attr in attrs)
						if (!_actualTypes.ContainsKey(attr.BaseType))
							_actualTypes.Add(attr.BaseType, attr.ActualType);
				}

				return _actualTypes;
			}
		}

		const BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		readonly Type     _baseType = typeof(DataAccessor);
		Type              _objectType;
		bool              _explicitObjectType;
		ParameterInfo[]   _parameters;
		ArrayList         _paramList;
		ArrayList         _refParamList;
		bool              _createManager;
		LocalBuilder      _locManager;
		LocalBuilder      _locObjType;
		ArrayList         _outputParameters;
		SqlQueryAttribute _sqlQueryAttribute;
		ArrayList         _formatParamList;
		ParameterInfo     _destination;
		ArrayList         _mapOutputParameters;

		protected override void BuildAbstractMethod()
		{
			// Any class variable must be initialized before use
			// as the same instance of the class is utilized to build abstract methods.
			//
			_paramList           = new ArrayList();
			_refParamList        = new ArrayList();
			_formatParamList     = new ArrayList();
			_mapOutputParameters = new ArrayList();
			_destination         = null;
			_createManager       = true;
			_objectType          = null;
			_explicitObjectType  = false;
			_parameters          = Context.CurrentMethod.GetParameters();
			_locManager          = Context.MethodBuilder.Emitter.DeclareLocal(typeof(DbManager));
			_locObjType          = Context.MethodBuilder.Emitter.DeclareLocal(typeof(Type));
			_outputParameters    = null;
			_sqlQueryAttribute   = null;

			GetSqlQueryAttribute();
			ProcessParameters();
			CreateDbManager();
			SetObjectType();

			// Define execute method type.
			//
			Type returnType = _destination != null? _destination.ParameterType: Context.CurrentMethod.ReturnType;

			if (returnType == typeof(IDataReader))
			{
				ExecuteReader();
			}
			else if (returnType == typeof(DataSet) || returnType.IsSubclassOf(typeof(DataSet)))
			{
				ExecuteDataSet(returnType);
			}
			else if (returnType == typeof(DataTable) || returnType.IsSubclassOf(typeof(DataTable)))
			{
				ExecuteDataTable();
			}
			else if (!returnType.IsArray && (IsInterfaceOf(returnType, typeof(IList))
				|| returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IList<>)))
			{
				if (!_explicitObjectType)
				{
					Type elementType = TypeHelper.GetListItemType(returnType);

					if (elementType == typeof(object) && _destination != null)
						elementType = TypeHelper.GetListItemType(Context.CurrentMethod.ReturnType);

					if (elementType != typeof(object))
						_objectType = elementType;

					if (ActualTypes.ContainsKey(_objectType))
						_objectType = ActualTypes[_objectType];
				}

				if (_objectType == null || _objectType == typeof(object))
					throw new TypeBuilderException(string.Format(
						"Can not determine object type for method '{0}.{1}'",
						Context.CurrentMethod.DeclaringType.Name,
						Context.CurrentMethod.Name));

				if (TypeHelper.IsScalar(_objectType))
					ExecuteScalarList();
				else
					ExecuteList();
			}
			else if (IsInterfaceOf(returnType, typeof(IDictionary))
				|| returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
			{
				Type   elementType = null;
				Type   keyType     = typeof(object);
				Type[] gTypes      = TypeHelper.GetGenericArguments(returnType, typeof(IDictionary));

				if ((gTypes == null || gTypes.Length != 2) && _destination != null)
					gTypes = TypeHelper.GetGenericArguments(_destination.ParameterType, typeof(IDictionary));

				if (gTypes != null && gTypes.Length == 2)
				{
					keyType     = gTypes[0];
					elementType = gTypes[1];
				}

				if (elementType == null || _explicitObjectType)
					elementType = _objectType;

				if (elementType == null || elementType == typeof(object))
					throw new TypeBuilderException(string.Format(
						"Can not determine object type for the method '{0}.{1}'",
						Context.CurrentMethod.DeclaringType.Name,
						Context.CurrentMethod.Name));

				bool isIndex = TypeHelper.IsSameOrParent(typeof(CompoundValue), keyType);

				if (keyType != typeof(object) && !isIndex && !TypeHelper.IsScalar(keyType))
					throw new TypeBuilderException(string.Format(
						"Key type for the method '{0}.{1}' can be of type object, CompoundValue, or a scalar type.",
						Context.CurrentMethod.DeclaringType.Name,
						Context.CurrentMethod.Name));

				MethodInfo mi = Context.CurrentMethod;

				object[] attrs = mi.GetCustomAttributes(typeof(IndexAttribute), true);
				NameOrIndexParameter[] fields = new NameOrIndexParameter[0];

				if (attrs.Length != 0)
					fields = ((IndexAttribute)attrs[0]).Fields;

				if (fields.Length > 1 && keyType != typeof(object) && !isIndex)
					throw new TypeBuilderException(string.Format(
						"Key type for the method '{0}.{1}' can be of type object or CompoundValue.",
						Context.CurrentMethod.DeclaringType.Name,
						Context.CurrentMethod.Name));

				if (TypeHelper.IsScalar(elementType))
				{
					attrs = mi.GetCustomAttributes(typeof(ScalarFieldNameAttribute), true);

					if (attrs.Length == 0)
						throw new TypeBuilderException(string.Format(
							"Scalar field name is not defined for the method '{0}.{1}'.",
							Context.CurrentMethod.DeclaringType.Name,
							Context.CurrentMethod.Name));

					NameOrIndexParameter scalarField = ((ScalarFieldNameAttribute)attrs[0]).NameOrIndex;

					if (fields.Length == 0)
						ExecuteScalarDictionaryWithPK(keyType, scalarField, elementType);
					else if (isIndex || fields.Length > 1)
						ExecuteScalarDictionaryWithMapIndex(fields, scalarField, elementType);
					else
						ExecuteScalarDictionaryWithScalarKey(fields[0], keyType, scalarField, elementType);
				}
				else
				{
					if (!_explicitObjectType && ActualTypes.ContainsKey(elementType))
						elementType = ActualTypes[elementType];

					if (fields.Length == 0)
						ExecuteDictionaryWithPK(keyType, elementType);
					else if (isIndex || fields.Length > 1)
						ExecuteDictionaryWithMapIndex(fields, elementType);
					else
						ExecuteDictionaryWithScalarKey(fields[0], elementType);
				}
			}
			else if (returnType == typeof(void))
			{
				ExecuteNonQuery();
			}
			else if (TypeHelper.IsScalar(returnType.IsByRef? returnType.GetElementType(): returnType))
			{
				ExecuteScalar();
			}
			else
			{
				if (_objectType == null)
					_objectType = returnType;

				if (!_explicitObjectType && ActualTypes.ContainsKey(_objectType))
					_objectType = ActualTypes[_objectType];

				ExecuteObject();
			}

			GetOutRefParameters();
			Finally();
		}

		protected override void BuildAbstractGetter()
		{
			BuildAbstractMethod();
		}

		protected override void BuildAbstractSetter()
		{
			BuildAbstractMethod();
		}

		private void GetSqlQueryAttribute()
		{
			object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(SqlQueryAttribute), true);

			if (attrs.Length != 0)
				_sqlQueryAttribute = (SqlQueryAttribute)attrs[0];
		}

		private void AddParameter(ParameterInfo pi)
		{
			Type pType = pi.ParameterType;

			if (pType.IsByRef)
				pType = pType.GetElementType();

			if (TypeHelper.IsScalar(pType))
				_paramList.Add(pi);
			else if (pType == typeof(DbManager) || pType.IsSubclassOf(typeof(DbManager)))
				_createManager = false;
			else
				_refParamList.Add(pi);
		}

		private void ProcessParameters()
		{
			for (int i = 0; i < _parameters.Length; i++)
			{
				ParameterInfo       pi = _parameters[i];
				NoMapAttribute[] attrs = (NoMapAttribute[])pi.GetCustomAttributes(typeof(NoMapAttribute), true);

				if (attrs.Length == 0)
					AddParameter(pi);
				else
				{
					for(int j = 0; j < attrs.Length; ++j)
					{
						if (!attrs[j].NoMap)
							AddParameter(pi);

						if (attrs[j] is FormatAttribute)
						{
							int index = ((FormatAttribute)attrs[j]).Index;

							if (index < 0)
								index = 0;
							else if (index > _formatParamList.Count)
								index = _formatParamList.Count;

							_formatParamList.Insert(index, pi);
						}
						else if (attrs[j] is DestinationAttribute)
						{
							if (_destination != null)
								throw new TypeBuilderException("More then one parameter is marked as destination");

							_destination = pi;
						}
					}
				}
			}
		}

		private void CreateDbManager()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (_createManager)
			{
				emit
					.ldarg_0
					.callvirt              (_baseType, "GetDbManager")
					.stloc                 (_locManager)
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
						emit
							.ldarg   (_parameters[i])
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
			else
				_explicitObjectType = true;

			if (attrs.Length != 0)
				_objectType = ((ObjectTypeAttribute)attrs[0]).ObjectType;

			if (_objectType == null)
			{
				Type[] types = TypeHelper.GetGenericArguments(mi.DeclaringType, typeof(DataAccessor));

				if (types != null)
					_objectType = types[0];
			}
		}

		#region ExecuteReader

		private void ExecuteReader()
		{
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();

			object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(CommandBehaviorAttribute), true);

			if (attrs.Length == 0)
			{
				Context.MethodBuilder.Emitter
					.callvirt (typeof(DbManager).GetMethod("ExecuteReader", Type.EmptyTypes))
					.stloc    (Context.ReturnValue)
					;
			}
			else
			{
				Context.MethodBuilder.Emitter
					.ldc_i4_  ((int)((CommandBehaviorAttribute)attrs[0]).CommandBehavior)
					.callvirt (typeof(DbManager), "ExecuteReader", typeof(CommandBehavior))
					.stloc    (Context.ReturnValue)
					;
			}
		}

		#endregion

		#region ExecuteDataSet

		private void ExecuteDataSet(Type returnType)
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();

			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (returnType == typeof(DataSet))
			{
				LoadDestinationOrReturnValue();

				object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(DataSetTableAttribute), true);

				if (attrs.Length == 0)
				{
					emit
						.callvirt (typeof(DbManager), "ExecuteDataSet", typeof(DataSet))
						.pop
						.end()
						;
				}
				else
				{
					emit
						.ldNameOrIndex(((DataSetTableAttribute)attrs[0]).NameOrIndex)
						.callvirt (typeof(DbManager), "ExecuteDataSet", typeof(DataSet), typeof(NameOrIndexParameter))
						.pop
						.end()
						;
				}
			}
			else
			{
				emit
					.pop
					.end()
					;

				LoadDestinationOrReturnValue();

				Label l1 = emit.DefineLabel();
				Label l2 = emit.DefineLabel();

				emit
					.callvirt  (typeof(DataSet).GetProperty("Tables").GetGetMethod())
					.callvirt  (typeof(InternalDataCollectionBase).GetProperty("Count").GetGetMethod())
					.ldc_i4_0
					.ble_s(l1)
					.ldloc     (_locManager);

				LoadDestinationOrReturnValue();

				object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(DataSetTableAttribute), true);

				if (attrs.Length == 0)
				{
					LoadDestinationOrReturnValue();

					emit
						.callvirt (typeof(DataSet).GetProperty("Tables").GetGetMethod())
						.ldc_i4_0
						.callvirt (typeof(DataTableCollection), "get_Item", typeof(int))
						.callvirt (typeof(DataTable).GetProperty("TableName").GetGetMethod())
						.call     (typeof(NameOrIndexParameter), "op_Implicit", typeof(string))
						;
				}
				else
				{
					emit
						.ldNameOrIndex(((DataSetTableAttribute)attrs[0]).NameOrIndex)
						;
				}

				emit
					.callvirt  (typeof(DbManager), "ExecuteDataSet", typeof(DataSet), typeof(NameOrIndexParameter))
					.pop
					.br_s      (l2)
					.MarkLabel (l1)
					.ldloc     (_locManager);

				LoadDestinationOrReturnValue();

				emit
					.callvirt  (typeof(DbManager), "ExecuteDataSet", typeof(DataSet))
					.pop
					.MarkLabel (l2)
					;
			}
		}

		#endregion

		#region ExecuteDataTable

		private void ExecuteDataTable()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			EmitHelper emit = Context.MethodBuilder.Emitter;

			emit
				.callvirt (typeof(DbManager), "ExecuteDataTable", typeof(DataTable))
				.pop
				.end()
				;

			// When DataSetTableAttribute is present, simply set table name to the name specified.
			//
			object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(DataSetTableAttribute), true);

			if (attrs.Length != 0)
			{
				DataSetTableAttribute attr = (DataSetTableAttribute)attrs[0];

				if (!attr.NameOrIndex.ByName)
					throw new TypeBuilderException(string.Format(
						"DataSetTable attribute for method '{0}.{1}' may not be an index",
						Context.CurrentMethod.DeclaringType.Name, Context.CurrentMethod.Name));

				LoadDestinationOrReturnValue();

				emit
					.ldstr(attr.NameOrIndex.Name)
					.callvirt (typeof(DataTable), "set_TableName", typeof(string))
					;
			}
		}

		#endregion

		#region ExecuteScalarList

		private void ExecuteScalarList()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(ScalarFieldNameAttribute), true);

			if (attrs.Length == 0)
			{
				Context.MethodBuilder.Emitter
					.ldloc(_locObjType)
					.callvirt(typeof(DbManager), "ExecuteScalarList", typeof(IList), typeof(Type))
					.pop
					.end()
					;
			}
			else
			{
				Context.MethodBuilder.Emitter
					.ldloc(_locObjType)
					.ldNameOrIndex(((ScalarFieldNameAttribute)attrs[0]).NameOrIndex)
					.callvirt(typeof(DbManager), "ExecuteScalarList", typeof(IList), typeof(Type), typeof(NameOrIndexParameter))
					.pop
					.end()
					;
			}
		}

		private void ExecuteList()
		{
			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			Context.MethodBuilder.Emitter
				.ldloc    (_locObjType)
				.callvirt (typeof(DbManager), "ExecuteList", typeof(IList), typeof(Type))
				.pop
				.end()
				;
		}

		#endregion

		#region ExecuteDictionary

		public FieldBuilder GetIndexField(NameOrIndexParameter[] namesOrIndexes)
		{
			string id = "index$" + string.Join("%",
				Array.ConvertAll<NameOrIndexParameter, string>(namesOrIndexes,
					delegate(NameOrIndexParameter nameOrIndex)
					{
						return nameOrIndex.ToString();
					}));

			FieldBuilder fieldBuilder = Context.GetField(id);

			if (fieldBuilder == null)
			{
				fieldBuilder = Context.CreatePrivateStaticField(id, typeof(MapIndex));

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				emit
					.ldc_i4_ (namesOrIndexes.Length)
					.newarr  (typeof(NameOrIndexParameter))
					;

				for (int i = 0; i < namesOrIndexes.Length; i++)
				{
					emit
						.dup
						.ldc_i4_(i)
						.ldelema(typeof(NameOrIndexParameter));

					if (namesOrIndexes[i].ByName)
					{
						emit
							.ldstr(namesOrIndexes[i].Name)
							.call(typeof(NameOrIndexParameter), "op_Implicit", typeof(string));
					}
					else
					{
						emit
							.ldc_i4_(namesOrIndexes[i].Index)
							.call(typeof(NameOrIndexParameter), "op_Implicit", typeof(int));
					}

					emit
						.stobj(typeof(NameOrIndexParameter))
						.end()
						;
				}
				
				emit
					.newobj (typeof(MapIndex), typeof(NameOrIndexParameter[]))
					.stsfld (fieldBuilder)
					;
			}

			return fieldBuilder;
		}

		/// <summary>
		/// Maps primary keys(s) to a scalar field.
		/// </summary>
		private void ExecuteScalarDictionaryWithPK(
			Type                 keyType,
			NameOrIndexParameter scalarField,
			Type                 elementType)
		{
			CreateReturnTypeInstance();
			InitObjectType();

			Context.MethodBuilder.Emitter
				.ldarg_0
				.end()
				;

			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			Context.MethodBuilder.Emitter
				.ldloc         (_locObjType)
				.LoadType      (keyType)
				.ldstr         (Context.CurrentMethod.Name)
				.ldNameOrIndex (scalarField)
				.LoadType      (elementType)
				.callvirt      (_baseType, "ExecuteScalarDictionary", _bindingFlags,
					            typeof(DbManager), typeof(IDictionary), typeof(Type),
					            typeof(Type), typeof(string), typeof(NameOrIndexParameter), typeof(Type))
				;
		}

		/// <summary>
		/// Maps a complex index to a scalar field.
		/// </summary>
		private void ExecuteScalarDictionaryWithMapIndex(
			NameOrIndexParameter[] index,
			NameOrIndexParameter   scalarField,
			Type                   elementType)
		{
			_objectType = elementType;

			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			Context.MethodBuilder.Emitter
				.ldsfld        (GetIndexField(index))
				.ldNameOrIndex (scalarField)
				.ldloc         (_locObjType)
				.callvirt      (typeof(DbManager), "ExecuteScalarDictionary",
				                typeof(IDictionary), typeof(MapIndex),
				                typeof(NameOrIndexParameter), typeof(Type))
				.pop
				.end()
				;
		}

		/// <summary>
		/// Maps any single field to any (other) single field.
		/// </summary>
		private void ExecuteScalarDictionaryWithScalarKey(
			NameOrIndexParameter keyField,    Type keyType,
			NameOrIndexParameter scalarField, Type elementType)
		{
			_objectType = elementType;

			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			Context.MethodBuilder.Emitter
				.ldNameOrIndex (keyField)
				.LoadType      (keyType)
				.ldNameOrIndex (scalarField)
				.ldloc         (_locObjType)
				.callvirt      (typeof(DbManager), "ExecuteScalarDictionary",
					            typeof(IDictionary), typeof(NameOrIndexParameter), typeof(Type),
					            typeof(NameOrIndexParameter), typeof(Type))
				.pop
				.end()
				;
		}

		/// <summary>
		/// Maps primary keys(s) to an object of the specified type.
		/// </summary>
		private void ExecuteDictionaryWithPK(
			Type keyType,
			Type elementType)
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			_objectType = elementType;
			
			CreateReturnTypeInstance();
			InitObjectType();

			emit
				.ldarg_0
				.end()
				;

			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			if (IsGenericDestinationOrReturnValue())
			{
				Type[]     genericArgs = Context.ReturnValue.LocalType.GetGenericArguments();
				Type[]     types       = new Type[]
					{
						typeof(DbManager),
						typeof(IDictionary<,>).MakeGenericType(genericArgs),
						typeof(Type),
						typeof(string),
					};
				MethodInfo method = _baseType.GetMethod("ExecuteDictionary",
					_bindingFlags, GenericBinder.Generic, types, null);

				if (TypeHelper.IsSameOrParent(typeof(CompoundValue), genericArgs[0]))
					method = method.MakeGenericMethod(genericArgs[1]);
				else
					method = method.MakeGenericMethod(genericArgs);

				emit
					.ldloc    (_locObjType)
					.ldstr    (Context.CurrentMethod.Name)
					.callvirt (method)
					;
			}
			else

				emit
					.ldloc    (_locObjType)
					.LoadType (keyType)
					.ldstr    (Context.CurrentMethod.Name)
					.callvirt (_baseType, "ExecuteDictionary", _bindingFlags,
							   typeof(DbManager), typeof(IDictionary), typeof(Type),
							   typeof(Type), typeof(string))
					;
		}

		/// <summary>
		/// Maps a complex index to an object of the specified type.
		/// </summary>
		private void ExecuteDictionaryWithMapIndex(
			NameOrIndexParameter[] index,
			Type                   elementType)
		{
			_objectType = elementType;

			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			Context.MethodBuilder.Emitter
				.ldsfld   (GetIndexField(index))
				.ldloc    (_locObjType)
				.ldnull
				.callvirt (typeof(DbManager), "ExecuteDictionary",
					       typeof(IDictionary), typeof(MapIndex), typeof(Type), typeof(object[]))
				.pop
				.end()
				;
		}

		/// <summary>
		/// Maps any single field to object type.
		/// </summary>
		private void ExecuteDictionaryWithScalarKey(
			NameOrIndexParameter keyField,
			Type                 elementType)
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			_objectType = elementType;

			CreateReturnTypeInstance();
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();
			LoadDestinationOrReturnValue();

			if (IsGenericDestinationOrReturnValue())
			{
				Type[]     genericArgs = Context.ReturnValue.LocalType.GetGenericArguments();
				Type[]     types       = new Type[]
					{
						typeof(IDictionary<,>).MakeGenericType(genericArgs),
						typeof(NameOrIndexParameter),
						typeof(Type),
						typeof(object[]),
					};
				MethodInfo method = typeof(DbManager).GetMethod("ExecuteDictionary", _bindingFlags, GenericBinder.Generic, types, null)
					.MakeGenericMethod(genericArgs);

				emit
					.ldNameOrIndex(keyField)
					.ldloc        (_locObjType)
					.ldnull
					.callvirt     (method)
					.pop
					.end()
					;
			}
			else
			{
				emit
					.ldNameOrIndex(keyField)
					.ldloc        (_locObjType)
					.ldnull
					.callvirt     (typeof(DbManager), "ExecuteDictionary", typeof(IDictionary), typeof(NameOrIndexParameter), typeof(Type), typeof(object[]))
					.pop
					.end()
					;
			}
		}

		#endregion

		#region ExecuteNonQuery

		public void ExecuteNonQuery()
		{
			if (_destination != null)
				throw new TypeBuilderException("ExecuteNonQuery does not support the Destination attribute");

			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();

			MethodInfo   mi      = typeof(DbManager).GetMethod("ExecuteNonQuery", Type.EmptyTypes);
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

		#endregion

		#region ExecuteScalar

		public void ExecuteScalar()
		{
			EmitHelper emit       = Context.MethodBuilder.Emitter;
			Type       returnType = Context.CurrentMethod.ReturnType;
			Type       scalarType;

			if (_destination != null)
			{
				if (_destination.ParameterType.IsByRef)
					scalarType = _destination.ParameterType.GetElementType();
				else
					throw new TypeBuilderException("ExecuteScalar destination must be an out or a ref parameter.");

				if (returnType != typeof(void) && !TypeHelper.IsSameOrParent(returnType, scalarType))
				{
					// object Foo(out int num) is valid,
					// IConvertible Foo(ref int num) is also ok,
					// but string Bar(out DateTime dt) is not
					//
					throw new TypeBuilderException(string.Format(
						"The return type '{0}' of the method '{1}' is incompatible with the destination parameter type '{2}'.",
						returnType.FullName, Context.CurrentMethod.Name, scalarType.FullName));
				}
			}
			else
				scalarType = returnType;

			if (_destination != null)
				emit
					.ldarg (_destination)
					;

			emit
				.ldarg_0
				.ldloc     (_locManager)
				;

			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();

			object[] attrs  = Context.CurrentMethod.GetCustomAttributes(typeof(ScalarSourceAttribute), true);

			if (attrs.Length == 0)
			{
				emit
					.callvirtNoGenerics   (typeof(DbManager), "ExecuteScalar")
					;
			}
			else
			{
				ScalarSourceAttribute attr = (ScalarSourceAttribute)attrs[0];

				emit
					.ldc_i4_            ((int)attr.ScalarType)
					.ldNameOrIndex      (attr.NameOrIndex)
					.callvirtNoGenerics (typeof(DbManager), "ExecuteScalar", typeof(ScalarSourceType), typeof(NameOrIndexParameter));
			}

			string converterName = GetConverterMethodName(scalarType);

			if (converterName == null)
			{
				emit
					.LoadType           (scalarType)
					.ldnull
					.callvirt           (_baseType, "ConvertChangeType", _bindingFlags, typeof (DbManager), typeof (object), typeof (Type), typeof (object))
					;
				
			}
			else
			{
				emit
					.ldnull
					.callvirt           (_baseType, converterName, _bindingFlags, typeof(DbManager), typeof(object), typeof(object))
					;
			}

			if (_destination != null)
			{
				emit
					.stind              (scalarType)
					;

				// The return value and a destination both are present
				//
				if (Context.ReturnValue != null)
				{
					emit
						.ldargEx        (_destination, false)
						;

					if (scalarType != returnType)
						emit
							.boxIfValueType (scalarType)
							.CastFromObject (returnType)
							;

					emit.stloc          (Context.ReturnValue)
					;
				}
			}
			else
				emit
					.stloc              (Context.ReturnValue)
					;
		}

		#endregion

		#region ExecuteObject

		public void ExecuteObject()
		{
			InitObjectType();
			GetSprocNameOrSqlQueryTest();
			CallSetCommand();

			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (_destination != null)
			{
				emit
					.ldarg(_destination)
					.callvirt(typeof(DbManager), "ExecuteObject", typeof(Object))
					;
			}
			else
			{
				emit
					.ldloc(_locObjType)
					.callvirt(typeof(DbManager), "ExecuteObject", typeof(Type))
					;
			}

			if (null != Context.ReturnValue)
			{
				emit
					.castclass(_objectType)
					.stloc(Context.ReturnValue)
					;
			}
			else
			{
				emit
					.pop
					.end()
					;
			}
		}

		#endregion

		private void Finally()
		{
			if (_createManager)
			{
				Context.MethodBuilder.Emitter
					.BeginFinallyBlock()
					.ldarg_0
					.ldloc    (_locManager)
					.callvirt (_baseType, "Dispose", _bindingFlags, typeof(DbManager))
					.EndExceptionBlock()
					;
			}
		}

		private void CreateReturnTypeInstance()
		{
			if (null == Context.ReturnValue)
				return;

			if (null != _destination)
			{
				Context.MethodBuilder.Emitter
					.ldarg    (_destination)
					.stloc    (Context.ReturnValue)
					;
			}
			else
			{
				Type returnType = Context.CurrentMethod.ReturnType;

				if (returnType.IsInterface)
				{
					if (IsInterfaceOf(returnType, typeof(IList)))
						returnType = typeof(ArrayList);
					else if (IsInterfaceOf(returnType, typeof(IDictionary)))
						returnType = typeof(Hashtable);
					else if (returnType.GetGenericTypeDefinition() == typeof(IList<>))
						returnType = typeof(List<>).MakeGenericType(returnType.GetGenericArguments());
					else if (returnType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
						returnType = typeof(Dictionary<,>).MakeGenericType(returnType.GetGenericArguments());
				}

				ConstructorInfo ci = TypeHelper.GetDefaultConstructor(returnType);

				if (ci == null)
					throw new TypeBuilderException(string.Format("Cannot create an instance of the type '{0}'",
						Context.CurrentMethod.ReturnType.FullName));

				Context.MethodBuilder.Emitter
					.newobj   (ci)
					.stloc    (Context.ReturnValue)
					;
			}
		}

		private void LoadDestinationOrReturnValue()
		{
			if (_destination != null)
				Context.MethodBuilder.Emitter.ldarg(_destination);
			else
				Context.MethodBuilder.Emitter.ldloc(Context.ReturnValue);
		}

		private bool IsGenericDestinationOrReturnValue()
		{
			return _destination == null?
				Context.ReturnValue.LocalType.IsGenericType:
				_destination.ParameterType.IsGenericType;
		}

		private void InitObjectType()
		{
			if (_objectType == null)
			{
				Context.MethodBuilder.Emitter
					.ldnull
					.stloc    (_locObjType)
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

		static int _nameCounter;
		static int _uniqueQueryID;

		private void GetSprocNameOrSqlQueryTest()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (_sqlQueryAttribute != null)
			{
				emit
					.ldloc (_locManager)
					;

				if (_sqlQueryAttribute.ID != int.MinValue)
				{
					emit
						.ldarg_0
						.ldloc   (_locManager)
						.ldc_i4_ (_sqlQueryAttribute.ID)
						.ldc_i4_ (++_uniqueQueryID)
						;
				}

				if (_sqlQueryAttribute.IsDynamic)
				{
					Type         attrType = typeof(SqlQueryAttribute);
					FieldBuilder field    = Context.CreatePrivateStaticField(attrType + "$" + ++_nameCounter, attrType);
					Label        isNull   = emit.DefineLabel();

					emit
						.ldsfld    (field)
						.brtrue_s  (isNull)

						.ldarg_0
						.call      (typeof(MethodBase), "GetCurrentMethod")
						.castclass (typeof(MethodInfo))
						.callvirt  (_baseType, "GetSqlQueryAttribute", _bindingFlags, typeof(MethodInfo))

						.stsfld    (field)
						.MarkLabel (isNull)

						.ldsfld    (field)
						.ldarg_0
						.ldloc     (_locManager)
						.callvirt  (attrType, "GetSqlText", _bindingFlags, typeof(DataAccessor), typeof(DbManager))
						;
				}
				else
				{
					emit
						.ldstr (_sqlQueryAttribute.SqlText)
						;
				}

				if (_sqlQueryAttribute.ID != int.MinValue)
				{
					emit
						.callvirt (_baseType, "PrepareSqlQuery", _bindingFlags,
							typeof(DbManager), typeof(int), typeof(int), typeof(string))
						;
				}
			}
			else
			{
				object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(SprocNameAttribute), true);

				if (attrs.Length == 0)
				{
					attrs = Context.CurrentMethod.GetCustomAttributes(typeof(ActionNameAttribute), true);

					string actionName = attrs.Length == 0 ?
						Context.CurrentMethod.Name : ((ActionNameAttribute)attrs[0]).Name;

					// Call GetSpName.
					//
					emit
						.ldloc    (_locManager)
						.ldarg_0
						.ldloc    (_locObjType)
						.ldstr    (actionName)
						.callvirt (_baseType, "GetSpName", _bindingFlags, typeof(Type), typeof(string))
						;
				}
				else
				{
					emit
						.ldloc    (_locManager)
						.ldstr    (((SprocNameAttribute)attrs[0]).Name)
						;
				}
			}

			// string.Format
			//
			if (_formatParamList.Count > 0)
			{
				emit
					.ldc_i4_      (_formatParamList.Count)
					.newarr       (typeof(object))
					;

				for (int i = 0; i < _formatParamList.Count; i++)
				{
					ParameterInfo pi = (ParameterInfo)_formatParamList[i];

					emit
						.dup
						.ldc_i4_        (i)
						.ldarg          (pi)
						.boxIfValueType (pi.ParameterType)
						.stelem_ref
						.end()
						;
				}

				emit
					.call (typeof(string), "Format", typeof(string), typeof(object[]))
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

			bool discoverParams = false;

			if (_sqlQueryAttribute == null)
			{
				discoverParams = attrs.Length == 0?
					false: ((DiscoverParametersAttribute)attrs[0]).Discover;

				attrs = Context.CurrentMethod.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

				if (attrs.Length != 0)
					discoverParams = ((DiscoverParametersAttribute)attrs[0]).Discover;
			}

			LocalBuilder locParams = discoverParams?
				BuildParametersWithDiscoverParameters():
				BuildParameters();

			// Call SetSpCommand.
			//
			string methodName = _sqlQueryAttribute == null? "SetSpCommand":   "SetCommand";
			Type   paramType  = _sqlQueryAttribute == null? typeof(object[]): typeof(IDbDataParameter[]);

			emit
				.ldloc    (locParams)
				.callvirt (typeof(DbManager), methodName, _bindingFlags, typeof(string), paramType)
				;
		}

		private LocalBuilder BuildParameters()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			LocalBuilder retParams = emit
				.DeclareLocal(typeof(IDbDataParameter[]));

			LocalBuilder locParams = _refParamList.Count > 0?
				BuildRefParameters():
				BuildSimpleParameters();

			emit
				.ldarg_0
				.ldloc    (_locManager)
				.ldloc    (locParams)
				.callvirt (_baseType, "PrepareParameters", _bindingFlags, typeof(DbManager), typeof(object[]))
				.stloc    (retParams)
				;

			return retParams;
		}

		private LocalBuilder BuildSimpleParameters()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			// Parameters.
			//
			LocalBuilder locParams = emit.DeclareLocal(
				_sqlQueryAttribute == null? typeof(object[]): typeof(IDbDataParameter[]));

			emit
				.ldc_i4_ (_paramList.Count)
				.newarr  (_sqlQueryAttribute == null? typeof(object): typeof(IDbDataParameter))
				;

			for (int i = 0; i < _paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)_paramList[i];

				emit
					.dup
					.ldc_i4_ (i)
					;

				BuildParameter(pi);

				emit
					.stelem_ref
					.end()
					;
			}

			emit.stloc(locParams);
			return locParams;
		}

		private FieldBuilder CreateStringArrayField(object[] attrs)
		{
			if (attrs.Length == 0)
				return null;

			List<string> list = new List<string>();

			foreach (Direction attr in attrs)
				if (attr.Members != null)
					list.AddRange(attr.Members);

			if (list.Count == 0)
				return null;

			list.Sort(string.CompareOrdinal);

			string[] strings = list.ToArray();

			// There a no limit for a field name length, but Visual Studio Debugger
			// may crash on fields with name longer then 256 symbols.
			//
			string       key            = "_string_array$" + string.Join("%", strings);
			FieldBuilder fieldBuilder   = Context.GetField(key);

			if (null == fieldBuilder)
			{
				fieldBuilder = Context.CreatePrivateStaticField(key, typeof(string[]));

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				emit
					.ldc_i4_ (strings.Length)
					.newarr  (typeof(string))
					;

				for (int i = 0; i < strings.Length; i++)
				{
					emit
						.dup
						.ldc_i4_ (i)
						.ldstr   (strings[i])
						.stelem_ref
						.end()
						;
				}
				
				emit
					.stsfld (fieldBuilder)
					;
			}

			return fieldBuilder;
		}

		private FieldBuilder CreateNullValueField(Type type, string value)
		{
			string       key          = "_null_value$" + type.FullName + "%" + value;
			FieldBuilder fieldBuilder = Context.GetField(key);

			if (null == fieldBuilder)
			{
				fieldBuilder = Context.CreatePrivateStaticField(key, type);

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				emit
					.LoadType  (type)
					.call      (typeof(TypeDescriptor), "GetConverter", typeof(Type))
					.ldstr     (value)
					.callvirt  (typeof(TypeConverter), "ConvertFromInvariantString", typeof(string))
					.unbox_any (type)
					.stsfld    (fieldBuilder)
					;
			}

			return fieldBuilder;
		}

		private LocalBuilder BuildRefParameters()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			// Parameters.
			//
			LocalBuilder locParams = emit.DeclareLocal(typeof(object[]));

			emit
				.ldc_i4_ (_parameters.Length)
				.newarr  (typeof(object))
				;

			for (int i = 0; i < _parameters.Length; i++)
			{
				ParameterInfo pi = _parameters[i];

				emit
					.dup
					.ldc_i4_ (i)
					;

				if (_paramList.Contains(pi))
				{
					BuildParameter(pi);
				}
				else if (_refParamList.Contains(pi))
				{
					bool         mapOutputParameters = false;
					string       returnValueMember   = null;
					FieldBuilder fieldBuilder;
					Type         type =
						pi.ParameterType == typeof(DataRow) || pi.ParameterType.IsSubclassOf(typeof(DataRow))?
							typeof(DataRow): typeof(object);

					emit
						.ldarg_0
						.ldloc   (_locManager)
						.ldarg   (pi)
						;

					fieldBuilder = CreateStringArrayField(pi.GetCustomAttributes(typeof(Direction.OutputAttribute), true));

					if (fieldBuilder != null)
					{
						emit.ldsfld (fieldBuilder);
						mapOutputParameters = true;
					}
					else
						emit.ldnull.end();

					fieldBuilder = CreateStringArrayField(pi.GetCustomAttributes(typeof(Direction.InputOutputAttribute), true));

					if (fieldBuilder != null)
					{
						emit.ldsfld (fieldBuilder);
						mapOutputParameters = true;
					}
					else
						emit.ldnull.end();

					fieldBuilder = CreateStringArrayField(pi.GetCustomAttributes(typeof(Direction.IgnoreAttribute), true));

					if (fieldBuilder != null)
						emit.ldsfld (fieldBuilder);
					else
						emit.ldnull.end();

					emit
						.ldnull
						.callvirt (_baseType, "CreateParameters", _bindingFlags,
							typeof(DbManager), type, typeof(string[]), typeof(string[]), typeof(string[]), typeof(IDbDataParameter[]))
						;

					object[] attrs = pi.GetCustomAttributes(typeof (Direction.ReturnValueAttribute), true);

					if (attrs.Length != 0)
						returnValueMember = ((Direction.ReturnValueAttribute)attrs[0]).Member;

					if (null != returnValueMember || mapOutputParameters)
						_mapOutputParameters.Add(new MapOutputParametersValue(returnValueMember, pi));
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

			emit.stloc(locParams);
			return locParams;
		}

		private void LoadParameterOrNull(ParameterInfo pi, Type type)
		{
			EmitHelper emit  = Context.MethodBuilder.Emitter;
			object[]   attrs = pi.GetCustomAttributes(typeof(ParamNullValueAttribute), true);

			object nullValue = attrs.Length == 0?
				null: ((ParamNullValueAttribute)attrs[0]).Value;

			Label labelNull  = emit.DefineLabel();
			Label labelEndIf = emit.DefineLabel();

			if (pi.Attributes == ParameterAttributes.Out)
			{
				emit
					.ldnull
					.end()
					;

				return;
			}

			if (nullValue != null)
			{
				Type nullValueType = type;
				bool isNullable    = TypeHelper.IsNullable(type);

				if (type.IsEnum)
				{
					nullValueType = Enum.GetUnderlyingType(type);
					nullValue     = System.Convert.ChangeType(nullValue, nullValueType);
				}
				else if (isNullable)
				{
					nullValueType = type.GetGenericArguments()[0];

					emit
						.ldarga  (pi)
						.call    (type, "get_HasValue")
						.brfalse (labelNull)
						;
				}

				if (nullValueType == nullValue.GetType() && emit.LoadWellKnownValue(nullValue))
				{
					if (nullValueType == typeof(string))
						emit
							.ldarg  (pi)
							.call   (nullValueType, "Equals", nullValueType)
							.brtrue (labelNull)
							;
					else if (isNullable)
						emit
							.ldarga (pi)
							.call   (type, "get_Value")
							.beq    (labelNull)
							;
					else
						emit
							.ldarg  (pi)
							.beq    (labelNull)
						;
				}
				else
				{
					string       nullString  = TypeDescriptor.GetConverter(nullValue).ConvertToInvariantString(nullValue);
					FieldBuilder staticField = CreateNullValueField(nullValueType, nullString);
					MethodInfo   miEquals    = new TypeHelper(nullValueType).GetPublicMethod("Equals", nullValueType);

					if (miEquals == null)
					{
						// Is it possible?
						//
						throw new TypeBuilderException(string.Format(
							"The type '{0}' does not have 'Equals' method", type.FullName));
					}

					if (isNullable)
						emit
							.ldsflda (staticField)
							.ldarga  (pi)
							.call    (pi.ParameterType, "get_Value")
							;
					else
						emit
							.ldsflda (staticField)
							.ldarg   (pi)
						;

					if (miEquals.GetParameters()[0].ParameterType.IsClass)
						emit
							.boxIfValueType(nullValueType)
							;

					emit
						.call   (miEquals)
						.brtrue (labelNull)
						;
				}
			}

			if (type.IsEnum)
				emit
					.ldloc     (_locManager)
					.callvirt  (typeof (DbManager).GetProperty("MappingSchema").GetGetMethod())
					;

			emit
				.ldargEx(pi, true)
				;

			if (type.IsEnum)
				emit
					.ldc_i4_1
					.callvirt  (typeof (MappingSchema), "MapEnumToValue", typeof (object), typeof (bool))
					;

			if (nullValue != null)
			{
				emit
					.br        (labelEndIf)
					.MarkLabel (labelNull)
					.ldnull
					.MarkLabel (labelEndIf)
					;
			}
		}

		private void BuildParameter(ParameterInfo pi)
		{
			EmitHelper emit  = Context.MethodBuilder.Emitter;
			object[]   attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

			string methodName;
			string paramName = attrs.Length == 0?
				pi.Name: ((ParamNameAttribute)attrs[0]).Name;

			emit.ldloc        (_locManager);

			if (paramName[0] != '@')
			{
				methodName = _sqlQueryAttribute == null? "GetSpParameterName": "GetQueryParameterName";
				emit
					.ldarg_0
					.ldloc    (_locManager)
					.ldstr    (paramName)
					.callvirt (_baseType, methodName, _bindingFlags, typeof(DbManager), typeof(string))
					;
			}
			else
				emit.ldstr    (paramName);

			Type type  = pi.ParameterType;
			methodName = "Parameter";

			if (type.IsByRef)
			{
				if (_outputParameters == null)
					_outputParameters = new ArrayList();

				_outputParameters.Add(pi);

				type       = type.GetElementType();
				methodName = pi.IsOut? "OutputParameter": "InputOutputParameter";
			}

			LoadParameterOrNull(pi, type);

			emit
				.callvirt          (typeof(DbManager), methodName, typeof(string), typeof(object))
				;

			// Check if parameter type/size is specified.
			//
			attrs = pi.GetCustomAttributes(typeof(ParamDbTypeAttribute), true);
			if (attrs.Length > 0)
			{
				emit
					.dup
					.ldc_i4_       ((int)((ParamDbTypeAttribute)attrs[0]).DbType)
					.callvirt      (typeof(IDataParameter), "set_DbType", typeof(DbType))
					;
			}

			attrs = pi.GetCustomAttributes(typeof(ParamSizeAttribute), true);
			if (attrs.Length > 0)
			{
				emit
					.dup
					.ldc_i4_       (((ParamSizeAttribute)attrs[0]).Size)
					.callvirt      (typeof(IDbDataParameter), "set_Size", typeof(int))
					;
			}
		}

		private LocalBuilder BuildParametersWithDiscoverParameters()
		{
			EmitHelper   emit      = Context.MethodBuilder.Emitter;
			LocalBuilder locParams = emit.DeclareLocal(typeof(object[]));

			emit
				.ldc_i4_     (_paramList.Count)
				.newarr      (typeof(object))
				;

			for (int i = 0; i < _paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)_paramList[i];

				emit
					.dup
					.ldc_i4_ (i)
					;

				LoadParameterOrNull(pi, pi.ParameterType);

				emit
					.stelem_ref
					.end()
					;
			}

			emit.stloc(locParams);
			return locParams;
		}

		private void StoreParameterValue(LocalBuilder param, ParameterInfo pi, Type type)
		{
			EmitHelper emit       = Context.MethodBuilder.Emitter;
			Label      labelNull  = emit.DefineLabel();
			Label      labelEndIf = emit.DefineLabel();

			object[]   attrs      = pi.GetCustomAttributes(typeof(ParamNullValueAttribute), true);
			object     nullValue  = attrs.Length == 0?
				null: ((ParamNullValueAttribute)attrs[0]).Value;

			if (nullValue != null)
			{
				emit
					.ldarg_0
					.ldloc              (_locManager)
					.ldloc              (param)
					.callvirt           (typeof(IDataParameter).GetProperty("Value").GetGetMethod())
					.ldloc              (param)
					.callvirt           (_baseType, "IsNull", _bindingFlags, typeof(DbManager), typeof(object), typeof(object))
					.brtrue             (labelNull)
					;
			}

			if (type.IsEnum)
			{
					emit
						.ldloc          (_locManager)
						.callvirt       (typeof(DbManager).GetProperty("MappingSchema").GetGetMethod())
						.ldloc          (param)
						.callvirt       (typeof(IDataParameter).GetProperty("Value").GetGetMethod())
						.LoadType       (type)
						.callvirt       (typeof(MappingSchema), "MapValueToEnum", typeof(object), typeof(Type))
						.CastFromObject (type)
						;
			}
			else
			{
				emit
					.ldarg_0
					.ldloc              (_locManager)
					.ldloc              (param)
					.callvirt           (typeof(IDataParameter).GetProperty("Value").GetGetMethod())
					;
				
				string converterName = GetConverterMethodName(type);

				if (converterName == null)
				{
					emit
						.LoadType       (type)
						.ldloc          (param)
						.callvirt       (_baseType, "ConvertChangeType", _bindingFlags, typeof(DbManager), typeof(object), typeof(Type), typeof(object))
						;
				}
				else
				{
					emit
						.ldloc          (param)
						.callvirt       (_baseType, converterName, _bindingFlags, typeof(DbManager), typeof(object), typeof(object))
						;
				}
			}

			if (nullValue != null)
			{
				emit
					.br                 (labelEndIf)
					.MarkLabel          (labelNull);

				if (nullValue.GetType() != type || !emit.LoadWellKnownValue(nullValue))
				{
					string       nullString  = TypeDescriptor.GetConverter(type).ConvertToInvariantString(nullValue);
					FieldBuilder staticField = CreateNullValueField(type, nullString);

					emit
						.ldsfld         (staticField)
						;
				}

				emit
					.MarkLabel          (labelEndIf)
					;
			}

			emit.stind(type);
		}

		private void GetOutRefParameters()
		{
			EmitHelper emit = Context.MethodBuilder.Emitter;

			if (_outputParameters != null)
			{
				LocalBuilder param = emit.DeclareLocal(typeof(IDataParameter));

				foreach (ParameterInfo pi in _outputParameters)
				{
					Type type = pi.ParameterType.GetElementType();

					emit
						.ldarg(pi)
						;

					// Get parameter.
					//
					object[] attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

					string paramName = attrs.Length == 0 ?
						pi.Name : ((ParamNameAttribute)attrs[0]).Name;

					emit
						.ldarg_0
						.ldloc        (_locManager)
						;

					if (paramName[0] != '@')
					{
						string methodName = _sqlQueryAttribute == null? "GetSpParameterName": "GetQueryParameterName";

						emit
							.ldarg_0
							.ldloc    (_locManager)
							.ldstr    (paramName)
							.callvirt (_baseType, methodName, _bindingFlags, typeof(DbManager), typeof(string))
							;
					}
					else
						emit.ldstr    (paramName);

					emit
						.callvirt     (_baseType, "GetParameter", _bindingFlags, typeof(DbManager), typeof(string))
						.stloc        (param)
						;

					StoreParameterValue(param, pi, type);
				}
			}

			foreach (MapOutputParametersValue v in _mapOutputParameters)
			{
				emit
					.ldloc    (_locManager)
					.ldstrEx  (v.ReturnValueMember)
					.ldarg    (v.ParameterInfo)
					.callvirt (typeof(DbManager), "MapOutputParameters", typeof(string), typeof(object));
			}
		}

		private static bool IsInterfaceOf(Type type, Type interfaceType)
		{
			Type[] types = type.GetInterfaces();

			foreach (Type t in types)
				if (t == interfaceType)
					return true;

			return type == interfaceType;
		}

		private static string GetConverterMethodName(Type type)
		{
			Type underlyingType = Nullable.GetUnderlyingType(type);

			if (underlyingType != null)
			{
				if (underlyingType.IsEnum)
					underlyingType = Enum.GetUnderlyingType(underlyingType);

				if (underlyingType == typeof(Int16))          return "ConvertToNullableInt16";
				if (underlyingType == typeof(Int32))          return "ConvertToNullableInt32";
				if (underlyingType == typeof(Int64))          return "ConvertToNullableInt64";
				if (underlyingType == typeof(Byte))           return "ConvertToNullableByte";
				if (underlyingType == typeof(UInt16))         return "ConvertToNullableUInt16";
				if (underlyingType == typeof(UInt32))         return "ConvertToNullableUInt32";
				if (underlyingType == typeof(UInt64))         return "ConvertToNullableUInt64";
				if (underlyingType == typeof(Char))           return "ConvertToNullableChar";
				if (underlyingType == typeof(Double))         return "ConvertToNullableDouble";
				if (underlyingType == typeof(Single))         return "ConvertToNullableSingle";
				if (underlyingType == typeof(Boolean))        return "ConvertToNullableBoolean";
				if (underlyingType == typeof(DateTime))       return "ConvertToNullableDateTime";
#if FW3
				if (underlyingType == typeof(DateTimeOffset)) return "ConvertToNullableDateTimeOffset";
#endif
				if (underlyingType == typeof(Decimal))        return "ConvertToNullableDecimal";
				if (underlyingType == typeof(Guid))           return "ConvertToNullableGuid";
			}

			if (type.IsPrimitive)
			{
				if (type.IsEnum)
					type = Enum.GetUnderlyingType(type);

				if (type == typeof(SByte))      return "ConvertToSByte";
				if (type == typeof(Int16))      return "ConvertToInt16";
				if (type == typeof(Int32))      return "ConvertToInt32";
				if (type == typeof(Int64))      return "ConvertToInt64";
				if (type == typeof(Byte))       return "ConvertToByte";
				if (type == typeof(UInt16))     return "ConvertToUInt16";
				if (type == typeof(UInt32))     return "ConvertToUInt32";
				if (type == typeof(UInt64))     return "ConvertToUInt64";
				if (type == typeof(Char))       return "ConvertToChar";
				if (type == typeof(Single))     return "ConvertToSingle";
				if (type == typeof(Double))     return "ConvertToDouble";
				if (type == typeof(Boolean))    return "ConvertToBoolean";
			}

			if (type == typeof(String))         return "ConvertToString";
			if (type == typeof(DateTime))       return "ConvertToDateTime";
#if FW3
			if (type == typeof(DateTimeOffset)) return "ConvertToDateTimeOffset";
#endif
			if (type == typeof(Decimal))        return "ConvertToDecimal";
			if (type == typeof(Guid))           return "ConvertToGuid";
			if (type == typeof(Stream))         return "ConvertToStream";
			if (type == typeof(XmlReader))      return "ConvertToXmlReader";
			if (type == typeof(XmlDocument))    return "ConvertToXmlDocument";
			if (type == typeof(Byte[]))         return "ConvertToByteArray";
			if (type == typeof(Char[]))         return "ConvertToCharArray";

			if (type == typeof(SqlByte))        return "ConvertToSqlByte";
			if (type == typeof(SqlInt16))       return "ConvertToSqlInt16";
			if (type == typeof(SqlInt32))       return "ConvertToSqlInt32";
			if (type == typeof(SqlInt64))       return "ConvertToSqlInt64";
			if (type == typeof(SqlSingle))      return "ConvertToSqlSingle";
			if (type == typeof(SqlBoolean))     return "ConvertToSqlBoolean";
			if (type == typeof(SqlDouble))      return "ConvertToSqlDouble";
			if (type == typeof(SqlDateTime))    return "ConvertToSqlDateTime";
			if (type == typeof(SqlDecimal))     return "ConvertToSqlDecimal";
			if (type == typeof(SqlMoney))       return "ConvertToSqlMoney";
			if (type == typeof(SqlGuid))        return "ConvertToSqlGuid";
			if (type == typeof(SqlString))      return "ConvertToSqlString";

			return null;
		}
	}
}
