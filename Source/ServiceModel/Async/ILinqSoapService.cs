using System;
using System.ServiceModel;

namespace BLToolkit.ServiceModel.Async
{
	[ServiceContract]
	[ServiceKnownType(typeof(LinqServiceQuery))]
	[ServiceKnownType(typeof(LinqServiceResult))]
	public interface ILinqSoapService
	{
		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/GetSqlProviderType", ReplyAction = "http://tempuri.org/GetSqlProviderTypeResponse")]
		IAsyncResult BeginGetSqlProviderType(AsyncCallback callback, object asyncState);

		string EndGetSqlProviderType(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ExecuteNonQuery", ReplyAction = "http://tempuri.org/ExecuteNonQueryResponse")]
		IAsyncResult BeginExecuteNonQuery(string queryData, AsyncCallback callback, object asyncState);

		int EndExecuteNonQuery(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ExecuteScalar", ReplyAction = "http://tempuri.org/ExecuteScalarResponse")]
		IAsyncResult BeginExecuteScalar(string queryData, AsyncCallback callback, object asyncState);

		object EndExecuteScalar(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ExecuteReader", ReplyAction = "http://tempuri.org/ExecuteReaderResponse")]
		IAsyncResult BeginExecuteReader(string queryData, AsyncCallback callback, object asyncState);

		string EndExecuteReader(IAsyncResult result);

		[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ExecuteBatch", ReplyAction = "http://tempuri.org/ExecuteBatchResponse")]
		IAsyncResult BeginExecuteBatch(string queryData, AsyncCallback callback, object asyncState);

		int EndExecuteBatch(IAsyncResult result);
	}
}
