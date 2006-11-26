using System;
using System.Collections;
using System.Diagnostics;

using BLToolkit.Mapping;

namespace BLToolkit.Reflection
{
	public class InitContext
	{
		private object[] _memberParameters;
		public  object[]  MemberParameters
		{
			[DebuggerStepThrough] get { return _memberParameters;  }
			[DebuggerStepThrough] set { _memberParameters = value; }
		}

		private object[] _parameters;
		public  object[]  Parameters
		{
			[DebuggerStepThrough] get { return _parameters;  }
			[DebuggerStepThrough] set { _parameters = value; }
		}

		private bool _isInternal;
		public  bool  IsInternal
		{
			[DebuggerStepThrough] get { return _isInternal;  }
			[DebuggerStepThrough] set { _isInternal = value; }
		}

		private bool _isLazyInstance;
		public  bool  IsLazyInstance
		{
			[DebuggerStepThrough] get { return _isLazyInstance;  }
			[DebuggerStepThrough] set { _isLazyInstance = value; }
		}

		private object _parent;
		public  object  Parent
		{
			[DebuggerStepThrough] get { return _parent;  }
			[DebuggerStepThrough] set { _parent = value; }
		}

		private Hashtable _items;
		public  Hashtable  Items
		{
			[DebuggerStepThrough] 
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
			[DebuggerStepThrough] get { return _stopMapping;  }
			[DebuggerStepThrough] set { _stopMapping = value; }
		}

		private IMapDataSource _dataSource;
		[CLSCompliant(false)]
		public  IMapDataSource  DataSource
		{
			[DebuggerStepThrough] get { return _dataSource;  }
			[DebuggerStepThrough] set { _dataSource = value; }
		}

		private object _sourceObject;
		public 	object  SourceObject
		{
			[DebuggerStepThrough] get { return _sourceObject;  }
			[DebuggerStepThrough] set { _sourceObject = value; }
		}

		private ObjectMapper _objectMapper;
		public  ObjectMapper  ObjectMapper
		{
			[DebuggerStepThrough] get { return _objectMapper;  }
			[DebuggerStepThrough] set { _objectMapper = value; }
		}

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			[DebuggerStepThrough] get { return _mappingSchema;  }
			[DebuggerStepThrough] set { _mappingSchema = value; }
		}

		private bool _isSource;
		public  bool  IsSource
		{
			[DebuggerStepThrough] get { return _isSource;  }
			[DebuggerStepThrough] set { _isSource = value; }
		}

		public  bool  IsDestination
		{
			[DebuggerStepThrough] get { return !_isSource;  }
			[DebuggerStepThrough] set { _isSource = !value; }
		}
	}
}
