using System;
using System.ServiceModel;

namespace BLToolkit.ServiceModel
{
	[ServiceContract]
	public interface ILinqService
	{
		[OperationContract]
		string GetSqlProviderType();

		[OperationContract] int               ExecuteNonQuery(LinqServiceQuery query);
		[OperationContract] object            ExecuteScalar  (LinqServiceQuery query);
		[OperationContract] LinqServiceResult ExecuteReader  (LinqServiceQuery query);
	}
}
