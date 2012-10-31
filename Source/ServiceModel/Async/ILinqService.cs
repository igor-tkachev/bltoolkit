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

		string EndGetSqlProviderType(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteNonQuery", ReplyAction = "http://tempuri.org/ILinqService/ExecuteNonQueryResponse")]
		IAsyncResult BeginExecuteNonQuery(string queryData, AsyncCallback callback, object asyncState);

		int EndExecuteNonQuery(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteScalar", ReplyAction = "http://tempuri.org/ILinqService/ExecuteScalarResponse")]
		IAsyncResult BeginExecuteScalar(string queryData, AsyncCallback callback, object asyncState);

		object EndExecuteScalar(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteReader", ReplyAction = "http://tempuri.org/ILinqService/ExecuteReaderResponse")]
		IAsyncResult BeginExecuteReader(string queryData, AsyncCallback callback, object asyncState);

		string EndExecuteReader(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ILinqService/ExecuteBatch", ReplyAction = "http://tempuri.org/ILinqService/ExecuteBatchResponse")]
		IAsyncResult BeginExecuteBatch(string queryData, AsyncCallback callback, object asyncState);

		int EndExecuteBatch(IAsyncResult result);
	}
}
