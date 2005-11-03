using System;
using System.Collections;

namespace BLToolkit.Reflection
{
	public class InitContext
	{
		public InitContext()
		{
		}

		private object [] _memberParameters;
		public  object []  MemberParameters
		{
			get { return _memberParameters;  }
			set { _memberParameters = value; }
		}

		private object [] _Parameters;
		public  object []  Parameters
		{
			get { return _Parameters;  }
			set { _Parameters = value; }
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
	}
}
