using System;
using System.Collections;

using BLToolkit.Mapping;

namespace BLToolkit.Reflection
{
	public class InitContext
	{
		public InitContext()
		{
		}

		private object[] _memberParameters;
		public  object[]  MemberParameters
		{
			get { return _memberParameters;  }
			set { _memberParameters = value; }
		}

		private object[] _parameters;
		public  object[]  Parameters
		{
			get { return _parameters;  }
			set { _parameters = value; }
		}

		private bool _isInternal;
		public  bool  IsInternal
		{
			get { return _isInternal;  }
			set { _isInternal = value; }
		}

		private bool _isLazyInstance;
		public  bool  IsLazyInstance
		{
			get { return _isLazyInstance;  }
			set { _isLazyInstance = value; }
		}

		private object _parent;
		public  object  Parent
		{
			get { return _parent;  }
			set { _parent = value; }
		}

		private Hashtable _items;
		private Hashtable  Items
		{
			get 
			{
				if (_items == null)
					_items = new Hashtable();

				return _items;
			}
		}

		private bool _stopMapping;
		public  bool  StopMapping
		{
			get { return _stopMapping;  }
			set { _stopMapping = value; }
		}

		private IObjectMapper _objectMapper;
		public  IObjectMapper  ObjectMapper
		{
			get { return _objectMapper;  }
			set { _objectMapper = value; }
		}

		private IMapDataSource _mapDataSource;
		public  IMapDataSource  MapDataSource
		{
			get { return _mapDataSource;  }
			set { _mapDataSource = value; }
		}

		private object _sourceObject;
		public 	object  SourceObject
		{
			get { return _sourceObject;  }
			set { _sourceObject = value; }
		}
	}
}
