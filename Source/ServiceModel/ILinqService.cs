using System;
using System.ServiceModel;

namespace BLToolkit.ServiceModel
{
	[ServiceContract]
	public interface ILinqService
	{
		[OperationContract]
		Type GetSqlProviderType();
	}
}
