using System;
using System.Collections.Generic;
using System.Data.Services.Providers;

namespace BLToolkit.ServiceModel
{
	class DataServiceMetadataProvider : IDataServiceMetadataProvider
	{
		public bool TryResolveResourceSet(string name, out ResourceSet resourceSet)
		{
			throw new NotImplementedException();
		}

		public ResourceAssociationSet GetResourceAssociationSet(ResourceSet resourceSet, ResourceType resourceType, ResourceProperty resourceProperty)
		{
			throw new NotImplementedException();
		}

		public bool TryResolveResourceType(string name, out ResourceType resourceType)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ResourceType> GetDerivedTypes(ResourceType resourceType)
		{
			throw new NotImplementedException();
		}

		public bool HasDerivedTypes(ResourceType resourceType)
		{
			throw new NotImplementedException();
		}

		public bool TryResolveServiceOperation(string name, out ServiceOperation serviceOperation)
		{
			throw new NotImplementedException();
		}

		public string ContainerNamespace
		{
			get { throw new NotImplementedException(); }
		}

		public string ContainerName
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<ResourceSet> ResourceSets
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<ResourceType> Types
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<ServiceOperation> ServiceOperations
		{
			get { throw new NotImplementedException(); }
		}
	}
}
