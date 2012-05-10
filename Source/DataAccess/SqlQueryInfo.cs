using System;
using System.Collections.Generic;
using System.Data;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public class SqlQueryInfo
	{
		//NOTE Added empty constructor
		public SqlQueryInfo()
		{
		}

		public SqlQueryInfo(ObjectMapper objectMapper)
		{
			ObjectMapper = objectMapper;
		}

        public QueryType QueryType { get; set; }
        public string OwnerName { get; set; }
		public string       QueryText    { get; set; }
		public ObjectMapper ObjectMapper { get; private set; }

		public Type ObjectType
		{
			get { return ObjectMapper.TypeAccessor.OriginalType; }
		}

		//NOTE Changed from private to protected
		protected readonly List<SqlQueryParameterInfo> Parameters = new List<SqlQueryParameterInfo>();

		//NOTE Changed to virtual
		public virtual SqlQueryParameterInfo AddParameter(string parameterName, string fieldName)
		{
			var parameter = new SqlQueryParameterInfo { ParameterName = parameterName, FieldName = fieldName };

			parameter.SetMemberMapper(ObjectMapper);

			Parameters.Add(parameter);

			return parameter;
		}

		public IDbDataParameter[] GetParameters(DbManager db, object[] key)
		{
			if (Parameters.Count != key.Length)
				throw new DataAccessException("Parameter list does match key list.");

			var parameters = new IDbDataParameter[Parameters.Count];

			for (var i = 0; i < Parameters.Count; i++)
			{
				var info = Parameters[i];

				parameters[i] = db.Parameter(info.ParameterName, key[i]);
			}

			return parameters;
		}

		public IDbDataParameter[] GetParameters(DbManager db, object obj)
		{
			var parameters = new IDbDataParameter[Parameters.Count];

			for (var i = 0; i < Parameters.Count; i++)
			{
				var info = Parameters[i];

				//parameters[i] = db.Parameter(info.ParameterName, info.MemberMapper.GetValue(obj));

				var mmi = info.MemberMapper.MapMemberInfo;

				var val = info.MemberMapper.GetValue(obj);
			    var mp = info.MemberMapper.MappingSchema.MetadataProvider;
			    var type = info.MemberMapper.Type;
                var typeExt = TypeExtension.GetTypeExtension(type, new ExtensionList());

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

                bool isPkSet;

                mp.GetPrimaryKeyOrder(type, typeExt, mmi.MemberAccessor, out isPkSet);
                if (isPkSet)
                {
                    bool isGeneratorSet;
                    KeyGeneratorInfo genInfo = mp.GetGeneratorType(typeExt, mmi.MemberAccessor, out isGeneratorSet);
                    if (isGeneratorSet)
                    {
                        if (genInfo.GeneratorType == PrimaryKeyGeneratorType.Sequence)
                        {
                            bool isSeqSet;
                            string sequenceName = mp.GetSequenceName(typeExt, mmi.MemberAccessor, out isSeqSet);
                            if (!isSeqSet)
                                throw new Exception("Sequence atribute is not present!");
                            if (string.IsNullOrWhiteSpace(sequenceName))
                                throw new Exception("SequenceName is empty");

                            if (!genInfo.RetrievePkValue)
                            {
                                throw new Exception("Not implemented");
                                //TODO Use base provider
                                val = string.Format("{0}.NEXTVAL", sequenceName);

                                parameters[i] = db.Parameter(info.ParameterName, val, DbType.String);
                            }
                            else
                            {                               
                                string seqQuery =  db.DataProvider.GetSequenceQuery(sequenceName, OwnerName);
                                val = db.SetCommand(seqQuery).ExecuteScalar();
                                parameters[i] = db.Parameter(info.ParameterName, val);
                            }
                        }
                    }
                }
			}

			return parameters;
		}

		public MemberMapper[] GetMemberMappers()
		{
            var members = new MemberMapper[Parameters.Count];

            for (var i = 0; i < Parameters.Count; i++)
                members[i] = Parameters[i].MemberMapper;

            return members;
		}
	}
}
