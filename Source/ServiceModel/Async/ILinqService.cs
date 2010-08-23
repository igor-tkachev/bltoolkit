using System;
using System.ServiceModel;

namespace BLToolkit.ServiceModel.Async
{
	[ServiceContract]
	[ServiceKnownType(typeof(LinqServiceQuery))]
	[ServiceKnownType(typeof(LinqServiceResult))]
	public interface ILinqService
	{
		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/GetSqlProviderType", ReplyAction = "http://tempuri.org/ILinqService/GetSqlProviderTypeResponse")]
		IAsyncResult BeginGetSqlProviderType(AsyncCallback callback, object asyncState);

		string EndGetSqlProviderType(System.IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteNonQuery", ReplyAction = "http://tempuri.org/ILinqService/ExecuteNonQueryResponse")]
		IAsyncResult BeginExecuteNonQuery(LinqServiceQuery query, AsyncCallback callback, object asyncState);

		int EndExecuteNonQuery(System.IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteScalar", ReplyAction = "http://tempuri.org/ILinqService/ExecuteScalarResponse")]
		IAsyncResult BeginExecuteScalar(LinqServiceQuery query, AsyncCallback callback, object asyncState);

		object EndExecuteScalar(System.IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteReader", ReplyAction = "http://tempuri.org/ILinqService/ExecuteReaderResponse")]
		IAsyncResult BeginExecuteReader(LinqServiceQuery query, AsyncCallback callback, object asyncState);

		LinqServiceResult EndExecuteReader(IAsyncResult result);
	}
}
