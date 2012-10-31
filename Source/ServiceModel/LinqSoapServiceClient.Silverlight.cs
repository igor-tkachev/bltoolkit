using System;
using System.ServiceModel;

namespace BLToolkit.ServiceModel
{
	class LinqSoapServiceClient : ClientBase<Async.ILinqSoapService>, ILinqService, IDisposable
	{
		#region Init

		public LinqSoapServiceClient(string endpointConfigurationName)                                            : base(endpointConfigurationName) { }
		public LinqSoapServiceClient(string endpointConfigurationName, string remoteAddress)                      : base(endpointConfigurationName, remoteAddress) { }
		public LinqSoapServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)             : base(endpointConfigurationName, remoteAddress) { }
		public LinqSoapServiceClient(System.ServiceModel.Channels.Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }

		#endregion

		#region ILinqService Members

		public string GetSqlProviderType()
		{
			var async = Channel.BeginGetSqlProviderType(null, null);
			return Channel.EndGetSqlProviderType(async);
		}

		public int ExecuteNonQuery(string queryData)
		{
			var async = Channel.BeginExecuteNonQuery(queryData, null, null);
			return Channel.EndExecuteNonQuery(async);
		}

		public object ExecuteScalar(string queryData)
		{
			var async = Channel.BeginExecuteScalar(queryData, null, null);
			return Channel.EndExecuteScalar(async);
		}

		public string ExecuteReader(string queryData)
		{
			var async = Channel.BeginExecuteReader(queryData, null, null);
			return Channel.EndExecuteReader(async);
		}

		public int ExecuteBatch(string queryData)
		{
			var async = Channel.BeginExecuteBatch(queryData, null, null);
			return Channel.EndExecuteBatch(async);
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			try
			{
				if (State != CommunicationState.Faulted)
					((ICommunicationObject)this).Close();
				else
					Abort();
			}
			catch (CommunicationException)
			{
				Abort();
			}
			catch (TimeoutException)
			{
				Abort();
			}
			catch (Exception)
			{
				Abort();
				throw;
			}
		}

		#endregion

		#region Overrides

		protected override Async.ILinqSoapService CreateChannel()
		{
			return new LinqSoapServiceClientChannel(this);
		}

		#endregion

		#region Channel

		class LinqSoapServiceClientChannel : ChannelBase<Async.ILinqSoapService>, Async.ILinqSoapService
		{
			public LinqSoapServiceClientChannel(ClientBase<Async.ILinqSoapService> client) :
				base(client)
			{
			}

			public IAsyncResult BeginGetSqlProviderType(AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("GetSqlProviderType", new object[0], callback, asyncState);
			}

			public string EndGetSqlProviderType(IAsyncResult result)
			{
				return (string)EndInvoke("GetSqlProviderType", new object[0], result);
			}

			public IAsyncResult BeginExecuteNonQuery(string queryData, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteNonQuery", new object[] { queryData }, callback, asyncState);
			}

			public int EndExecuteNonQuery(IAsyncResult result)
			{
				return (int)EndInvoke("ExecuteNonQuery", new object[0], result);
			}

			public IAsyncResult BeginExecuteScalar(string queryData, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteScalar", new object[] { queryData }, callback, asyncState);
			}

			public object EndExecuteScalar(IAsyncResult result)
			{
				return EndInvoke("ExecuteScalar", new object[0], result);
			}

			public IAsyncResult BeginExecuteReader(string queryData, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteReader", new object[] { queryData }, callback, asyncState);
			}

			public string EndExecuteReader(IAsyncResult result)
			{
				return (string)EndInvoke("ExecuteReader", new object[0], result);
			}

			public IAsyncResult BeginExecuteBatch(string queryData, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteBatch", new object[] { queryData }, callback, asyncState);
			}

			public int EndExecuteBatch(IAsyncResult result)
			{
				return (int)EndInvoke("ExecuteBatch", new object[0], result);
			}
		}

		#endregion
	}
}
