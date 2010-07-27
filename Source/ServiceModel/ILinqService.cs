using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace BLToolkit.ServiceModel
{
	[ServiceContract]
	public interface ILinqService
	{
		[OperationContract]
		string GetSqlProviderType();

		[OperationContract, LinqServiceDataContractFormat] int               ExecuteNonQuery(LinqServiceQuery query);
		[OperationContract, LinqServiceDataContractFormat] object            ExecuteScalar  (LinqServiceQuery query);
		[OperationContract, LinqServiceDataContractFormat] LinqServiceResult ExecuteReader  (LinqServiceQuery query);
	}

	class LinqServiceDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
	{
		public LinqServiceDataContractSerializerOperationBehavior(OperationDescription operationDescription)
			: base(operationDescription) { }

		public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
		{
			if (type == typeof(LinqServiceQuery))  return new LinqServiceSerializer.XmlQuerySerializer();
			if (type == typeof(LinqServiceResult)) return new LinqServiceSerializer.XmlResultSerializer();

			return base.CreateSerializer(type, name, ns, knownTypes);
		}

		public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
		{
			if (type == typeof(LinqServiceQuery))  return new LinqServiceSerializer.XmlQuerySerializer();
			if (type == typeof(LinqServiceResult)) return new LinqServiceSerializer.XmlResultSerializer();

			return base.CreateSerializer(type, name, ns, knownTypes);
		}
	}

	class LinqServiceDataContractFormatAttribute : Attribute, IOperationBehavior
	{
		public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
		{
		}
	 
		public void ApplyClientBehavior(OperationDescription description,System.ServiceModel.Dispatcher.ClientOperation proxy)
		{
			IOperationBehavior innerBehavior = new LinqServiceDataContractSerializerOperationBehavior(description);
			innerBehavior.ApplyClientBehavior(description, proxy);
		}
	 
		public void ApplyDispatchBehavior(OperationDescription description,System.ServiceModel.Dispatcher.DispatchOperation dispatch)
		{
			IOperationBehavior innerBehavior = new LinqServiceDataContractSerializerOperationBehavior(description);
			innerBehavior.ApplyDispatchBehavior(description, dispatch);
		}
	 
		public void Validate(OperationDescription description)
		{
		}
	}
}
