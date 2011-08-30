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

		public int ExecuteNonQuery(LinqServiceQuery query)
		{
			var async = Channel.BeginExecuteNonQuery(query, null, null);
			return Channel.EndExecuteNonQuery(async);
		}

		public object ExecuteScalar(LinqServiceQuery query)
		{
			var async = Channel.BeginExecuteScalar(query, null, null);
			return Channel.EndExecuteScalar(async);
		}

		public LinqServiceResult ExecuteReader(LinqServiceQuery query)
		{
			var async = Channel.BeginExecuteReader(query, null, null);
			return Channel.EndExecuteReader(async);
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

			public IAsyncResult BeginExecuteNonQuery(LinqServiceQuery query, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteNonQuery", new object[] { query }, callback, asyncState);
			}

			public int EndExecuteNonQuery(IAsyncResult result)
			{
				return (int)EndInvoke("ExecuteNonQuery", new object[0], result);
			}

			public IAsyncResult BeginExecuteScalar(LinqServiceQuery query, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteScalar", new object[] { query }, callback, asyncState);
			}

			public object EndExecuteScalar(IAsyncResult result)
			{
				return (object)base.EndInvoke("ExecuteScalar", new object[0], result);
			}

			public IAsyncResult BeginExecuteReader(LinqServiceQuery query, AsyncCallback callback, object asyncState)
			{
				return BeginInvoke("ExecuteReader", new object[] { query }, callback, asyncState);
			}

			public LinqServiceResult EndExecuteReader(IAsyncResult result)
			{
				return (LinqServiceResult)base.EndInvoke("ExecuteReader", new object[0], result);
			}
		}

		#endregion
	}
}
