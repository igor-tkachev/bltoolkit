using System;
using System.ServiceModel;

using BLToolkit.Data.Sql;

namespace BLToolkit.ServiceModel
{
	[ServiceContract]
	public interface ILinqService
	{
		[OperationContract]
		string GetSqlProviderType();

		[OperationContract]
		int ExecuteNonQuery(SqlQuery query, SqlParameter[] parameters);
	}
}
