using System;
using System.Collections.Generic;
using System.Data;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace BLToolkit.DataAccess
{
	public class SqlQueryInfo
	{
		public SqlQueryInfo(ObjectMapper objectMapper)
		{
			ObjectMapper = objectMapper;
		}

		public string       QueryText    { get; set; }
		public ObjectMapper ObjectMapper { get; private set; }

		public Type ObjectType
		{
			get { return ObjectMapper.TypeAccessor.OriginalType; }
		}

		private readonly List<SqlQueryParameterInfo> _parameters = new List<SqlQueryParameterInfo>();

		public SqlQueryParameterInfo AddParameter(string parameterName, string fieldName)
		{
			var parameter = new SqlQueryParameterInfo { ParameterName = parameterName, FieldName = fieldName };

			parameter.SetMemberMapper(ObjectMapper);

			_parameters.Add(parameter);

			return parameter;
		}

		public IDbDataParameter[] GetParameters(DbManager db, object[] key)
		{
			if (_parameters.Count != key.Length)
				throw new DataAccessException("Parameter list does match key list.");

			var parameters = new IDbDataParameter[_parameters.Count];

			for (var i = 0; i < _parameters.Count; i++)
			{
				var info = _parameters[i];

				parameters[i] = db.Parameter(info.ParameterName, key[i]);
			}

			return parameters;
		}

		public IDbDataParameter[] GetParameters(DbManager db, object obj)
		{
			var parameters = new IDbDataParameter[_parameters.Count];

			for (var i = 0; i < _parameters.Count; i++)
			{
				var info = _parameters[i];

				//parameters[i] = db.Parameter(info.ParameterName, info.MemberMapper.GetValue(obj));

				var mmi = info.MemberMapper.MapMemberInfo;
				var val = info.MemberMapper.GetValue(obj);

				if (val == null && mmi.Nullable/* && mmi.NullValue == null*/)
				{
					//replace value with DbNull
					val = DBNull.Value;
				}

				if (mmi.IsDbTypeSet)
				{
					parameters[i] = mmi.IsDbSizeSet 
						? db.Parameter(info.ParameterName, val, info.MemberMapper.DbType, mmi.DbSize) 
						: db.Parameter(info.ParameterName, val, info.MemberMapper.DbType);
				}
				else
				{
					parameters[i] = db.Parameter(info.ParameterName, val);
				}
			}

			return parameters;
		}

		public MemberMapper[] GetMemberMappers()
		{
			var members = new MemberMapper[_parameters.Count];

			for (var i = 0; i < _parameters.Count; i++)
				members[i] = _parameters[i].MemberMapper;

			return members;
		}
	}
}
