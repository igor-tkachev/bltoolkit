using System;
using System.Collections;
using System.Data;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace BLToolkit.DataAccess
{
	public class SqlQueryInfo
	{
		public SqlQueryInfo(ObjectMapper objectMapper)
		{
			_objectMapper = objectMapper;
		}

		private string _queryText;
		public  string  QueryText
		{
			get { return _queryText;  }
			set { _queryText = value; }
		}

		private readonly ObjectMapper _objectMapper;
		public           ObjectMapper  ObjectMapper
		{
			get { return _objectMapper; }
		}

		public Type ObjectType
		{
			get { return _objectMapper.TypeAccessor.OriginalType; }
		}

		private readonly ArrayList _parameters = new ArrayList();

		public SqlQueryParameterInfo AddParameter(string parameterName, string fieldName)
		{
			SqlQueryParameterInfo parameter = new SqlQueryParameterInfo();

			parameter.ParameterName = parameterName;
			parameter.FieldName     = fieldName;

			parameter.SetMemberMapper(_objectMapper);

			_parameters.Add(parameter);

			return parameter;
		}

		public IDbDataParameter[] GetParameters(DbManager db, object[] key)
		{
			if (_parameters.Count != key.Length)
				throw new DataAccessException("Parameter list does match key list.");

			IDbDataParameter[] parameters = new IDbDataParameter[_parameters.Count];

			for (int i = 0; i < _parameters.Count; i++)
			{
				SqlQueryParameterInfo info = (SqlQueryParameterInfo)_parameters[i];

				parameters[i] = db.Parameter(info.ParameterName, key[i]);
			}

			return parameters;
		}

		public IDbDataParameter[] GetParameters(DbManager db, object obj)
		{
			IDbDataParameter[] parameters = new IDbDataParameter[_parameters.Count];

			for (int i = 0; i < _parameters.Count; i++)
			{
				SqlQueryParameterInfo info = (SqlQueryParameterInfo)_parameters[i];

				parameters[i] = db.Parameter(info.ParameterName, info.MemberMapper.GetValue(obj));
			}

			return parameters;
		}
	}
}
