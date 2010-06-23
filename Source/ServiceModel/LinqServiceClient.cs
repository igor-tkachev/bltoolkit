using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace BLToolkit.ServiceModel
{
	class LinqServiceClient : ClientBase<ILinqService>, ILinqService, IDisposable
	{
		#region Init

		//public LinqServiceClient() {}
		public LinqServiceClient(string endpointConfigurationName)                                                                  : base(endpointConfigurationName) { }
		public LinqServiceClient(string endpointConfigurationName, string remoteAddress)                                            : base(endpointConfigurationName, remoteAddress) { }
		public LinqServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)                                   : base(endpointConfigurationName, remoteAddress) { }
		public LinqServiceClient(Binding binding, EndpointAddress remoteAddress)                                                    : base(binding, remoteAddress) { }
		//public LinqServiceClient(InstanceContext callbackInstance)                                                                  : base(callbackInstance) { }
		//public LinqServiceClient(InstanceContext callbackInstance, string endpointConfigurationName)                                : base(callbackInstance, endpointConfigurationName) { }
		//public LinqServiceClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)          : base(callbackInstance, endpointConfigurationName, remoteAddress) { }
		//public LinqServiceClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress) { }
		//public LinqServiceClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)                  : base(callbackInstance, binding, remoteAddress) { }

		#endregion

		#region ILinqService Members

		public Type GetSqlProviderType()
		{
			return Channel.GetSqlProviderType();
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			try
			{
				if (State != CommunicationState.Faulted)
					Close();
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
	}
}
